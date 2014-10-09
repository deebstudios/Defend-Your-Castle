using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Defend_Your_Castle
{
    public class OptionsScreen : MenuScreen
    {
        public OptionsScreen(GamePage page, Game1 game) : base(page, game)
        {
            // Create a Grid to hold all of the content on the screen
            Grid RootGrid = new Grid();

            // Create two rows for the Grid
            RowDefinition row = new RowDefinition();
            RowDefinition row2 = new RowDefinition();

            // Set the Height property of each row
            row.Height = new GridLength(0.1, GridUnitType.Star);
            row2.Height = new GridLength(1, GridUnitType.Star);
            
            // Add the rows to the Grid
            RootGrid.RowDefinitions.Add(row);
            RootGrid.RowDefinitions.Add(row2);

            // Background Image

            // Create a background image for the screen and set it to span across both rows of the root Grid
            Image BackgroundImage = CreateBackgroundImage("OptionsScreenBackground.png");
            BackgroundImage.SetValue(Grid.RowSpanProperty, 2);

            // Screen Title

            // Create a new ViewBox for the screen title TextBlock
            Viewbox TitleBox = new Viewbox();

            // Set the Horizontal and Vertical alignment of the ViewBox
            TitleBox.HorizontalAlignment = HorizontalAlignment.Center;
            TitleBox.VerticalAlignment = VerticalAlignment.Top;

            // Create the screen title
            TextBlock ScreenTitle = CreateLabel("Options", true);

            // Add the title screen TextBlock to its ViewBox
            TitleBox.Child = ScreenTitle;

            // Volume Controls

            // Create the "Music Volume" TextBlock
            TextBlock MusicVol = CreateLabel("Music Volume: ", true);

            // Add right spacing to the TextBlock
            MusicVol.Margin = new Thickness(0, 0, 20, 0);

            // Vertically center the TextBlock
            MusicVol.VerticalAlignment = VerticalAlignment.Center;

            // Create the Music volume ComboBox
            ComboBox MusicVolumes = CreateDropdown();

            // Create the "Sound Volume" TextBlock
            TextBlock SoundVol = CreateLabel("Sound Volume: ", true);

            // Add right spacing to the TextBlock
            SoundVol.Margin = new Thickness(0, 0, 20, 0);

            // Vertically center the TextBlock
            SoundVol.VerticalAlignment = VerticalAlignment.Center;

            // Create the Sound volume ComboBox
            ComboBox SoundVolumes = CreateDropdown();

            // Back Button

            // Create the Back button
            Button Back = CreateButton("Back");

            // Add top spacing to the TextBlock
            Back.Margin = new Thickness(0, 30, 0, 0);

            // Stretch the Back Button horizontally
            Back.HorizontalAlignment = HorizontalAlignment.Stretch;

            // Volume Controls (Continued)

            // Set the SelectedIndex of MusicVolumes and SoundVolumes to the current volume
            MusicVolumes.SelectedIndex = (int)Math.Round((SoundManager.MusicVolume * 10));
            SoundVolumes.SelectedIndex = (int)Math.Round((SoundManager.SoundVolume * 10));

            // Add the SelectionChanged events to the ComboBoxes
            MusicVolumes.SelectionChanged += MusicVolumes_SelectionChanged;
            SoundVolumes.SelectionChanged += SoundVolumes_SelectionChanged;

            // Put the Music volume controls in a horizontal menu, and vertically center this menu
            StackPanel MusicMenu = CreateHorizontalMenu(MusicVol, MusicVolumes);
            MusicMenu.VerticalAlignment = VerticalAlignment.Center;

            // Put the Sound volume controls in a horizontal menu, and add spacing and vertically center this menu
            StackPanel SoundMenu = CreateHorizontalMenu(SoundVol, SoundVolumes);
            SoundMenu.Margin = new Thickness(20, 20, 20, 0);
            SoundMenu.VerticalAlignment = VerticalAlignment.Center;

            // Create a vertical menu to stack both horizontal menus
            StackPanel VolumeVerticalMenu = CreateVerticalMenu(MusicMenu, SoundMenu);

            // Border for Volume Controls

            // Create a Border to hold the vertical menu
            Border TheBorder = new Border();

            // Set the properties of the Border
            TheBorder.Background = (Application.Current.Resources["GameButtonBackgroundColor"] as SolidColorBrush);
            TheBorder.BorderBrush = new SolidColorBrush(Colors.Black);
            TheBorder.BorderThickness = new Thickness(2);
            TheBorder.Padding = new Thickness(0, 20, 0, 20);

            // Add the volume vertical menu to the Border
            TheBorder.Child = VolumeVerticalMenu;

            // Vertical Menu for the Border and Back Button

            // Put the Border and the Back button in a vertical menu
            StackPanel VerticalMenu = CreateVerticalMenu(TheBorder, Back);

            // ViewBox for the Vertical Menu

            // Create a ViewBox for the rest of the screen
            Viewbox MenuViewBox = new Viewbox();

            // Add the vertical menu to the ViewBox so it scales proportionately on different screen resolutions
            MenuViewBox.Child = VerticalMenu;

            // Horizontally and vertically center the ViewBox on screen
            MenuViewBox.HorizontalAlignment = HorizontalAlignment.Center;
            MenuViewBox.VerticalAlignment = VerticalAlignment.Center;

            // Grid for the Menu ViewBox

            // Create a new Grid to hold the MenuViewBox
            Grid ContentGrid = new Grid();

            // Create two new rows for the ContentGrid
            row = new RowDefinition();
            row2 = new RowDefinition();

            // Set the Height property of each row
            // The second row is there to make the MenuViewBox smaller without having to specify any specific size values
            // This adds automatic support for scaling to different screen sizes and resolutions
            row.Height = new GridLength(0.33, GridUnitType.Star);
            row2.Height = new GridLength(1, GridUnitType.Star);

            // Add the rows to the Grid
            ContentGrid.RowDefinitions.Add(row);
            ContentGrid.RowDefinitions.Add(row2);

            // Horizontally and vertically center the Grid on screen
            ContentGrid.HorizontalAlignment = HorizontalAlignment.Center;
            ContentGrid.VerticalAlignment = VerticalAlignment.Center;

            // Add the MenuViewBox to the Grid
            ContentGrid.Children.Add(MenuViewBox);

            // Move the ContentGrid to the second row of the Grid so it appears under the TitleBox
            ContentGrid.SetValue(Grid.RowProperty, 1);

            // Add the background image, screen title, and the rest of the screen content to the RootGrid
            RootGrid.Children.Add(BackgroundImage);
            RootGrid.Children.Add(TitleBox);
            RootGrid.Children.Add(ContentGrid);

            // Add the RootGrid to the screen
            Controls.Add(RootGrid);

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