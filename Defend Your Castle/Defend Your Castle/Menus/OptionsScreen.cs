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
    public class OptionsScreen : MenuScreen
    {
        public OptionsScreen(GamePage page, Game1 game) : base(page, game)
        {
            CursorOffset = new Vector2(40, 0);

            TextBlock MusicVol = CreateLabel("Music Volume: ", new Vector2(50, 50));
            ComboBox MusicVolumes = CreateDropdown(new Vector2(200, 50));

            TextBlock SoundVol = CreateLabel("Sound Volume: ", new Vector2(50, 100));
            ComboBox SoundVolumes = CreateDropdown(new Vector2(200, 100));

            TextBlock Back = CreateLabel("Back", new Vector2(50, 150));

            // Set the SelectedIndex of MusicVolumes and SoundVolumes to the current volume
            //MusicVolumes.SelectedIndex = (int)(SoundManager.MusicVolume * 10);
            //SoundVolumes.SelectedIndex = (int)(SoundManager.SoundVolume * 10);

            MusicVolumes.SelectionChanged += MusicVolumes_SelectionChanged;
            SoundVolumes.SelectionChanged += SoundVolumes_SelectionChanged;

            AllControls.Add(MusicVol);
            AllControls.Add(MusicVolumes);
            AllControls.Add(SoundVol);
            AllControls.Add(SoundVolumes);
            AllControls.Add(Back);

            AddMenuOption(MusicVol);
            AddMenuOption(SoundVol);
            AddMenuOption(Back);

            SetCursorPosition();
        }

        protected void MusicVolumes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = (ComboBox)sender;

            float thevol = ((float)box.SelectedIndex / 10);

            //SoundManager.SetMusicVolume(thevol);
        }

        protected void SoundVolumes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = (ComboBox)sender;

            float thevol = ((float)box.SelectedIndex / 10);

            //SoundManager.SetSoundVolume(thevol);
        }

        protected override void AddDropdownItems(ComboBox Dropdown)
        {
            for (int i = 0; i <= 10; i++)
            {
                Dropdown.Items.Add(i);
            }
        }

        protected override void PickOption()
        {
            // The user selected the "Back" option
            if (SelectedOption == 2)
            {
                // Bring the user back to the previous screen
                Game.RemoveScreen();
            }
        }


    }
}