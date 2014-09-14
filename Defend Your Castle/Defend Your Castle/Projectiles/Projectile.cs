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
    public class Projectile : LevelObject
    {
        //Velocity and direction of the projectile
        protected Vector2 Velocity;
        protected Texture2D SpriteSheet;
        protected AnimFrame Sprite;

        private Projectile()
        {
            Velocity = Vector2.Zero;
        }

        public Projectile(Vector2 position, Vector2 velocity, Texture2D spritesheet, AnimFrame sprite)
        {
            Position = position;
            Velocity = velocity;
            if (Velocity.X < 0) DirectionFacing = Direction.Left;

            SpriteSheet = spritesheet;
            Sprite = sprite;

            int width = (int)Sprite.FrameSize.X;
            int height = (int)Sprite.FrameSize.Y;
            SetHitbox(width, height);
            SetHurtbox(width, height);
        }

        public override void Update(Level level)
        {
            Move(Velocity);

            base.Update(level);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, SpriteSheet, Position, DirectionFacing, Color.White, 0f, .999f);

            base.Draw(spriteBatch);
        }
    }
}
