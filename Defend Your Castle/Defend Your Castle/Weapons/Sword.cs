using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defend_Your_Castle
{
    public class Sword : Weapon
    {
        public Sword()
        {
            // Set the attack speed to half a second
            AttackSpeed = 500;

            // Set the sound of the sword to a test sound
            Sound = LoadAssets.TestSound;
        }


    }
}