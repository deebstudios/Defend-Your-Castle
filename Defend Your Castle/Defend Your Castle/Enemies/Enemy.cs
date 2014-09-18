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

        // The amount of gold the enemy gives when killed
        protected int Gold;

        //Current action the enemy is performing
        protected Action CurAction;

        public Enemy()
        {

        }

        public Enemy(Animation animation, Level level)
        {
            // Set the enemy's properties
            MoveSpeed = new Vector2(2, 0);
            Range = 1;

            ObjectSheet = LoadAssets.testanim;

            // Set the animation of the enemy
            Animation = animation;

            Gold = 100;

            Position = new Vector2(0, 100);

            SetHurtbox((int)Animation.CurrentAnimFrame.FrameSize.X, (int)Animation.CurrentAnimFrame.FrameSize.Y);

            //By default, enemies start out moving right
            CurAction = new MoveForward(this, Animation, ((int)level.GetPlayer.GetPosition.X - hurtbox.Width - Range));
        }

        //Gets the movespeed of the enemy
        public Vector2 GetMoveSpeed
        {
            get { return MoveSpeed; }
        }

        //Gets the amount of gold the enemy grants upon being killed
        public int GetGold
        {
            get { return Gold; }
        }

        public override void Die(Level level)
        {
            level.GetPlayer.ReceiveGold(Gold);
            base.Die(level);
        }

        protected virtual void ChooseNextAction(Level level)
        {
            if (CurAction.GetActionType == Action.ActionType.Moving)
            {
                //Attack now
                Animation AttackAnim = new Animation(new AnimFrame(new Rectangle(6, 16, 6, 16), 300, new Vector2(2, 0)), new AnimFrame(new Rectangle(23, 16, 7, 16), 300, new Vector2(1, 0)), new AnimFrame(new Rectangle(40, 16, 8, 16), 300));

                CurAction = new MeleeAttack(this, AttackAnim, 50, 600, 300);
            }
        }

        public override void Update(Level level)
        {
            if (CurAction.IsComplete == false)
            {
                CurAction.Update(level);
                if (CurAction.IsComplete == true) ChooseNextAction(level);
            }
            // Move the enemy
            //Move(MoveSpeed);

            // Update the animation
            //Animation.Update();


            base.Update(level);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            CurAction.Draw(spriteBatch, ObjectSheet);//Animation.Draw(spriteBatch, LoadAssets.testanim, Position, DirectionFacing, Color.White, 0f, 1f);

            base.Draw(spriteBatch);
        }
    }
}