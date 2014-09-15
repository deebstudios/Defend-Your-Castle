using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_Your_Castle
{
    public class Enemy : LevelObject
    {
        // Movement speed
        protected Vector2 MoveSpeed;

        // Range of the Enemy. Use 1 for melee enemies
        protected int Range;

        // The point on the level at which the enemy will stop moving
        //protected Vector2 StopPoint;

        //Current action the enemy is performing
        protected Action CurAction;

        public Enemy(Animation animation, Level level)
        {
            // Set the enemy's properties
            MoveSpeed = new Vector2(2, 0);
            Range = 1;
            //StopPoint = (CastleX - Range);

            // Set the animation of the enemy
            Animation = animation;

            Position = new Vector2(0, 100);
            
            //By default, enemies start out moving right
            CurAction = new MoveForward(this, Animation, (level.GetPlayer.GetStartX - (int)Animation.CurrentAnimFrame.FrameSize.X - Range));

            SetHurtbox((int)Animation.CurrentAnimFrame.FrameSize.X, (int)Animation.CurrentAnimFrame.FrameSize.Y);
        }

        //Gets the movespeed of the enemy
        public Vector2 GetMoveSpeed
        {
            get { return MoveSpeed; }
        }

        protected virtual void ChooseNextAction()
        {
            if (CurAction.GetActionType == Action.ActionType.Moving)
            {
                //Attack now

            }
        }

        public override void Update(Level level)
        {
            if (CurAction.IsComplete == false)
            {
                CurAction.Update();
                if (CurAction.IsComplete == true) ChooseNextAction();
            }
            // Move the enemy
            //Move(MoveSpeed);

            // Update the animation
            //Animation.Update();


            base.Update(level);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            CurAction.Draw(spriteBatch, LoadAssets.testanim);//Animation.Draw(spriteBatch, LoadAssets.testanim, Position, DirectionFacing, Color.White, 0f, 1f);

            base.Draw(spriteBatch);
        }
    }
}