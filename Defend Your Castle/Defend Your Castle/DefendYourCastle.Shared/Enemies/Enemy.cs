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
        //The minimum speed an enemy can move
        protected static readonly Vector2 MinimumMoveSpeed;

        //"Fake" death for the enemy, indicating that the gold animation must play now
        protected bool FakeDead;

        //The fade for the gold drop
        protected FadeOnce GoldDrop;

        // Movement speed
        protected Vector2 MoveSpeed;

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

            ObjectSheet = LoadAssets.GoblinSheet[0];

            // Set the slow amount
            SlowAmount = new Vector2(0, 0);

            //Default length
            InvincibilityLength = 1500f;
            InvincibilityFade = new Fade(Color.White, 10, 0, 255, Fade.InfiniteLoops, 0f);

            Gold = 100;

            Position = new Vector2(0, 100);

            WeaponWeakness = (int)Player.WeaponTypes.Sword;
        }

        static Enemy()
        {
            // Set the minimum movement speed
            MinimumMoveSpeed = new Vector2(0.1f, 0);
        }

        protected void SetProperties(Level level)
        {
            #if WINDOWS_APP
                SetHurtbox((int)Animation.CurrentAnimFrame.FrameSize.X, (int)Animation.CurrentAnimFrame.FrameSize.Y, new Vector2(10));
            #else
                SetHurtbox((int)Animation.CurrentAnimFrame.FrameSize.X, (int)Animation.CurrentAnimFrame.FrameSize.Y, new Vector2(15));
            #endif

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
                // Get the movement speed of the enemy
                Vector2 TrueMoveSpeed = MoveSpeed - SlowAmount;

                // If the enemy is slowed, decrease its movement speed
                //if (IsSlowed == true) TrueMoveSpeed -= SlowAmount;

                //Check the true movement speed's X value. If it is less than the minimum movement speed's X value, set it to the minimum
                if (TrueMoveSpeed.X < MinimumMoveSpeed.X) TrueMoveSpeed.X = MinimumMoveSpeed.X;

                return TrueMoveSpeed;
            }
        }

        //Gets the amount of gold the enemy grants upon being killed
        public int GetGold
        {
            get { return Gold; }
        }

        public bool IsSlowed
        {
            get { return (Game1.ActiveTime < SlowTime); }
        }

        public override bool IsDying
        {
            get { return (FakeDead == true); }
        }

        //Gets the shade color to apply to the enemy
        public Color GetColor
        {
            get 
            {
                // Check if the Enemy was killed and has begun the GoldDrop animation
                if (GoldDrop != null)
                {
                    // Return the fade color of the GoldDrop animation
                    return GoldDrop.GetFadeColor;
                }
                else
                {
                    // Return Light Pink if the enemy is slowed and White if not
                    return ((IsSlowed == true) ? Color.LightPink : Color.White);
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
            SoundManager.PlaySound(LoadAssets.GoblinDeath);

            FakeDead = true;
            GoldDrop = new FadeOnce(new Color(255, 255, 255, 255), -5, 0, 255, 0f);
        }

        public void ApplySlow(Vector2 slowAmount, float slowDur, int slowerlevel)
        {
            // Set the slow amount
            SlowAmount = slowAmount;

            // Set the slow time
            SlowTime = (Game1.ActiveTime + slowDur);

            //Slow down the current animation
            float slowamount = (Slower.AnimationSlow * (slowerlevel + 1));
            CurAction.GetAnim.ChangeAnimSpeed(slowamount);
        }

        public void SetInvincible(float invincibilityduration)
        {
            InvincibilityLength = invincibilityduration;
            UseInvincibility();
        }

        protected abstract void ChooseNextAction(Level level);

        protected void CheckEndSlow()
        {
            if (SlowAmount != Vector2.Zero && IsSlowed == false)
            {
                SlowAmount = Vector2.Zero;

                CurAction.GetAnim.RestoreAnimSpeed();
            }
        }

        public sealed override void Update(Level level)
        {
            if (FakeDead == false)
            {
                CheckEndSlow();

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