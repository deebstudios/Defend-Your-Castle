using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Defend_Your_Castle
{
    //A class for helping with debugging
    public static class Debug
    {
        //Variables for showing certain things, like hitboxes and hurtboxes
        public static bool HurtboxShow;
        public static bool HitboxShow;

        public static KeyboardState DebugKeyboard;

        static Debug()
        {
            HurtboxShow = HitboxShow = false;

            DebugKeyboard = new KeyboardState();
        }

        //Displays the value of an object into the Output window
        public static void OutputValue(object value)
        {
            System.Diagnostics.Debug.WriteLine(value);
        }

        public static void Update()
        {
            //Key combinations
            if (Input.IsKeyHeld(Keys.LeftShift) == true)
            {
                //Left for hurtboxes, right for hitboxes
                if (Input.IsKeyDown(DebugKeyboard, Keys.Left) == true)
                    HurtboxShow = !HurtboxShow;
                else if (Input.IsKeyDown(DebugKeyboard, Keys.Right) == true)
                    HitboxShow = !HitboxShow;
            }

            DebugKeyboard = Keyboard.GetState();
        }
    }
}
