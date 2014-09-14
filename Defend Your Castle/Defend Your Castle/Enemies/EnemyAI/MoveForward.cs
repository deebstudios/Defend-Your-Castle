using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Defend_Your_Castle
{
    //The enemy moves forward and stops at a certain point in front of the player castle
    //This is the default behavior for an enemy
    public sealed class MoveForward : Action
    {
        //The X to stop
        public int XStop;

        public MoveForward(Enemy enem, Animation anim, int xstop) : base(enem, anim, ActionType.Moving)
        {
            XStop = xstop;
        }

        public override void Update()
        {
            base.Update();

            enemy.Move(enemy.GetMoveSpeed);

            //Check position
            if (enemy.GetPosition.X >= XStop)
            {
                Complete = true;

                //Move the enemy to the exact spot the enemy should stop if the enemy passed the stop location
                if (enemy.GetPosition.X > XStop) enemy.Move(new Vector2(XStop - enemy.GetPosition.X, 0));
            }
        }
    }
}
