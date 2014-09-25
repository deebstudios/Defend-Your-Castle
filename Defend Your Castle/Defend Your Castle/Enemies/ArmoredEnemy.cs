using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Defend_Your_Castle
{
    //An armored enemy
    public sealed class ArmoredEnemy : Enemy
    {
        public ArmoredEnemy(Level level)
        {
            MoveSpeed = new Vector2(1, 0);

            WeaponWeakness = (int)Player.WeaponTypes.Warhammer;

            Gold = 200;

            Position = new Vector2(0, 120);

            Animation = new Animation(new AnimFrame(new Rectangle(5, 0, 9, 16), 300, new Vector2(1, 0)), new AnimFrame(new Rectangle(23, 0, 8, 16), 300), new AnimFrame(new Rectangle(40, 0, 8, 16), 300));

            SetProperties(level);
        }

        protected override void ChooseNextAction(Level level)
        {
            if (CurAction.GetActionType == Action.ActionType.Moving)
            {
                //Attack now
                Animation AttackAnim = new Animation(new AnimFrame(new Rectangle(6, 16, 6, 16), 400, new Vector2(2, 0)), new AnimFrame(new Rectangle(23, 16, 7, 16), 400, new Vector2(1, 0)), new AnimFrame(new Rectangle(40, 16, 8, 16), 400));

                CurAction = new MeleeAttack(this, AttackAnim, 100, 800, 400);
            }
        }
    }
}
