using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_Your_Castle
{
    public class Enemy : LevelObject
    {
        // Movement speed
        private Vector2 MoveSpeed;

        // Range of the Enemy. Use 1 for melee enemies
        private int Range;

        // The point on the level at which the enemy will stop moving
        private Vector2 StopPoint;

        public Enemy()
        {
            // Set the enemy's properties
            MoveSpeed = new Vector2(10, 0);
            Range = 1;

            //StopPoint = (CastleX - Range);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            

            base.Draw(spriteBatch);
        }

    }
}