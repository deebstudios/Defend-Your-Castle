using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Defend_Your_Castle
{
    //An enemy that goes up to the tower and throws spears at it
    public class SpearEnemy : Enemy
    {
        //The amount of damage the projectile deals
        protected int ProjectileDamage;

        public SpearEnemy(Level level, float xdiff, float Y, float speedadd, int costume)
        {
            MoveSpeed = new Vector2(.5f + speedadd, 0);
            Range = 150;

            ProjectileDamage = 25;

            ObjectSheet = LoadAssets.SpearGoblinSheet[costume];
            InvincibleSheet = LoadAssets.SpearGoblinInvincibleSheet;
            Animation = new Animation(true, new AnimFrame(new Rectangle(0, 0, 17, 35), 225), new AnimFrame(new Rectangle(21, 0, 17, 35), 225), new AnimFrame(new Rectangle(40, 0, 20, 35), 225, new Vector2(2, 0)));

            WeaponWeakness = (int)Player.WeaponTypes.Sword;

            Position = new Vector2(xdiff - Animation.CurrentAnimFrame.FrameSize.X, Y - Animation.CurrentAnimFrame.FrameSize.Y);

            SetProperties(level);
        }

        protected sealed override void ChooseNextAction(Level level)
        {
            if (CurAction.GetActionType == Action.ActionType.Moving)
            {
                //Throw spears
                Animation ShootAnim = new Animation(new AnimFrame(new Rectangle(0, 36, 17, 35), 225), new AnimFrame(new Rectangle(21, 36, 17, 35), 150), new AnimFrame(new Rectangle(42, 36, 17, 35), 400));
                ShootAnim.SetAnimSpeed(Animation.GetSpeed);

                CurAction = new ThrowSpear(this, ShootAnim, ShootAnim.MaxFrame, ProjectileDamage);
            }
        }
    }
}
