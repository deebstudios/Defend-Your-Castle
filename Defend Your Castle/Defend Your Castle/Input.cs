using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Defend_Your_Castle
{
    // Class that handles key input
    public static class Input
    {
        public static bool IsKeyDown(KeyboardState KeyboardState, Keys Key)
        {
            // Return true if the specified key is pressed and held; otherwise, return false
            return (Keyboard.GetState().IsKeyDown(Key) && KeyboardState.IsKeyUp(Key));
        }

        public static bool IsKeyHeld(Keys Key)
        {
            return (Keyboard.GetState().IsKeyDown(Key));
        }

        public static bool IsLeftMouseButtonDown(MouseState MouseState)
        {
            return (MouseState.LeftButton == ButtonState.Pressed && Mouse.GetState().LeftButton == ButtonState.Released);
        }

        public static bool IsRightMouseButtonDown(MouseState MouseState)
        {
            return (MouseState.RightButton == ButtonState.Pressed && Mouse.GetState().RightButton == ButtonState.Released);
        }

        public static int GetX(int X)
        {
            return ((int)((float)X * Game1.ResolutionScaleFactor.X));
        }

        public static int GetY(int Y)
        {
            return ((int)((float)Y * Game1.ResolutionScaleFactor.Y));
        }

        public static bool IsMouseInRect(Rectangle Rect, MouseState MouseState)
        {
            // Create a rectangle for the mouse on the screen
            // The GetX and GetY methods are needed here since the mouse's coordinates are read by XAML
            Rectangle MouseRect = new Rectangle(GetX(MouseState.X), GetY(MouseState.Y), 1, 1);

            // Return true if the mouse is within the Rectangle's bounds; otherwise, return false
            return MouseRect.Intersects(Rect);
        }

        public static Rectangle MouseRect(MouseState MouseState)
        {
            // Create a rectangle for the mouse on the screen
            // The GetX and GetY methods are needed here since the mouse's coordinates are read by XAML
            Rectangle MouseRect = new Rectangle(GetX(MouseState.X), GetY(MouseState.Y), 1, 1);

            return MouseRect;
        }

        public static TouchLocation? GetTouchLocation()
        {
            // Get the collection of TouchLocation objects
            TouchCollection touchCollection = TouchPanel.GetState();

            // Return null if no touches can be found
            if (touchCollection.Count < 1) return null;

            // Return null if the touch is not pressed
            if (touchCollection[0].State != TouchLocationState.Pressed) return null;

            // Return the first TouchLocation object
            return (touchCollection[0]);
        }

        //public static GestureSample? GetTouchGesture()
        //{
        //    // Check if a gesture is NOT available, and return null if so
        //    if (TouchPanel.IsGestureAvailable == false) return null;

        //    // Return the last gesture
        //    return (TouchPanel.ReadGesture());
        //}

        public static bool IsScreenTapped(TouchLocation? touchLoc)
        {
            return (touchLoc != null);
        }

        //public static bool IsScreenTapped(GestureSample? gesture)
        //{
        //    // Check if the specified gesture is invalid, and return false if so
        //    if (gesture == null) return false;

        //    // Convert the specified gesture to a non-nullable type GestureSample to access its properties
        //    GestureSample fullGesture = (GestureSample)gesture;

        //    // Return whether or not the gesture was a tap
        //    return (fullGesture.GestureType == GestureType.Tap);
        //}

        //public static bool IsTapInRect(Rectangle Rect, GestureSample? gesture)
        //{
        //    // Convert the specified gesture to a non-nullable type GestureSample to access its properties
        //    GestureSample fullGesture = (GestureSample)gesture;

        //    // Create a rectangle for the tap on the screen
        //    Rectangle TapRect = new Rectangle((int)fullGesture.Position.X, (int)fullGesture.Position.Y, 1, 1);

        //    // Return true if the tap is within the Rectangle's bounds; otherwise, return false
        //    return TapRect.Intersects(Rect);
        //}

        //public static Rectangle GestureRect(GestureSample? gesture)
        //{
        //    // Convert the specified gesture to a non-nullable type GestureSample to access its properties
        //    GestureSample fullGesture = (GestureSample)gesture;

        //    // Create a rectangle for the tap on the screen
        //    // The GetX and GetY methods are needed here since the touch coordinates are based on the screen resolution
        //    Rectangle TapRect = new Rectangle(GetX((int)fullGesture.Position.X), GetY((int)fullGesture.Position.Y), 1, 1);

        //    // Return the rectangle that represents the tapped region
        //    return TapRect;
        //}

        public static Rectangle GetTouchRect(TouchLocation? touchLoc)
        {
            // Convert the specified location to a non-nullable TouchLocation to access its properties
            TouchLocation fullTouchLoc = (TouchLocation)touchLoc;

            // Create a rectangle for the touch location on the screen
            // The GetX and GetY methods are needed here since the touch coordinates are based on the screen resolution
            Rectangle TapRect = new Rectangle(GetX((int)fullTouchLoc.Position.X), GetY((int)fullTouchLoc.Position.Y), 1, 1);

            // Return the rectangle that represents the tapped region
            return TapRect;
        }


    }
}