using System;
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
        public MeleeEnemy(Level level, float xdiff, float Y, float speedadd, int costume)
        {
            ObjectSheet = LoadAssets.GoblinSheet[costume];
            InvincibleSheet = LoadAssets.GoblinInvincibleSheet;
            Animation = new Animation(true, new AnimFrame(new Rectangle(0, 0, 17, 35), 225), new AnimFrame(new Rectangle(21, 0, 17, 35), 225), new AnimFrame(new Rectangle(40, 0, 19, 35), 225, new Vector2(2, 0)));

            MoveSpeed = new Vector2(1f + speedadd, 0);

            Position = new Vector2(xdiff - Animation.CurrentAnimFrame.FrameSize.X, Y - Animation.CurrentAnimFrame.FrameSize.Y);

            WeaponWeakness = (int)Player.WeaponTypes.Sword;

            SetProperties(level);
        }

        protected override void ChooseNextAction(Level level)
        {
            if (CurAction.GetActionType == Action.ActionType.Moving)
            {
                //Attack now
                Animation AttackAnim = new Animation(new AnimFrame(new Rectangle(0, 36, 17, 35), 300), new AnimFrame(new Rectangle(20, 36, 17, 35), 400, new Vector2(1, 0)), new AnimFrame(new Rectangle(42, 37, 18, 34), 100, new Vector2(0, -1)));
                AttackAnim.SetAnimSpeed(Animation.GetSpeed);

                CurAction = new MeleeAttack(this, AttackAnim, 75, AttackAnim.MaxFrame);
            }
        }
    }
}
