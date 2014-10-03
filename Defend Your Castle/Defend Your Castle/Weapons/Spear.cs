using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defend_Your_Castle
{
    //The Spear can hit flying enemies
    public sealed class Spear : Weapon
    {
        public Spear()
        {
            // Set the attack speed
            AttackSpeed = 200;

            //Set the sound of the spear to a test sound
            Sound = LoadAssets.TestSound;

            //TEMPORARY
            MakeAvailable();
        }

        public override bool CanHit(int weakness)
        {
            return (weakness != (int)Player.WeaponTypes.Warhammer);
        }
    }
}
