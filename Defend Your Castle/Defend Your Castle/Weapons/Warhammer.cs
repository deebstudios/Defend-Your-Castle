using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defend_Your_Castle
{
    //The Warhammer can hit armored enemies
    public sealed class Warhammer : Weapon
    {
        public Warhammer()
        {
            //Set the attack speed
            AttackSpeed = 50;

            //Set the sound of the warhammer
            Sound = LoadAssets.TestSound;

            // TEMPORARY
            MakeAvailable();
        }

        public override bool CanHit(int weakness)
        {
            return (weakness != (int)Player.WeaponTypes.Spear);
        }
    }
}