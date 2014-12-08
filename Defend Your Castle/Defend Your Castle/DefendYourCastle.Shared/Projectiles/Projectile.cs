using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_Your_Castle
{
    //A projectile
    public abstract class Projectile : LevelObject
    {
        //Tells if the projectile has been launched or not
        protected bool Launched;

        //The damage the projectile does
        protected int Damage;

        //Amount of gold the projectile gives
        protected int Gold;

        protected bool FakeDead;

        //The fade for the gold drop
        protected FadeOnce GoldDrop;

        //Velocity of the projectile
        protected Vector2 Velocity;
        protected Vector2 CurVelocity;

        //The position to stop and damage the castle
        protected int StopX;

        protected AnimFrame Sprite;

        public Projectile()
        {
            Velocity = Vector2.Zero;

            Damage = 1;

            Gold = 15;
            FakeDead = false;
            GoldDrop = null;

            Launched = false;

            UsesGravity = true;
        }

        public Projectile(Vector2 velocity) : this()
        {
            Velocity = velocity;
            if (Velocity.X < 0) DirectionFacing = Direction.Left;
        }

        //Defines rules for setting the rotation of a projectile
        protected virtual void CheckRotation()
        {

        }

        //Launches the projectile
        public void Launch(Vector2 position, Vector2 animsize, Vector2 playerpos)
        {
            Position = position;
            CurVelocity = Velocity;
            CheckRotation();

            objectType = ObjectType.Projectile;

            int width = (int)Sprite.FrameSize.X;
            int height = (int)Sprite.FrameSize.Y;
            SetHitbox(width, height);

            #if WINDOWS_APP
                SetHurtbox(width, height, new Vector2(5, 5));
            #else
                SetHurtbox(width, height, new Vector2(10, 10));
            #endif

            StopX = StopAtCastle(playerpos, Sprite.FrameSize + new Vector2(0, animsize.Y), 0);

            Launched = true;
        }

        public override bool IsDying
        {
            get { return (FakeDead == true); }
        }

        //Gives gold to the player
        public override void GrantGold(Level level, bool killedbyplayer)
        {
            level.GetPlayer.ReceiveGold(Gold);
        }

        public override void Die(Level level)
        {
            FakeDead = true;
            GoldDrop = new FadeOnce(new Color(255, 255, 255, 255), -5, 0, 255, 0f);
        }

        public override void Update(Level level)
        {
            if (FakeDead == false)
            {
                if (Launched == true)
                {
                    Move(CurVelocity);
                    if (UsesGravity == true)
                    {
                        CurVelocity.Y += LevelObject.Gravity;

                        //Rotate the projectile
                        CheckRotation();
                    }

                    //If the projectile hitbox touches the player, make the player take damage and remove the projectile from the level
                    if (hitbox.GetRect.Right >= StopX)
                    {
                        level.GetPlayer.TakeDamage(Damage, level);
                        base.Die(level);
                    }
                }
            }
            else
            {
                GoldDrop.Update();
                if (GoldDrop.DoneFading == true)
                    base.Die(level);
            }

            base.Update(level);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Launched == true)
            {
                Sprite.Draw(spriteBatch, ObjectSheet, Position, DirectionFacing, GoldDrop != null ? GoldDrop.GetFadeColor : Color.White, Rotation, GetDrawDepth);

                //Draw gold dropping animation
                if (FakeDead == true)
                {
                    //Center the text and gold icon above the enemy you killed
                    Vector2 center = new Vector2(Position.X + (int)(Sprite.FrameSize.X / 2), GetTruePosition.Y - 10);
                    Vector2 textsize = LoadAssets.DYFFont.MeasureString(Gold.ToString()) / 2;

                    spriteBatch.DrawString(LoadAssets.DYFFont, Gold.ToString(), new Vector2(center.X, center.Y - 10), GoldDrop.GetFadeColor, 0f, textsize, 1f, SpriteEffects.None, .999f);
                    spriteBatch.Draw(LoadAssets.GoldCoinEffect, center, null, GoldDrop.GetFadeColor, 0f, new Vector2(LoadAssets.GoldCoinEffect.Width / 2, LoadAssets.GoldCoinEffect.Height / 2), 1f, SpriteEffects.None, 1f);
                }

                base.Draw(spriteBatch);
            }
        }
    }
}
