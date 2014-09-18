using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Defend_Your_Castle
{
    //An enemy that goes up to the tower and throws spears at it
    public sealed class SpearEnemy : Enemy
    {
        public SpearEnemy(Level level)
        {
            MoveSpeed = new Vector2(1, 0);
            Range = 150;

            ObjectSheet = LoadAssets.testanim;

            Animation = new Animation(new AnimFrame(new Rectangle(5, 0, 9, 16), 300, new Vector2(1, 0)), new AnimFrame(new Rectangle(23, 0, 8, 16), 300), new AnimFrame(new Rectangle(40, 0, 8, 16), 300));

            Position = new Vector2(0, 140);

            SetHurtbox((int)Animation.CurrentAnimFrame.FrameSize.X, (int)Animation.CurrentAnimFrame.FrameSize.Y);

            //By default, enemies start out moving right
            CurAction = new MoveForward(this, Animation, ((int)level.GetPlayer.GetPosition.X - hurtbox.Width - Range));
        }

        protected override void ChooseNextAction(Level level)
        {
            if (CurAction.GetActionType == Action.ActionType.Moving)
            {
                //Throw spears
                Animation ShootAnim = new Animation(new AnimFrame(new Rectangle(6, 16, 6, 16), 400, new Vector2(2, 0)), new AnimFrame(new Rectangle(23, 16, 7, 16), 400, new Vector2(1, 0)), new AnimFrame(new Rectangle(40, 16, 8, 16), 400));

                CurAction = new ThrowSpear(this, ShootAnim, ShootAnim.MaxFrame);
            }
        }
    }
}
