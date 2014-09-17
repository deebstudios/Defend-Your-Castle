using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_Your_Castle
{
    //A melee attack for an enemy; this action does not end until the enemy is killed or the level is over
    public sealed class MeleeAttack : Action
    {
        //The damage of the attack
        private int Damage;

        //The rate at which the damage is applied
        private float AttackRate;
        private float PrevAttackRate;

        //A possible delay for the attack
        //This can be used for looping animations where the attack starts on a designated frame, in which it would be shorter on the first attack
        /*Ex. 3 Frame animation, 300 ms each in duration. The attack starts on the 3rd frame, so after 600 ms. The next time the 3rd frame starts
          again would be 900 ms, so set the delay to 300 so all subsequent attacks start when the 3rd frame starts*/
        private float AttackDelay;

        public MeleeAttack(Enemy enem, Animation anim, int damage, float attackrate, float attackdelay) : base(enem, anim, ActionType.Attacking)
        {
            Damage = damage;

            AttackRate = attackrate;
            AttackDelay = attackdelay;
            RefreshAttack(false);
        }

        //Refreshes the attack timer
        private void RefreshAttack(bool delay)
        {
            PrevAttackRate = Game1.ActiveTime + AttackRate;
            if (delay == true) PrevAttackRate += AttackDelay;
        }

        public override void Update(Level level)
        {
            base.Update(level);

            //Check the attack rate
            if (Game1.ActiveTime >= PrevAttackRate)
            {
                level.GetPlayer.TakeDamage(Damage, level);
                RefreshAttack(true);
            }
        }
    }
}
