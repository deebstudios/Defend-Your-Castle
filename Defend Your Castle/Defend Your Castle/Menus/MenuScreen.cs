using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace Defend_Your_Castle
{
    public class MenuScreen
    {
        // All of the controls on the menu
        public List<UIElement> AllControls;

        // All of the controls on the menu that are menu options
        public List<UIElement> MenuOptions;

        // The cursor
        public Image Cursor;

        // The amount to offset the cursor from the menu options
        public Vector2 CursorOffset;

        // The option the player currently has selected
        public int SelectedOption;

        // The position of the cursor on the menu
        public Vector2 CursorPos;

        // References to GamePage.xaml and Game1.cs
        public readonly GamePage GamePage;
        public readonly Game1 Game;

        public MenuScreen()
        {
            // Initialize the AllControls list
            AllControls = new List<UIElement>();

            // Initialize the MenuOptions list
            MenuOptions = new List<UIElement>();

            // Create a cursor for the menu
            CreateCursor();

            // Automatically add the Cursor to the AllControls list
            AllControls.Add(Cursor);
        }

        public MenuScreen(GamePage page, Game1 game) : this()
        {
            // Store a reference to both GamePage.xaml and Game1.cs, respectively
            GamePage = page;
            Game = game;
        }

        private void CreateCursor()
        {
            // Create a new Image control to represent the cursor
            Cursor = new Image();

            // Get the image for the cursor
            Cursor.Source = new BitmapImage(new Uri("ms-appx:/Content/Graphics/cursor.png"));

            // Set the Width and Height of the Cursor Image
            Cursor.Width = 32;
            Cursor.Height = 32;
        }

        public void CursorMove(VirtualKey key)
        {
            // Get the index value of the last item in the MenuOptions list
            int LastMenuOption = (MenuOptions.Count - 1);

            // Check if the player moved the selection cursor up
            if (key == VirtualKey.Up && SelectedOption > 0)
            {
                SelectedOption -= 1;
                SetCursorPosition();
            }
            // Check if the player moved the selection cursor down
            if (key == VirtualKey.Down && SelectedOption < LastMenuOption)
            {
                SelectedOption += 1;
                SetCursorPosition();
            }
            // Check if the player pressed the "Enter" key
            if (key == VirtualKey.Enter)
            {
                PickOption();
            }
        }

        private void SetElementPosition(UIElement element, Vector2 Position)
        {
            // Set the Left and Top properties of the element in the Canvas
            element.SetValue(Canvas.LeftProperty, Position.X);
            element.SetValue(Canvas.TopProperty, Position.Y);
        }

        public void SetCursorPosition()
        {
            // Get the X and Y position at which the Cursor should be displayed in the canvas
            float x = (float)((double)MenuOptions[SelectedOption].GetValue(Canvas.LeftProperty));
            float y = (float)((double)MenuOptions[SelectedOption].GetValue(Canvas.TopProperty));
            
            // Get the new cursor position. Subtract the CursorOffset by the generated position
            CursorPos = (new Vector2(x, y) - CursorOffset);
            
            // Set the position of the Cursor
            SetElementPosition(Cursor, CursorPos);
        }

        protected Style GetStyle(String StyleName)
        {
            // Return the XAML Style with the specified name. null will be returned if the Style is not found
            return (Application.Current.Resources[StyleName] as Style);
        }

        protected TextBlock CreateLabel(String Text, Vector2 Position)
        {
            // Create a new TextBlock
            TextBlock txtBlock = new TextBlock();

            // Set the TextBlock's Style
            txtBlock.Style = GetStyle("TextBlockStyle");

            // Set the TextBlock's Text
            txtBlock.Text = Text;

            // Set the TextBlock's position
            SetElementPosition(txtBlock, Position);

            // Return the TextBlock
            return txtBlock;
        }

        protected ComboBox CreateDropdown(Vector2 Position)
        {
            // Create a new ComboBox
            ComboBox cmbBox = new ComboBox();

            // Set the ComboBox's Style
            cmbBox.Style = GetStyle("ComboBoxStyle");

            // Add items to the ComboBox
            AddDropdownItems(cmbBox);

            // Set the ComboBox's SelectedIndex to 0 by default
            if (cmbBox.Items.Count > 0) cmbBox.SelectedIndex = 0;

            // Set the ComboBox's position
            SetElementPosition(cmbBox, Position);

            // Return the ComboBox
            return cmbBox;
        }

        protected virtual void AddDropdownItems(ComboBox Dropdown)
        {
            // Nothing in the base class
        }

        protected void AddMenuOption(UIElement element)
        {
            // Add the Tapped and PointerEntered events to the element
            element.Tapped += MenuOption_Tapped;
            element.PointerEntered += MenuOption_MouseOver;

            // Add the menu option
            MenuOptions.Add(element);
        }

        protected void MenuOption_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Set the SelectedOption to the tapped MenuOption
            SelectedOption = MenuOptions.IndexOf((UIElement)sender);
            SetCursorPosition();

            // Try to pick the option
            PickOption();
        }

        protected void MenuOption_MouseOver(object sender, PointerRoutedEventArgs e)
        {
            // Set the SelectedOption to the tapped MenuOption
            SelectedOption = MenuOptions.IndexOf((UIElement)sender);
            SetCursorPosition();
        }

        protected virtual void PickOption()
        {
            // Nothing here for the base class
        }

        public void ShowScreen()
        {
            // Clear all of the children from the Canvas
            GamePage.CurrentScreen.Children.Clear();
            
            // Loop through all of the controls on the menu
            for (int i = 0; i < AllControls.Count; i++)
            {
                // Add each control to the Canvas
                GamePage.CurrentScreen.Children.Add(AllControls[i]);
            }
        }


    }
}