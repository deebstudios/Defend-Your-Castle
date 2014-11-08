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
        // The minimum amount required to move your finger after holding it down to be considered a swipe input
        private const float DragTolerance = 30f;

        // Stores the previous touch location. Used for swiping
        private static Vector2 PrevTouchLoc;

        static Input()
        {
            ResetPrevTouchLoc();
        }

        private static void ResetPrevTouchLoc()
        {
            PrevTouchLoc = -Vector2.One;
        }

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

            // Check if no touches can be found
            if (touchCollection.Count < 1)
            {
                // Reset the previous touch location
                ResetPrevTouchLoc();

                // Return null
                return null;
            }

            // Check if a previous touch location doesn't exist
            if (PrevTouchLoc == -Vector2.One)
            {
                // Set the previous touch location to the first touch location
                PrevTouchLoc = touchCollection[0].Position;
            }

            // Return the first TouchLocation object
            return (touchCollection[0]);
        }

        public static bool IsScreenSwiped(float delta)
        {
            // Translate the raw delta value to in-game coordinates. Also, get the positive value of the delta
            delta = Math.Abs(GetX((int)delta));

            // Return whether or not a swipe was performed
            return (delta >= DragTolerance);
        }

        public static float GetSwipeDelta(TouchLocation? touchLoc)
        {
            // Return false if the touch location is null
            if (touchLoc == null) return 0f;

            // Convert the specified location to a non-nullable TouchLocation to access its properties
            TouchLocation fullTouchLoc = (TouchLocation)touchLoc;
            
            // Return false if the touch is not in the released state
            if (fullTouchLoc.State != TouchLocationState.Released) return 0f;

            // Get the difference between the two positions
            Vector2 delta = (fullTouchLoc.Position - PrevTouchLoc);
            
            // Reset the previous touch location
            ResetPrevTouchLoc();

            // Return the X difference between the current and previous touch points
            return delta.X;
        }

        public static bool IsScreenTapped(TouchLocation? touchLoc)
        {
            // Return false if the touch location is null
            if (touchLoc == null) return false;

            // Convert the specified location to a non-nullable TouchLocation to access its properties
            TouchLocation fullTouchLoc = (TouchLocation)touchLoc;

            // Return true if the touch is pressed
            return (fullTouchLoc.State == TouchLocationState.Pressed);
        }

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