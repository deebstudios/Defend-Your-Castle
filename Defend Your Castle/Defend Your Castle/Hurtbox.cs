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
        //The box for collision
        private Rectangle Box;

        public Hurtbox(Vector2 position, int width, int height)
        {
            Box = new Rectangle((int)position.X, (int)position.Y, width, height);
        }

#if DEBUG
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(LoadAssets.ScalableBox, new Vector2(Box.X, Box.Y), null, Color.Blue, 0f, Vector2.Zero, new Vector2(Box.Width, Box.Height), SpriteEffects.None, .999f);

            base.Draw(spriteBatch);
        }
#endif
    }
}