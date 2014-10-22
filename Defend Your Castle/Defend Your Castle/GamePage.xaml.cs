using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
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
        }

        private async void SwapChainBackgroundPanel_Loaded(object sender, RoutedEventArgs e)
        {
            // Store the width of the HP Bars in the HUD and the Shop
            Shop_InnerHPBarWidth = Shop_InnerHPBar.Width;
            HUD_InnerHPBarWidth = HUD_InnerHPBar.Width;

            // Load the player's saved game data
            await _game.LoadData();

            // Disable/enable the Continue Game button based on whether or not the player has saved game data
            TitleScreen_ContinueGame.IsEnabled = Game1.HasSavedData;

            // Configure the volume controls
            ConfigureVolumeControls();

            // Initialize the Screen Manager
            _game.screenManager = new ScreenManager(_game, this);

            // Set up the screen events
            _game.screenManager.SetUpEvents();
        }

        // General Methods

        private void ConfigureVolumeControls()
        {
            // Get the stored Music and Sound volumes
            double MusicVolume = (double)(SoundManager.MusicVolume * 20);
            double SoundVolume = (double)(SoundManager.SoundVolume * 20);

            // Set the value for the Music and Sound volume on the Options screen
            OptionsScreen_MusicVolume.Value = MusicVolume;
            OptionsScreen_SoundVolume.Value = SoundVolume;

            // Set the value for the Music and Sound volume on the Pause Menu
            PauseMenu_MusicVolume.Value = MusicVolume;
            PauseMenu_SoundVolume.Value = SoundVolume;

            // Add the ValueChanged events to the Sliders on the Options screen
            OptionsScreen_MusicVolume.ValueChanged += Slider_Volume_ValueChanged;
            OptionsScreen_SoundVolume.ValueChanged += Slider_Volume_ValueChanged;

            // Add the ValueChanged events to the Sliders on the Pause Menu
            PauseMenu_MusicVolume.ValueChanged += Slider_Volume_ValueChanged;
            PauseMenu_SoundVolume.ValueChanged += Slider_Volume_ValueChanged;
        }

        private void Slider_Volume_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            // Get the Slider that had its value changed
            Slider slider = (Slider)sender;

            // Get the volume based on the Slider's value
            float thevol = (float)(slider.Value / 20);

            // Check if the Slider controls the music volume
            if (slider == PauseMenu_MusicVolume || slider == OptionsScreen_MusicVolume)
            {
                // Set the music volume
                SoundManager.SetMusicVolume(thevol);

                // Remove the ValueChanged events from the Sliders on the Options and Pause Menus
                // This is done so that the the ValueChanged event will not proc again for the slider that was updated
                OptionsScreen_MusicVolume.ValueChanged -= Slider_Volume_ValueChanged;
                PauseMenu_MusicVolume.ValueChanged -= Slider_Volume_ValueChanged;

                // Set the value of both music volume Sliders
                OptionsScreen_MusicVolume.Value = slider.Value;
                PauseMenu_MusicVolume.Value = slider.Value;

                // Add the ValueChanged events back to the Sliders
                OptionsScreen_MusicVolume.ValueChanged += Slider_Volume_ValueChanged;
                PauseMenu_MusicVolume.ValueChanged += Slider_Volume_ValueChanged;
            }
            else // The Slider controls the sound volume
            {
                // Set the sound volume
                SoundManager.SetSoundVolume(thevol);
                
                // Remove the ValueChanged events from the Sliders on the Options and Pause Menus
                // This is done so that the the ValueChanged event will not proc again for the slider that was updated
                OptionsScreen_SoundVolume.ValueChanged -= Slider_Volume_ValueChanged;
                PauseMenu_SoundVolume.ValueChanged -= Slider_Volume_ValueChanged;

                // Set the value of both sound volume Sliders
                OptionsScreen_SoundVolume.Value = slider.Value;
                PauseMenu_SoundVolume.Value = slider.Value;

                // Add the ValueChanged events back to the Sliders
                OptionsScreen_SoundVolume.ValueChanged += Slider_Volume_ValueChanged;
                PauseMenu_SoundVolume.ValueChanged += Slider_Volume_ValueChanged;
            }
        }

        // HUD

        private void ChangeWeaponButton(object sender, RoutedEventArgs e)
        {
            // Make sure the player is in game
            if (_game.GameState == GameState.InGame)
            {
                // Get the weapon radio button that was clicked
                RadioButton WeaponButton = (RadioButton)sender;

                // Switch the player's weapon
                _game.level.GetPlayer.SwitchWeapon(Convert.ToInt32(WeaponButton.Tag));
            }
        }

        private void PauseGame(object sender, RoutedEventArgs e)
        {
            // Check if the game is NOT paused
            if (_game.GameState == GameState.InGame)
            {
                // Pause the game
                _game.PauseGame();
            }
            else if (_game.GameState == GameState.Paused) // The game is paused
            {
                // Resume the game
                _game.ChangeGameState(GameState.InGame);
            }
        }

        private void HUD_ConsumablesListItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Get the shop item that was tapped
            ShopItem item = (ShopItem)((Image)sender).DataContext;

            // Use the item while in game
            item.UseItemInGame();

            // Remove the item from the HUD
            HUD_ConsumablesList.Items.Remove(item);
        }

        // Pause Menu

        private void PauseMenu_Resume_Click(object sender, RoutedEventArgs e)
        {
            // Resume the game
            _game.ChangeGameState(GameState.InGame);
        }

        private void PauseMenu_Quit_Click(object sender, RoutedEventArgs e)
        {
            _game.level.QuitLevel();
        }

        // Level End

        // Occurs when the "Next" button is clicked on the Level End screen
        private void LevelEnd_Next_Click(object sender, RoutedEventArgs e)
        {
            // Change the game state to Shop
            _game.ChangeGameState(GameState.Shop);
        }

        // Shop

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

        private void ShopItem_Click(object sender, RoutedEventArgs e)
        {
            // Get the Button that was clicked
            Button TheButton = (Button)sender;

            // Get the ShopItem associated with the Button
            ShopItem shopItem = (ShopItem)TheButton.DataContext;

            // Buy the item if possible
            _game.shop.BuyItem(shopItem);
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

            // State that the player has saved data
            Game1.HasSavedData = true;
        }


    }
}