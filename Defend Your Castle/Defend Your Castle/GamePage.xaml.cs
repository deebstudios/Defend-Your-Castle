﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MonoGame.Framework;
using Windows.ApplicationModel.Activation;


namespace Defend_Your_Castle
{
    /// <summary>
    /// The root page used to display the game.
    /// </summary>
    public sealed partial class GamePage : SwapChainBackgroundPanel
    {
        readonly Game1 _game;

        public double InnerHPBarWidth;

        public GamePage(LaunchActivatedEventArgs args)
        {
            this.InitializeComponent();

            // Create the game.
            _game = XamlGame<Game1>.Create(args, Window.Current.CoreWindow, this);

            // Set the GamePage of Game1 to the current class
            _game.GamePage = this;
        }

        private void SwapChainBackgroundPanel_Loaded(object sender, RoutedEventArgs e)
        {
            // Create a new Title Screen
            TitleScreen screen = new TitleScreen(_game.GamePage, _game);
            
            // Add the Title Screen to the MenuScreens Stack
            _game.AddScreen(screen);

            // Get the InnerHPBarWidth
            InnerHPBarWidth = HUD_InnerHPBar.Width;
        }

        private void ChangeWeaponButton(object sender, RoutedEventArgs e)
        {
            // Get the weapon button that was clicked
            Button WeaponButton = (Button)sender;

            // Switch the player's weapon
            _game.level.GetPlayer.SwitchWeapon(Convert.ToInt32(WeaponButton.Tag));
        }

        private void PauseGame(object sender, RoutedEventArgs e)
        {
            // Check if the game is NOT paused
            if (_game.GameState == GameState.InGame)
            {
                // Pause the game
                _game.ChangeGameState(GameState.Paused);
            }
            else if (_game.GameState == GameState.Paused) // The game is paused
            {
                // Resume the game
                _game.ChangeGameState(GameState.InGame);
            }
        }

        private void ShopItem_Click(object sender, RoutedEventArgs e)
        {
            // Get the Button that was clicked
            Button TheButton = (Button)sender;

            // Get the ShopItem associated with the Button
            ShopItem shopItem = (ShopItem)TheButton.DataContext;

            // Buy the item if possible
            _game.shop.BuyItem(shopItem);
        }

        private void PauseMenuOption1_Click(object sender, RoutedEventArgs e)
        {
            _game.ChangeGameState(GameState.Shop);
            PauseMenu.Visibility = Visibility.Collapsed;
        }

        private void ShowShop(object sender, RoutedEventArgs e)
        {
            // Hide the level end screen
            LevelEnd.Visibility = Visibility.Collapsed;
            
            // Show the shop
            Shop.Visibility = Visibility.Visible;
        }

        private void StartNextLevel(object sender, RoutedEventArgs e)
        {
            // Start the next level
            _game.level.StartNextLevel();

            // Set the level start message's text to the level number
            LevelStart_Message.Text = "Level " + _game.level.GetLevelNum;

            // Show the level animation
            LevelStart_Anim.Begin();
        }

        private void SaveGame(object sender, RoutedEventArgs e)
        {
            // Save the player
            Data.SavePlayer(_game.level.GetPlayer);
        }


    }
}