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
            
            AllControls.Add(StartGame);
            AllControls.Add(Options);

            AddMenuOption(StartGame);
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
                case 1: // Options
                    Game.AddScreen(new OptionsScreen(GamePage, Game));
                    break;
            }
        }


    }
}