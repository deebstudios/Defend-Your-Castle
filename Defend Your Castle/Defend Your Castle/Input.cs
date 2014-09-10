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

        public static bool IsMouseInRect(Rectangle Rect)
        {
            // Get the current MouseState
            MouseState mouseState = Mouse.GetState();

            // Create a rectangle for the mouse on the screen
            Rectangle MouseRect = new Rectangle(GetX(mouseState.X), GetY(mouseState.Y), 1, 1);

            // Return true if the mouse is within the Rectangle's bounds; otherwise, return false
            return MouseRect.Intersects(Rect);
        }

        public static bool IsScreenTapped(TouchPanelState touchState)
        {
            // Check if a gesture is NOT available, and return false if so
            if (touchState.IsGestureAvailable == false) return false;

            return (touchState.ReadGesture().GestureType == GestureType.Tap);
        }

        public static bool IsTapInRect(Rectangle Rect)
        {
            // Check if a gesture is NOT available, and return false if so
            if (TouchPanel.IsGestureAvailable == false) return false;

            // Get the gesture
            GestureSample gesture = TouchPanel.ReadGesture();

            // Check if the gesture was a tap
            if (gesture.GestureType == GestureType.Tap)
            {
                // Create a rectangle for the tap on the screen
                Rectangle TapRect = new Rectangle(GetX((int)gesture.Position.X), GetY((int)gesture.Position.Y), 1, 1);

                // Return true if the tap is within the Rectangle's bounds; otherwise, return false
                return Rect.Intersects(TapRect);
            }

            // A tap gesture was not found, so return false
            return false;
        }


    }
}