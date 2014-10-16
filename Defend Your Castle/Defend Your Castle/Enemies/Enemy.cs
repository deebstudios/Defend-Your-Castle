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

        // Minimum movement speed
        protected Vector2 MinimumMoveSpeed;

        // Tracks whether or not the enemy is slowed
        protected bool IsSlowed;

        // The amount to slow the enemy
        protected Vector2 SlowAmount;

        // The time until the enemy should be slowed
        protected float SlowTime;

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

            // Set the slow amount
            SlowAmount = new Vector2(0, 0);

            // Set the minimum movement speed
            MinimumMoveSpeed = new Vector2(0.1f, 0);

            //FOR TESTING INVINCIBILITY
            //InvincibilityLength = 5000f;
            //InvincibilityFade = new Fade(Color.White, 10, 0, 255, Fade.InfiniteLoops, 0f);
            //UseInvincibility();

            Gold = 100;

            Position = new Vector2(0, 100);

            WeaponWeakness = (int)Player.WeaponTypes.Sword;
        }

        protected void SetProperties(Level level)
        {
            SetHurtbox((int)Animation.CurrentAnimFrame.FrameSize.X, (int)Animation.CurrentAnimFrame.FrameSize.Y, new Vector2(10));

            //By default, enemies start out moving right
            CurAction = new MoveForward(this, Animation, StopAtCastle(level.GetPlayer.GetPosition, Animation.CurrentAnimFrame.FrameSize, Range));//((int)level.GetPlayer.GetPosition.X - hurtbox.Width - Range));
        }

        //The amount to offset the Y position when drawing used ONLY for flying enemies right now)
        //public virtual float YDrawOffset
        //{
        //    get { return 0f; } 
        //}

        //Gets the movespeed of the enemy
        public Vector2 GetMoveSpeed
        {
            get
            {
                // Get the true movement speed, taking into account any slows applied to the enemy
                Vector2 TrueMoveSpeed = (MoveSpeed - SlowAmount);

                // Return the true movement speed. If it is less than the minimum movement speed, return the minimum
                return ((TrueMoveSpeed.X >= MinimumMoveSpeed.X) ? TrueMoveSpeed : MinimumMoveSpeed);
            }
        }

        //Gets the amount of gold the enemy grants upon being killed
        public int GetGold
        {
            get { return Gold; }
        }

        public override bool IsDying
        {
            get { return (FakeDead == true); }
        }

        //Gets the fade color of the GoldDrop animation
        public Color GetColor
        {
            get 
            {
                // Check if the Enemy was killed and has begun the GoldDrop animation
                if (GoldDrop != null)
                {
                    // Return the fade color
                    return GoldDrop.GetFadeColor;
                }
                else
                {
                    // Return Light Coral if the enemy is slowed and White if not
                    return ((IsSlowed == true) ? Color.LightCoral : Color.White);
                }
            }
        }

        //Checks if the enemy can get hit
        //public override bool CanGetHit(Rectangle rect)
        //{
        //    return (base.CanGetHit(rect) == true && FakeDead == false);
        //}

        public override void GrantGold(Level level, bool killedbyplayer)
        {
            //Check if the enemy is killed by a player or not
            if (killedbyplayer == true)
            {
                //Find out the total amount of gold to give. If the player is using a suboptimal weapon (Ex. Warhammer instead of Sword), cut gold by 1/3
                if (WeaponWeakness == (int)Player.WeaponTypes.Sword && level.GetPlayer.CurWeapon != WeaponWeakness)
                    Gold = (int)(Gold * (float)(2 / 3f));
            }

            level.GetPlayer.ReceiveGold(Gold);
        }

        public override void Die(Level level)
        {
            FakeDead = true;
            GoldDrop = new FadeOnce(new Color(255, 255, 255, 255), -5, 0, 255, 0f);
        }

        public virtual void ApplySlow(Vector2 slowAmount, float slowDur)
        {
            // Set the slow amount
            SlowAmount = slowAmount;

            // Set the slow time
            SlowTime = (Game1.ActiveTime + slowDur);

            // State that the enemy is slowed
            IsSlowed = true;
        }

        public virtual void CheckEndSlow()
        {
            // Check if the enemy is currently marked as slowed but should no longer be slowed
            if ((IsSlowed == true) && (Game1.ActiveTime >= SlowTime))
            {
                // Reset the slow amount
                SlowAmount = new Vector2(0, 0);

                // State that the enemy is no longer slowed
                IsSlowed = false;
            }
        }

        protected abstract void ChooseNextAction(Level level);

        public override void Update(Level level)
        {
            if (FakeDead == false)
            {
                if (IsInvincible == true) InvincibilityFade.Update();
                CurAction.Update(level);
                if (CurAction.IsComplete == true) ChooseNextAction(level);
            }
            //Update gold dropping animation
            else
            {
                GoldDrop.Update();
                if (GoldDrop.DoneFading == true) base.Die(level);
            }

            // Check if the enemy's slow has ended
            CheckEndSlow();

            base.Update(level);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            CurAction.Draw(spriteBatch, ObjectSheet);
            //Draw gold dropping animation
            if (FakeDead == true)
            {
                //Center the text and gold icon above the enemy you killed
                Vector2 center = new Vector2(Position.X + (int)(Animation.CurrentAnimFrame.FrameSize.X / 2), GetTruePosition.Y - 10);
                Vector2 textsize = LoadAssets.DYFFont.MeasureString(Gold.ToString()) / 2;

                spriteBatch.DrawString(LoadAssets.DYFFont, Gold.ToString(), new Vector2(center.X, center.Y - 10), GoldDrop.GetFadeColor, 0f, textsize, 1f, SpriteEffects.None, .999f);
                spriteBatch.Draw(LoadAssets.GoldCoinEffect, center, null, GoldDrop.GetFadeColor, 0f, new Vector2(LoadAssets.GoldCoinEffect.Width / 2, LoadAssets.GoldCoinEffect.Height / 2), 1f, SpriteEffects.None, 1f);
            }

            base.Draw(spriteBatch);
        }
    }
}