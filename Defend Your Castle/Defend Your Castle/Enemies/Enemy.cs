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
        //"Fake" death for the enemy, indicating that the gold animation must play now
        protected bool FakeDead;

        //The fade for the gold drop
        protected FadeOnce GoldDrop;

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

            objectType = ObjectType.Enemy;

            FakeDead = false;
            GoldDrop = null;

            ObjectSheet = LoadAssets.testanim;

            Gold = 100;

            Position = new Vector2(0, 100);

            WeaponWeakness = (int)Player.WeaponTypes.Sword;
        }

        protected void SetProperties(Level level)
        {
            SetHurtbox((int)Animation.CurrentAnimFrame.FrameSize.X, (int)Animation.CurrentAnimFrame.FrameSize.Y, new Vector2(10));

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

        public override bool IsDying
        {
            get { return FakeDead; }
        }

        //Checks if the enemy can get hit
        public override bool CanGetHit(Rectangle rect)
        {
            return (base.CanGetHit(rect) == true && FakeDead == false);
        }

        public override void Die(Level level)
        {
            level.GetPlayer.ReceiveGold(Gold);
            FakeDead = true;
            GoldDrop = new FadeOnce(new Color(255, 255, 255, 255), -5, 0, 255, 0f);
        }

        protected abstract void ChooseNextAction(Level level);

        public override void Update(Level level)
        {
            if (FakeDead == false)
            {
                CurAction.Update(level);
                if (CurAction.IsComplete == true) ChooseNextAction(level);
            }
            //Update gold dropping animation
            else
            {
                GoldDrop.Update();
                if (GoldDrop.DoneFading == true) base.Die(level);
            }

            base.Update(level);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (FakeDead == false) CurAction.Draw(spriteBatch, ObjectSheet);
            //Draw gold dropping animation
            else
            {
                spriteBatch.DrawString(LoadAssets.bmpFont, "+" + Gold, new Vector2(Position.X - 20, Position.Y - 10), GoldDrop.GetFadeColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, .999f);
                spriteBatch.Draw(LoadAssets.GoldCoinEffect, Position, null, GoldDrop.GetFadeColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            }

            base.Draw(spriteBatch);
        }
    }
}