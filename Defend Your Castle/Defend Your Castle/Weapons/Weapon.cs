using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Defend_Your_Castle
{
    public abstract class Weapon
    {
        // The attack speed of the weapon (in milliseconds)
        public float AttackSpeed;
        private float PrevAttack;

        //Is the weapon available for use yet?
        private bool Available;

        // The sound to play when the player uses the weapon
        public SoundEffect Sound;

        public Weapon()
        {
            PrevAttack = 0f;

            //Weapons start out not available by default
            Available = false;
        }

        public bool CanUse
        {
            get { return Available; }
        }

        public bool CanAttack
        {
            get { return Game1.ActiveTime >= PrevAttack; }
        }

        //Makes the Weapon available for use
        public void MakeAvailable()
        {
            Available = true;
        }

        public void Attack()
        {
            SoundManager.PlaySound(Sound);
            PrevAttack = Game1.ActiveTime + AttackSpeed;
        }
    }
}