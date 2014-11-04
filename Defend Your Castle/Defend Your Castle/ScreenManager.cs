using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
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

        // Stores the max screen number for the How To Play Screen
        public int HTP_MaxScreenNum = 6;

        // Stores the opacity of focused UI elements on the How To Play Screen
        public double HTP_Focused_Opacity = 1.0;

        // Stores the opacity of unfocused UI elements on the How To Play Screen
        public double HTP_Unfocused_Opacity = 0.30;

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
                default:
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
            GamePage.HTP_ExitButton.Click += HowToPlayScreen_Exit;
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

            //Play the title screen music
            SoundManager.PlaySong(LoadAssets.TitleScreenMusic);
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

            // Make sure the screen number doesn't go above the maximum
            if (HTP_ScreenNum > HTP_MaxScreenNum) HTP_ScreenNum = HTP_MaxScreenNum;

            // Hide the left arrow button if the screen number is 1
            GamePage.HTP_LeftArrowButton.Visibility = ((HTP_ScreenNum == 1) ? Visibility.Collapsed : Visibility.Visible);

            // Hide the right arrow button if the screen number is the maximum screen number
            GamePage.HTP_RightArrowButton.Visibility = ((HTP_ScreenNum == HTP_MaxScreenNum) ?
                                                        Visibility.Collapsed : Visibility.Visible);

            // Show the current screen's elements
            ChangeHowToPlayScreen(Visibility.Visible);
        }

        private void HowToPlayScreen_Exit(object sender, RoutedEventArgs e)
        {
            // Change the game state to Screen
            Game.ChangeGameState(GameState.Screen);

            // Reset the How To Play screen
            ResetHowToPlayScreen();

            // Change the screen to the Title Screen
            ChangeScreen(Screens.TitleScreen);
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
                    GamePage.HTP_HUD_Screen2_Goblin.Visibility = visibility;

                    // Check if the screen is being shown
                    if (visibility == Visibility.Visible)
                    {
                        // Create a new MeleeEnemy
                        MeleeEnemy goblin = new MeleeEnemy(Game.level, 0, Game1.ScreenHalf.Y, 0, 1);

                        // Move the goblin in plain view
                        goblin.Move(new Vector2(150, 75));

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

                    // Set the properties of the column
                    SetHUDColumnProperties(visibility, GamePage.HTP_HUD_Screen3_RedBox, GamePage.HTP_HUD_WeaponButtonColumn);

                    break;
                case 4: // Weapon Shortcuts
                    GamePage.HTP_HUD_WeaponButtonColumn.Visibility = visibility;
                    GamePage.HTP_HUD_Screen4_WeaponShortcutInfo.Visibility = visibility;
                    GamePage.HTP_HUD_HealthGoldColumn.Visibility = visibility;
                    GamePage.HTP_HUD_PauseColumn.Visibility = visibility;

                    // Set the properties of the column
                    SetHUDColumnProperties(visibility, GamePage.HTP_HUD_Screen3_RedBox, GamePage.HTP_HUD_WeaponButtonColumn);

                    break;
                case 5: // Health and Gold
                    GamePage.HTP_HUD_WeaponButtonColumn.Visibility = visibility;
                    GamePage.HTP_HUD_HealthGoldColumn.Visibility = visibility;
                    GamePage.HTP_HUD_PauseColumn.Visibility = visibility;
                    GamePage.HTP_HUD_Screen5_HealthGoldInfo.Visibility = visibility;

                    // Set the properties of the column
                    SetHUDColumnProperties(visibility, GamePage.HTP_HUD_Screen5_RedBox, GamePage.HTP_HUD_HealthGoldColumn);

                    break;
                case 6: // Ready to start
                    GamePage.HTP_HUD_Screen6_ReadyToPlay.Visibility = visibility;

                    break;
                default:
                    break;
            }
        }

        // Sets the properties of a HUD column when the screen number is changed
        // This method hides or shows the red box border and highlights or greys out the HUD column
        private void SetHUDColumnProperties(Visibility visibility, Border RedBoxBorder, Grid HUDColumn)
        {
            // Check if the screen is being shown
            if (visibility == Visibility.Visible)
            {
                // Set the red box's border color to Red
                RedBoxBorder.BorderBrush = new SolidColorBrush(Colors.Red);

                // Highlight the column by setting its opacty
                HUDColumn.Opacity = HTP_Focused_Opacity;
            }
            else // The screen is being hidden
            {
                // Set the red box's border color to Transparent
                RedBoxBorder.BorderBrush = new SolidColorBrush(Colors.Transparent);

                // Grey out the column
                HUDColumn.Opacity = HTP_Unfocused_Opacity;
            }
        }

        // Resets the How To Play tutorial to the first screen
        private void ResetHowToPlayScreen()
        {
            // Loop as long as the How To Play screen is past the first screen
            while (HTP_ScreenNum > 1)
            {
                // Hide the current screen
                ChangeHowToPlayScreen(Visibility.Collapsed);

                // Move to the previous screen
                HTP_ScreenNum -= 1;
            }

            // Show the first screen
            ChangeHowToPlayScreen(Visibility.Visible);

            // Hide the left arrow button
            GamePage.HTP_LeftArrowButton.Visibility = Visibility.Collapsed;

            // Show the right arrow button
            GamePage.HTP_RightArrowButton.Visibility = Visibility.Visible;
        }


    }
}