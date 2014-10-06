using System;
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

        public double HUD_InnerHPBarWidth;
        public double Shop_InnerHPBarWidth;

        public GamePage(LaunchActivatedEventArgs args)
        {
            this.InitializeComponent();

            // Create the game.
            _game = XamlGame<Game1>.Create(args, Window.Current.CoreWindow, this);

            // Set the GamePage of Game1 to the current class
            _game.GamePage = this;

            // Load the player's saved game data
            _game.LoadData();
        }

        private void SwapChainBackgroundPanel_Loaded(object sender, RoutedEventArgs e)
        {
            // Create a new Title Screen
            TitleScreen screen = new TitleScreen(_game.GamePage, _game);
            
            // Add the Title Screen to the MenuScreens Stack
            _game.AddScreen(screen);

            // Store the width of the HP Bars in the HUD and the Shop
            HUD_InnerHPBarWidth = HUD_InnerHPBar.Width;
            Shop_InnerHPBarWidth = Shop_InnerHPBar.Width;
        }

        private void ChangeWeaponButton(object sender, RoutedEventArgs e)
        {
            // Get the weapon radio button that was clicked
            RadioButton WeaponButton = (RadioButton)sender;
            
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

        // Occurs when the "Next" button is clicked on the Level End screen
        private void LevelEnd_Next_Click(object sender, RoutedEventArgs e)
        {
            // Show the shop
            ShowShop();
        }

        public void ShowShop()
        {
            // Hide the level end screen
            LevelEnd.Visibility = Visibility.Collapsed;
            
            // Update the player's Health and Gold in the Shop
            _game.level.GetPlayer.UpdateHealthInShop();
            _game.level.GetPlayer.UpdateGoldAmountInShop();
            
            // Show the shop
            Shop.Visibility = Visibility.Visible;

            // Reset the animation

            // Set the opacity of all the level stats and the "Next" button to 0
            LevelEnd_Kills.Opacity = LevelEnd_HelperKills.Opacity = LevelEnd_TotalKills.Opacity = LevelEnd_AccuracyRate.Opacity =
            LevelEnd_GoldEarned.Opacity = LevelEnd_Next.Opacity = 0;

            // Disable the "Next" button again
            LevelEnd_Next.IsEnabled = false;
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
            // Save the game
            Data.SaveGameData(_game.shop, _game.level);

            // Show the animation that the game was saved
            Shop_SaveGameAnim.Begin();
        }


    }
}