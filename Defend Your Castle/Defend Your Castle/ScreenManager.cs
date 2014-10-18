using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Defend_Your_Castle
{
    public sealed class ScreenManager
    {
        // An enum for each screen in the game
        public enum Screens : byte { TitleScreen, OptionsScreen, GameOverScreen, HowToPlayScreen };

        // References to Game1.cs and GamePage.cs
        public Game1 Game;
        public GamePage GamePage;

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


    }
}