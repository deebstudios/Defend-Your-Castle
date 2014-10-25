using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Microsoft.Xna.Framework;

namespace Defend_Your_Castle
{
    public sealed class ScreenManager
    {
        // An enum for each screen in the game
        public enum Screens : byte { TitleScreen, OptionsScreen, GameOverScreen, HowToPlayScreen };

        // References to Game1.cs and GamePage.cs
        public Game1 Game;
        public GamePage GamePage;

        // Stores the screen number for the How To Play Screen
        public int HTP_ScreenNum = 1;

        public ScreenManager(Game1 game, GamePage gamePage)
        {
            // Get the references
            Game = game;
            GamePage = gamePage;
        }

        // Changes from one screen to another
        public void ChangeScreen(Screens screen)
        {
            switch (screen)
            {
                case Screens.TitleScreen:
                    GamePage.TitleScreen.Visibility = Visibility.Visible;
                    GamePage.OptionsScreen.Visibility = Visibility.Collapsed;
                    GamePage.GameOverScreen.Visibility = Visibility.Collapsed;
                    GamePage.HowToPlayScreen.Visibility = Visibility.Collapsed;

                    break;
                case Screens.OptionsScreen:
                    GamePage.OptionsScreen.Visibility = Visibility.Visible;
                    GamePage.TitleScreen.Visibility = Visibility.Collapsed;
                    GamePage.GameOverScreen.Visibility = Visibility.Collapsed;
                    GamePage.HowToPlayScreen.Visibility = Visibility.Collapsed;

                    break;
                case Screens.GameOverScreen:
                    GamePage.GameOverScreen.Visibility = Visibility.Visible;
                    GamePage.TitleScreen.Visibility = Visibility.Collapsed;
                    GamePage.OptionsScreen.Visibility = Visibility.Collapsed;
                    GamePage.HowToPlayScreen.Visibility = Visibility.Collapsed;

                    break;
                case Screens.HowToPlayScreen:
                    GamePage.HowToPlayScreen.Visibility = Visibility.Visible;
                    GamePage.GameOverScreen.Visibility = Visibility.Collapsed;
                    GamePage.TitleScreen.Visibility = Visibility.Collapsed;
                    GamePage.OptionsScreen.Visibility = Visibility.Collapsed;

                    break;
            }
        }

        // Attaches events, such as Button clicks, to events defined in this class
        // This allows Screen events to be handled cleanly within this class
        public void SetUpEvents()
        {
            // Title Screen
            GamePage.TitleScreen_StartGame.Click += StartGame;
            GamePage.TitleScreen_ContinueGame.Click += ContinueGame;
            GamePage.TitleScreen_Options.Click += TitleScreen_Options_Click;
            GamePage.TitleScreen_HowToPlay.Click += TitleScreen_HowToPlay_Click;

            // Options Screen
            GamePage.OptionsScreen_Back.Click += OptionsScreen_Back_Click;

            // GameOver Screen
            GamePage.GameOverScreen_Continue.Click += GameOverScreen_Continue;

            // How To Play Screen
            GamePage.HTP_LeftArrowButton.Click += HowToPlayScreen_ChangeScreens;
            GamePage.HTP_RightArrowButton.Click += HowToPlayScreen_ChangeScreens;
        }

        // Title Screen

        private void StartGame(object sender, RoutedEventArgs e)
        {
            // Start the game
            Game.StartGame();
        }

        private void ContinueGame(object sender, RoutedEventArgs e)
        {
            // Continue an existing game
            Game.ContinueGame();
        }

        private void TitleScreen_Options_Click(object sender, RoutedEventArgs e)
        {
            // Switch to the Options Screen
            ChangeScreen(Screens.OptionsScreen);
        }

        private void TitleScreen_HowToPlay_Click(object sender, RoutedEventArgs e)
        {
            // Create a new level
            Game.level = new Level(new Player(GamePage), Game);

            // Create a new shop
            Game.shop = new Shop(GamePage, Game.level.GetPlayer);

            // Change the game state to How To Play
            Game.ChangeGameState(GameState.HowToPlay);
        }

        // Options Screen

        private void OptionsScreen_Back_Click(object sender, RoutedEventArgs e)
        {
            // Switch to the Title Screen
            ChangeScreen(Screens.TitleScreen);
        }

        // Game Over Screen

        private void GameOverScreen_Continue(object sender, RoutedEventArgs e)
        {
            // Switch to the Title Screen
            ChangeScreen(Screens.TitleScreen);
        }

        // How To Play Screen

        private void HowToPlayScreen_ChangeScreens(object sender, RoutedEventArgs e)
        {
            // Get the Button that was clicked
            Button btn = (Button)sender;

            // Hide the current screen's elements
            ChangeHowToPlayScreen(Visibility.Collapsed);

            // Increase the screen number if the right arrow button was clicked; otherwise, decrease the screen number
            HTP_ScreenNum += ((btn == GamePage.HTP_RightArrowButton) ? 1 : -1);

            // Make sure the screen number doesn't go below 1
            if (HTP_ScreenNum < 1) HTP_ScreenNum = 1;

            // Hide the left arrow button if the screen number is 1
            GamePage.HTP_LeftArrowButton.Visibility = ((HTP_ScreenNum == 1) ? Visibility.Collapsed : Visibility.Visible);

            // Show the current screen's elements
            ChangeHowToPlayScreen(Visibility.Visible);
        }

        // Changes the display of the How To Play Screen based on the screen number the player is viewing
        private void ChangeHowToPlayScreen(Visibility visibility)
        {
            switch (HTP_ScreenNum)
            {
                case 1: // Intro
                    GamePage.HTP_HUD_Screen1_Intro.Visibility = visibility;

                    break;
                case 2: // Goblin
                    // Check if the screen is being shown
                    if (visibility == Visibility.Visible)
                    {
                        // Create a new MeleeEnemy
                        MeleeEnemy goblin = new MeleeEnemy(Game.level, Game1.ScreenHalf.Y, 0, 1);

                        // Move the goblin in plain view
                        goblin.Move(new Vector2(150, 50));

                        // Add the goblin to the level
                        Game.level.AddEnemy(goblin);
                    }
                    else // The screen is being hidden
                    {
                        // Remove all enemies from the level
                        Game.level.GetEnemies.Clear();
                    }

                    break;
                case 3: // Weapons
                    GamePage.HTP_HUD_WeaponButtonColumn.Visibility = visibility;
                    GamePage.HTP_HUD_Screen3_WeaponInfo.Visibility = visibility;
                    GamePage.HTP_HUD_HealthGoldColumn.Visibility = visibility;
                    GamePage.HTP_HUD_PauseColumn.Visibility = visibility;
                    
                    // Add a border thickness of 3 if the screen is being shown
                    GamePage.HTP_HUD_Screen3_RedBox.BorderThickness = ((visibility == Visibility.Visible) ?
                                                                       new Thickness(3) : new Thickness(0));

                    break;
            }
        }

    }
}