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
            TextBlock ContinueGame = CreateLabel("Continue Game", new Vector2(50, 100));
            TextBlock Options = CreateLabel("Options", new Vector2(50, 150));
            
            AllControls.Add(StartGame);
            AllControls.Add(ContinueGame);
            AllControls.Add(Options);

            AddMenuOption(StartGame);
            AddMenuOption(ContinueGame);
            AddMenuOption(Options);
            
            SetCursorPosition();
        }

        protected override void PickOption()
        {
            switch (SelectedOption)
            {
                case 0: // Start Game
                    // Start the game
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