using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_Your_Castle
{
    public sealed class Hitbox : LevelObject
    {
        //The hurtboxes this hitbox hit
        //public List<Hurtbox> Hurtboxes;

        //The box for collision
        private Vector2 WidthHeight;

        public Hitbox(Vector2 position, int width, int height)
        {
            Position = position;
            WidthHeight = new Vector2(width, height);

            //Hurtboxes = new List<Hurtbox>();
        }

        public int Width
        {
            get { return (int)WidthHeight.X; }
        }

        public int Height
        {
            get { return (int)WidthHeight.Y; }
        }

        //public void Reset()
        //{
        //    Hurtboxes.Clear();
        //}

        public Rectangle GetRect
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, (int)WidthHeight.X, (int)WidthHeight.Y); }
        }

        private bool CanHit(Hurtbox hurtbox)
        {
            return (GetRect.Intersects(hurtbox.GetRect));
        }
        //
        //protected void AddHurtbox(Hurtbox hurtbox)
        //{
        //    Hurtboxes.Add(hurtbox);
        //}

#if DEBUG
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Debug.HitboxShow == true)
            {
                spriteBatch.Draw(LoadAssets.ScalableBox, Position, null, Color.Red, 0f, Vector2.Zero, WidthHeight, SpriteEffects.None, .999f);

                base.Draw(spriteBatch);
            }
        }
#endif
    }
}
