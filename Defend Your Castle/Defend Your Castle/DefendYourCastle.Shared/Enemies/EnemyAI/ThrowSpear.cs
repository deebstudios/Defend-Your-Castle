using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Defend_Your_Castle
{
    //The spear enemy throws spears at the player castle
    public sealed class ThrowSpear : Action
    {
        //The amount of damage the projectile does
        private int Damage;

        //The frame the enemy throws the spear
        private int FrameThrow;

        public ThrowSpear(Enemy enem, Animation anim, int framethrow, int damage) : base(enem, anim, ActionType.Attacking)
        {
            Damage = damage;

            FrameThrow = framethrow;
        }

        private Projectile SpearThrown()
        {
            return (new EnemySpear(Damage, new Vector2(2, -4)));
        }

        public override void Update(Level level)
        {
            int curframe = Anim.CurrentFrame;

            base.Update(level);

            if (Anim.CurrentFrame != curframe && Anim.CurrentFrame == FrameThrow)
            {
                //Vector2 launchpos = new Vector2(enemy.GetPosition.X, enemy.GetPosition.Y + Anim.CurrentAnimFrame.FrameSize.Y);

                Projectile spear = SpearThrown();
                spear.Launch(enemy.GetPosition, Anim.CurrentAnimFrame.FrameSize, level.GetPlayer.GetPosition);
                level.AddEnemy(spear);
            }
        }
    }
}
