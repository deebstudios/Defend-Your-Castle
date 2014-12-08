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

        public Hurtbox(Vector2 position, int width, int height, Vector2 padamount)
        {
            Position = position - (padamount / 2);
            WidthHeight = new Vector2(width + (int)padamount.X, height + (int)padamount.Y);
        }

        public int Width
        {
            get { return (int)WidthHeight.X; }
        }

        public int Height
        {
            get { return (int)WidthHeight.Y; }
        }

        public Rectangle GetRect
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, (int)WidthHeight.X, (int)WidthHeight.Y); }
        }

        public bool CanBeHit(Rectangle rect)
        {
            return (rect.Intersects(GetRect));
        }

#if DEBUG
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Debug.HurtboxShow == true)
            {
                spriteBatch.Draw(LoadAssets.ScalableBox, Position, null, Color.Blue, 0f, Vector2.Zero, WidthHeight, SpriteEffects.None, .999f);

                base.Draw(spriteBatch);
            }
        }
#endif
    }
}