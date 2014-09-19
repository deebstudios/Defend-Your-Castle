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
    //NOTE: This, along with Enemy, need to be way more flexible than they currently are. At the moment I'm just testing if they work or not
    public class Projectile : LevelObject
    {
        //Tells if the projectile has been launched or not
        protected bool Launched;

        //The damage the projectile does
        protected int Damage;

        //Velocity of the projectile
        protected Vector2 Velocity;
        protected Vector2 CurVelocity;

        protected AnimFrame Sprite;

        private Projectile()
        {
            Velocity = Vector2.Zero;
        }

        public Projectile(Vector2 velocity, Texture2D spritesheet, AnimFrame sprite)
        {
            Velocity = velocity;
            if (Velocity.X < 0) DirectionFacing = Direction.Left;

            Damage = 1;

            ObjectSheet = spritesheet;
            Sprite = sprite;
        }

        //Defines rules for setting the rotation of a projectile
        protected virtual void CheckRotation()
        {

        }

        //Launches the projectile
        public void Launch(Vector2 position)
        {
            Position = position;
            CurVelocity = Velocity;
            CheckRotation();

            int width = (int)Sprite.FrameSize.X;
            int height = (int)Sprite.FrameSize.Y;
            SetHitbox(width, height);
            SetHurtbox(width, height);

            Launched = true;
        }

        public override void Update(Level level)
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

                base.Update(level);

                //If the projectile hitbox touches the player, make the player take damage and remove the projectile from the level
                if (hitbox.GetRect.Right >= level.GetPlayer.GetPosition.X)
                {
                    level.GetPlayer.TakeDamage(Damage, level);
                    Die(level);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Launched == true)
            {
                Sprite.Draw(spriteBatch, ObjectSheet, Position, DirectionFacing, Color.White, Rotation, .999f);

                base.Draw(spriteBatch);
            }
        }
    }
}
