using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_Your_Castle
{
    //A spear for ranged enemies to throw
    public sealed class Spear : Projectile
    {
        public Spear(Vector2 velocity) : base(velocity)
        {
            ObjectSheet = LoadAssets.EnemySpear;
            Sprite = new AnimFrame(new Rectangle(0, 0, LoadAssets.EnemySpear.Width, LoadAssets.EnemySpear.Height), 0f);

            Damage = 20;
        }

        //The spear is oriented diagonally, so we'll simply change it as we see fit
        protected override void CheckRotation()
        {
            //Facing up-right
            if (CurVelocity.Y < 0f) Rotation = 0f;
            //Facing straight right
            else if (CurVelocity.Y == 0f) Rotation = ((float)Math.PI / 4f);
            //Facing down-right
            else Rotation = ((float)Math.PI / 2f);
        }
    }
}
