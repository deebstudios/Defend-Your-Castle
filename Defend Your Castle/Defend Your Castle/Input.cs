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
        // Stores the global touch state for the game window
        public static TouchPanelState TouchState;

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

        public static GestureSample? GetTouchGesture()
        {
            // Check if a gesture is NOT available, and return null if so
            if (TouchState.IsGestureAvailable == false) return null;
            
            // Return the last gesture
            return (TouchState.ReadGesture());
        }

        public static bool IsScreenTapped(GestureSample? gesture)
        {
            // Check if the specified gesture is invalid, and return false if so
            if (gesture == null) return false;

            // Convert the specified gesture to a non-nullable type GestureSample to access its properties
            GestureSample fullGesture = (GestureSample)gesture;

            // Return whether or not the gesture was a tap
            return (fullGesture.GestureType == GestureType.Tap);
        }

        public static bool IsTapInRect(Rectangle Rect, GestureSample? gesture)
        {
            // Convert the specified gesture to a non-nullable type GestureSample to access its properties
            GestureSample fullGesture = (GestureSample)gesture;

            // Create a rectangle for the tap on the screen
            Rectangle TapRect = new Rectangle((int)fullGesture.Position.X, (int)fullGesture.Position.Y, 1, 1);

            // Return true if the tap is within the Rectangle's bounds; otherwise, return false
            return TapRect.Intersects(Rect);
        }

        public static Rectangle GestureRect(GestureSample? gesture)
        {
            // Convert the specified gesture to a non-nullable type GestureSample to access its properties
            GestureSample fullGesture = (GestureSample)gesture;

            // Create a rectangle for the tap on the screen
            Rectangle TapRect = new Rectangle((int)fullGesture.Position.X, (int)fullGesture.Position.Y, 1, 1);

            return TapRect;
        }
    }
}