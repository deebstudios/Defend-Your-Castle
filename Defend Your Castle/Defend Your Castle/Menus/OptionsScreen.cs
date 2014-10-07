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
            Grid RootGrid = new Grid();

            RowDefinition row = new RowDefinition();
            RowDefinition row2 = new RowDefinition();

            row.Height = new GridLength(0.1, GridUnitType.Star);
            row2.Height = new GridLength(1, GridUnitType.Star);

            RootGrid.RowDefinitions.Add(row);
            RootGrid.RowDefinitions.Add(row2);

            // Create a background image for the screen
            Image BackgroundImage = CreateBackgroundImage("OptionsScreenBackground.png");
            BackgroundImage.SetValue(Grid.RowSpanProperty, 2);

            Viewbox TitleBox = new Viewbox();

            // Create the screen title
            TextBlock ScreenTitle = CreateLabel("Options");

            TitleBox.Child = ScreenTitle;

            TitleBox.HorizontalAlignment = HorizontalAlignment.Center;
            TitleBox.VerticalAlignment = VerticalAlignment.Top;
            TitleBox.Margin = new Thickness(0, 0, 0, 0);

            // Create the "Music Volume" label, and add spacing to it
            TextBlock MusicVol = CreateLabel("Music Volume: ");
            MusicVol.Margin = new Thickness(0, 0, 20, 0);

            // Create the Music volume ComboBox
            ComboBox MusicVolumes = CreateDropdown();

            // Create the "Sound Volume" label, and add spacing to it
            TextBlock SoundVol = CreateLabel("Sound Volume: ");
            SoundVol.Margin = new Thickness(0, 0, 20, 0);

            // Create the Sound volume ComboBox
            ComboBox SoundVolumes = CreateDropdown();

            // Create the Back button
            Button Back = CreateButton("Back", 250);

            Back.Margin = new Thickness(0, 30, 0, 0);

            // Set the SelectedIndex of MusicVolumes and SoundVolumes to the current volume
            MusicVolumes.SelectedIndex = (int)Math.Round((SoundManager.MusicVolume * 10));
            SoundVolumes.SelectedIndex = (int)Math.Round((SoundManager.SoundVolume * 10));

            // Add the SelectionChanged events to the ComboBoxes
            MusicVolumes.SelectionChanged += MusicVolumes_SelectionChanged;
            SoundVolumes.SelectionChanged += SoundVolumes_SelectionChanged;

            // Put the Music volume controls in a horizontal menu
            StackPanel MusicMenu = CreateHorizontalMenu(MusicVol, MusicVolumes);

            // Put the Sound volume controls in a horizontal menu, and add spacing to this menu
            StackPanel SoundMenu = CreateHorizontalMenu(SoundVol, SoundVolumes);
            SoundMenu.Margin = new Thickness(0, 20, 0, 20);

            // Put all of the horizontal menus and the Back button in a vertical menu
            StackPanel VerticalMenu = CreateVerticalMenu(MusicMenu, SoundMenu, Back);

            // Create a ViewBox
            Viewbox MenuViewBox = new Viewbox();

            // Add the vertical menu to the ViewBox so it scales proportionately on different screen resolutions
            MenuViewBox.Child = VerticalMenu;

            // Put the ViewBox in the second row of the Grid
            MenuViewBox.SetValue(Grid.RowProperty, 1);

            MenuViewBox.HorizontalAlignment = HorizontalAlignment.Center;
            MenuViewBox.VerticalAlignment = VerticalAlignment.Center;

            MenuViewBox.Margin = new Thickness(75);

            // This does not adjust automatically when new 
            MenuViewBox.MaxWidth = (Window.Current.Bounds.Width/ 4);
            MenuViewBox.MaxHeight = (Window.Current.Bounds.Height / 4);

            RootGrid.Children.Add(BackgroundImage);
            RootGrid.Children.Add(TitleBox);
            RootGrid.Children.Add(MenuViewBox);

            Controls.Add(RootGrid);

            // Add the background image to the screen
            //Controls.Add(BackgroundImage);

            // Add the Screen Title to the screen
            //Controls.Add(ScreenTitle);

            // Add the ViewBox to the controls on the screen
            //Controls.Add(MenuViewBox);

            // Add the volume ComboBoxes and the Back button as a menu options so they can be selected
            AddMenuOption(MusicVol);
            AddMenuOption(SoundVol);
            AddMenuOption(Back);
        }

        protected void MusicVolumes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the ComboBox that had its value changed
            ComboBox box = (ComboBox)sender;

            // Get the volume based on the ComboBox's SelectedIndex
            float thevol = ((float)box.SelectedIndex / 10);

            // Set the music volume
            SoundManager.SetMusicVolume(thevol);
        }

        protected void SoundVolumes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the ComboBox that had its value changed
            ComboBox box = (ComboBox)sender;

            // Get the volume based on the ComboBox's SelectedIndex
            float thevol = ((float)box.SelectedIndex / 10);

            // Set the sound volume
            SoundManager.SetSoundVolume(thevol);
        }

        protected override void AddDropdownItems(ComboBox Dropdown)
        {
            // Add volume choices of 0 to 10
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