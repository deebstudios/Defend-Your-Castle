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
        protected Vector2 MoveSpeed;

        // Range of the Enemy. Use 1 for melee enemies
        protected int Range;

        // The point on the level at which the enemy will stop moving
        protected Vector2 StopPoint;

        // Movement durations
        private float MoveDuration;
        private float PrevMoveDuration;

        public Enemy(Animation animation, float moveDuration = 20)
        {
            // Set the enemy's properties
            MoveSpeed = new Vector2(5, 0);
            Range = 1;
            //StopPoint = (CastleX - Range);

            // Set the animation of the enemy
            Animation = animation;

            // Set the previous move timer to 0
            PrevMoveDuration = 0;

            Position = new Vector2(0, 100);

            // Set the move duration
            MoveDuration = moveDuration;
        }

        public override void Update()
        {
            // Check if the enemy can move
            if (Game1.ActiveTime >= PrevMoveDuration)
            {
                // Move the enemy
                Move(MoveSpeed);

                // Update the animation
                Animation.Update();

                // Reset the previous move timer
                PrevMoveDuration = (Game1.ActiveTime + MoveDuration);
            }

            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch, LoadAssets.testanim, Position, DirectionFacing, Color.White, 0f, 1f);

            base.Draw(spriteBatch);
        }

    }
}