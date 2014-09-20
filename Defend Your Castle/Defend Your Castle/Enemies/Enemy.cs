using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_Your_Castle
{
    public abstract class Enemy : LevelObject
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
            // Set the enemy's properties
            MoveSpeed = new Vector2(2, 0);
            Range = 1;

            ObjectSheet = LoadAssets.testanim;

            Gold = 100;

            Position = new Vector2(0, 100);

            WeaponWeakness = (int)Player.WeaponTypes.Sword;
        }

        protected void SetProperties(Level level)
        {
            SetHurtbox((int)Animation.CurrentAnimFrame.FrameSize.X + 10, (int)Animation.CurrentAnimFrame.FrameSize.Y + 10);

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

        protected abstract void ChooseNextAction(Level level);

        public override void Update(Level level)
        {
            if (CurAction.IsComplete == false)
            {
                CurAction.Update(level);
                if (CurAction.IsComplete == true) ChooseNextAction(level);
            }

            base.Update(level);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            CurAction.Draw(spriteBatch, ObjectSheet);//Animation.Draw(spriteBatch, LoadAssets.testanim, Position, DirectionFacing, Color.White, 0f, 1f);

            base.Draw(spriteBatch);
        }
    }
}