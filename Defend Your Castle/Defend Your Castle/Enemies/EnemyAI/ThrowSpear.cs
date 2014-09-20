﻿using System;
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
        //The frame the enemy throws the spear
        private int FrameThrow;

        public ThrowSpear(Enemy enem, Animation anim, int framethrow) : base(enem, anim, ActionType.Attacking)
        {
            FrameThrow = framethrow;
        }

        private Projectile SpearThrown()
        {
            return (new Spear(new Vector2(2, -4)));
        }

        public override void Update(Level level)
        {
            int curframe = Anim.CurrentFrame;

            base.Update(level);

            if (Anim.CurrentFrame != curframe && Anim.CurrentFrame == FrameThrow)
            {
                Projectile spear = SpearThrown();
                spear.Launch(enemy.GetPosition);
                level.AddEnemy(spear);
            }
        }
    }
}
