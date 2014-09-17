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
        }

        private void ChangeWeaponButton(object sender, RoutedEventArgs e)
        {
            // Get the weapon button that was clicked
            Button WeaponButton = (Button)sender;

            // Switch the player's weapon
            _game.level.GetPlayer.SwitchWeapon(Convert.ToInt32(WeaponButton.Tag));
        }


    }
}