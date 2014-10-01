﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Defend_Your_Castle
{
    //An enemy that goes up to the castle and attacks
    public sealed class MeleeEnemy : Enemy
    {
        public MeleeEnemy(Level level)
        {
            ObjectSheet = LoadAssets.GoblinSheet;
            Animation = new Animation(true, new AnimFrame(new Rectangle(0, 0, 17, 35), 225), new AnimFrame(new Rectangle(21, 0, 17, 35), 225), new AnimFrame(new Rectangle(40, 0, 19, 35), 225, new Vector2(2, 0)));

            SetProperties(level);
        }

        protected override void ChooseNextAction(Level level)
        {
            if (CurAction.GetActionType == Action.ActionType.Moving)
            {
                //Attack now
                Animation AttackAnim = new Animation(new AnimFrame(new Rectangle(6, 16, 6, 16), 300, new Vector2(2, 0)), new AnimFrame(new Rectangle(23, 16, 7, 16), 300, new Vector2(1, 0)), new AnimFrame(new Rectangle(40, 16, 8, 16), 300));

                CurAction = new MeleeAttack(this, /*AttackAnim*/Animation, 50, 600, 300);
            }
        }
    }
}
