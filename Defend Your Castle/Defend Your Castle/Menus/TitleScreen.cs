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
            CursorOffset = new Vector2(40, 0);

            TextBlock StartGame = CreateLabel("Start Game", new Vector2(50, 50));
            TextBlock Options = CreateLabel("Options", new Vector2(50, 100));
            TextBlock Quit = CreateLabel("Quit", new Vector2(50, 150));

            AllControls.Add(StartGame);
            AllControls.Add(Options);
            AllControls.Add(Quit);

            AddMenuOption(StartGame);
            AddMenuOption(Options);
            AddMenuOption(Quit);
            
            SetCursorPosition();
        }

        protected override void PickOption()
        {
            switch (SelectedOption)
            {
                case 0: // Start Game
                    // Start the game
                    //Game.StartGame();
                    break;
                case 1: // Options
                    Game.AddScreen(new OptionsScreen(GamePage, Game));
                    break;
                case 2: // Quit
                    Game.Exit();
                    Application.Current.Exit();
                    break;
            }
        }


    }
}