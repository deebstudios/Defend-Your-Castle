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

            // Configure the pause menu
            ConfigurePauseMenu();
        }

        private void ChangeWeaponButton(object sender, RoutedEventArgs e)
        {
            // Get the weapon radio button that was clicked
            RadioButton WeaponButton = (RadioButton)sender;
            
            // Switch the player's weapon
            _game.level.GetPlayer.SwitchWeapon(Convert.ToInt32(WeaponButton.Tag));
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

        // Pause Menu

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

        private void ConfigurePauseMenu()
        {
            // Add the dropdown items to the pause menu
            PauseMenu_AddDropdownItems();

            // Set the selected index for the Music and Sound volume
            PauseMenu_MusicVolume.SelectedIndex = (int)Math.Round((SoundManager.MusicVolume * 10));
            PauseMenu_SoundVolume.SelectedIndex = (int)Math.Round((SoundManager.SoundVolume * 10));

            // Add the SelectionChanged events to the ComboBoxes
            PauseMenu_MusicVolume.SelectionChanged += PauseMenu_Volume_SelectionChanged;
            PauseMenu_SoundVolume.SelectionChanged += PauseMenu_Volume_SelectionChanged;
        }

        private void PauseMenu_AddDropdownItems()
        {
            // Add volume choices of 0 to 10 to both the Music and Sound volume ComboBoxes
            for (int i = 0; i <= 10; i++)
            {
                PauseMenu_MusicVolume.Items.Add(i);
                PauseMenu_SoundVolume.Items.Add(i);
            }
        }

        private void PauseMenu_Resume_Click(object sender, RoutedEventArgs e)
        {
            // Resume the game
            _game.ChangeGameState(GameState.InGame);
        }

        private void PauseMenu_Volume_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the ComboBox that had its value changed
            ComboBox box = (ComboBox)sender;

            // Get the volume based on the ComboBox's SelectedIndex
            float thevol = ((float)box.SelectedIndex / 10);

            // Check if the ComboBox controls the music volume
            if (box == PauseMenu_MusicVolume)
                // Set the music volume
                SoundManager.SetMusicVolume(thevol);
            else
                // Set the sound volume
                SoundManager.SetSoundVolume(thevol);
        }

        private void PauseMenu_Quit_Click(object sender, RoutedEventArgs e)
        {
            _game.level.QuitLevel();
        }

        // Level End

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
            
            // Show the next level in the shop
            Shop_NextLevel.Text = "Next Level: " + (_game.level.GetLevelNum + 1);

            // Show the shop
            Shop.Visibility = Visibility.Visible;

            // Reset the effects of the Level End animation

            // Set the opacity of all the level stats and the "Next" button to 0
            LevelEnd_Kills.Opacity = LevelEnd_HelperKills.Opacity = LevelEnd_TotalKills.Opacity = LevelEnd_AccuracyRate.Opacity =
            LevelEnd_BonusGold.Opacity = LevelEnd_GoldEarned.Opacity = LevelEnd_TotalGoldEarned.Opacity = LevelEnd_Next.Opacity = 0;

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