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

namespace Defend_Your_Castle
{
    public class TitleScreen : MenuScreen
    {
        public TitleScreen(GamePage page, Game1 game) : base(page, game)
        {
            // Create the Buttons for the Title screen
            Button StartGame = CreateButton("Start Game", 250);
            Button ContinueGame = CreateButton("Continue Game", 250);
            Button Options = CreateButton("Options", 250);

            // Add additional spacing between all of the Buttons
            ContinueGame.Margin = new Thickness(0, 20, 0, 20);

            // Create a vertical menu containing all of the Buttons
            StackPanel VerticalMenu = CreateVerticalMenu(StartGame, ContinueGame, Options);
            
            // Add the vertical menu to the controls on the screen
            Controls.Add(VerticalMenu);

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