using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace Defend_Your_Castle
{
    public class TitleScreen : MenuScreen
    {
        public TitleScreen(GamePage page, Game1 game) : base(page, game)
        {
            Grid RootGrid = new Grid();

            RowDefinition row = new RowDefinition();
            RowDefinition row2 = new RowDefinition();

            row.Height = new GridLength(1, GridUnitType.Star);
            row2.Height = new GridLength(0.6, GridUnitType.Star);

            RootGrid.RowDefinitions.Add(row);
            RootGrid.RowDefinitions.Add(row2);

            // Create the Buttons for the Title screen
            Button StartGame = CreateButton("Start Game", 250);
            Button ContinueGame = CreateButton("Continue Game", 250);
            Button Options = CreateButton("Options", 250);

            StartGame.HorizontalAlignment = HorizontalAlignment.Center;
            ContinueGame.HorizontalAlignment = HorizontalAlignment.Center;
            Options.HorizontalAlignment = HorizontalAlignment.Center;

            Image Logo = new Image();
            Logo.Source = new BitmapImage(new Uri("ms-appx:/Content/Graphics/DefendYourFortLogo.png"));

            Logo.HorizontalAlignment = HorizontalAlignment.Center;
            Logo.VerticalAlignment = VerticalAlignment.Top;

            // Add a margin to the logo to make it smaller
            Logo.Margin = new Thickness(50);

            // Add additional spacing between all of the Buttons
            ContinueGame.Margin = new Thickness(0, 20, 0, 20);

            // Enable the ContinueGame Button if the user has saved game data; otherwise, disable it
            ContinueGame.IsEnabled = Game1.HasSavedData;

            // Create a vertical menu containing all of the Buttons
            StackPanel VerticalMenu = CreateVerticalMenu(StartGame, ContinueGame, Options);

            // Set the margin
            VerticalMenu.Margin = new Thickness(0, 30, 0, 100);

            // Create a ViewBox
            Viewbox MenuViewBox = new Viewbox();

            // Add the vertical menu to the ViewBox so it scales proportionately on different screen resolutions
            MenuViewBox.Child = VerticalMenu;

            // Put the ViewBox in the second row of the Grid
            MenuViewBox.SetValue(Grid.RowProperty, 1);

            // Add the logo and the ViewBox to the screen
            RootGrid.Children.Add(Logo);
            RootGrid.Children.Add(MenuViewBox);

            // Add the root Grid to the controls on the screen
            Controls.Add(RootGrid);
            
            // Add each of the Buttons as a menu option so they can be selected
            AddMenuOption(StartGame);
            AddMenuOption(ContinueGame);
            AddMenuOption(Options);
        }

        protected override void PickOption()
        {
            switch (SelectedOption)
            {
                case 0: // Start Game
                    Game.StartGame();
                    break;
                case 1: // Continue Game
                    Game.ContinueGame();
                    break;
                case 2: // Options
                    Game.AddScreen(new OptionsScreen(GamePage, Game));
                    break;
            }
        }


    }
}