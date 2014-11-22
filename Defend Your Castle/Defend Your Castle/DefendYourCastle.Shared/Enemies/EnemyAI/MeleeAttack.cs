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

        //The frame the enemy attacks
        private int FrameAttack;

        public MeleeAttack(Enemy enem, Animation anim, int damage, int frameattack) : base(enem, anim, ActionType.Attacking)
        {
            Damage = damage;

            FrameAttack = frameattack;
        }

        public override void Update(Level level)
        {
            int curframe = Anim.CurrentFrame;

            base.Update(level);

            //Check if the enemy should attack
            if (Anim.CurrentFrame != curframe && Anim.CurrentFrame == FrameAttack)
            {
                level.GetPlayer.TakeDamage(Damage, level);
            }
        }
    }
}
