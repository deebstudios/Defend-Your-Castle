using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defend_Your_Castle
{
    //The Sword can hit all enemies that are not flying or armored
    public sealed class Sword : Weapon
    {
        public Sword()
        {
            // Set the attack speed to half a second
            AttackSpeed = 100;

            // Set the sound of the sword to a test sound
            Sound = LoadAssets.TestSound;

            //The Sword is available from the start, so make it available
            MakeAvailable();
        }

        public override bool CanHit(int weakness)
        {
            return (weakness == (int)Player.WeaponTypes.Sword);
        }
    }
}