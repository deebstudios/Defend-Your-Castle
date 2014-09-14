using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_Your_Castle
{
    //A hurtbox for LevelObjects
    public sealed class Hurtbox : LevelObject
    {
        //The width and height for collision
        private Vector2 WidthHeight;

        public Hurtbox(Vector2 position, int width, int height)
        {
            Position = position;
            WidthHeight = new Vector2(width, height);
        }

        public Rectangle GetRect
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, (int)WidthHeight.X, (int)WidthHeight.Y); }
        }

#if DEBUG
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(LoadAssets.ScalableBox, Position, null, Color.Blue, 0f, Vector2.Zero, WidthHeight, SpriteEffects.None, .999f);

            base.Draw(spriteBatch);
        }
#endif
    }
}