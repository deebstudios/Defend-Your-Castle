using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defend_Your_Castle
{
    public sealed class Warhammer : Weapon
    {
        public Warhammer()
        {
            //Set the attack speed
            AttackSpeed = 750;

            //Set the sound of the warhammer
            Sound = LoadAssets.TestSound;

            // TEMPORARY
            MakeAvailable();
        }
    }
}