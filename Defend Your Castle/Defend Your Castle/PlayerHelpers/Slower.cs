using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_Your_Castle
{
    // NOTE: Name should be changed to something more creative

    // An infinite-ranged helper that slows enemies that approach the castle. It reacts on a timer
    // It can slow any type of enemy, but not projectiles
    // The Slower is a child of the Player
    public sealed class Slower : PlayerHelper
    {
        // The amount the Slower's stats increase by each level
        private const int ChanceIncrease = 1;
        private const float SpeedIncrease = 150f;
        private Vector2 SlowAmountIncrease;
        private const float SlowDurIncrease = 250f;

        public const float AnimationSlow = .10f;

        // The Slower's chance of slowing a nearby enemy
        private int SlowChance;

        // The amount to slow the movement speed of enemies
        private Vector2 SlowAmount;

        // The duration of time enemies are slowed
        private float SlowDur;

        private const int AttackTimeRange = 100;
        private float AttackTime;
        private float PrevAttack;

        public Slower(int index)
        {
            Victim = null;

            ObjectSheet = LoadAssets.PlayerSlower[HelperLevel];

            MaxLevel = 2;

            Animation = new Animation(new AnimFrame(new Rectangle(0, 0, 17, 35), 0f));
            AttackingAnim = new Animation(true, new AnimFrame(new Rectangle(21, 0, 17, 35), 100f), new AnimFrame(new Rectangle(43, 0, 17, 35), 300f));

            SlowChance = 4;

            SlowAmount = new Vector2(0.5f, 0);
            SlowDur = 3000;

            // Set the slow amount increase
            SlowAmountIncrease = new Vector2(0.5f, 0);
            
            AttackTime = 1000;
            PrevAttack = 0f;

            HelperIndex = index;
        }

        public bool IsSlowing
        {
            get { return (Victim != null); }
        }

        private bool CanSlow
        {
            get { return (Game1.ActiveTime >= PrevAttack); }
        }

        private float GetAttackTime
        {
            get { return Rand.Next((int)AttackTime - AttackTimeRange, (int)AttackTime + (AttackTimeRange + 1)); }
        }

        private void RefreshAttackTimer()
        {
            PrevAttack = Game1.ActiveTime + GetAttackTime;
        }

        private void CheckSlowEnemy(Level level, LevelObject enemy)
        {
            //If the object is an enemy and is within a certain range, there is a chance of slowing it based on AttackChance
            if (enemy.GetObjectType == ObjectType.Enemy && enemy.IsDying == false && enemy.IsInvincible == false)
            {
                int randnum = Rand.Next(0, SlowChance);

                //We selected the enemy as our victim, so set it and start the attacking animation
                if (randnum == 0)
                {
                    Victim = enemy;
                    AttackingAnim.Restart();
                }
            }
        }

        public override void SetPosition()
        {
            float X = Parent.GetPosition.X + 5 + (HelperIndex * 56);

            Position = new Vector2(X, Parent.GetPosition.Y - 29);
        }

        //Make the Slower attack faster, further, and more successfully
        protected override void IncreaseStats()
        {
            SlowChance -= ChanceIncrease;
            AttackTime -= SpeedIncrease;
            SlowAmount += SlowAmountIncrease;
            SlowDur += SlowDurIncrease;

            //Ensure that we don't access a value out of bounds
            if (HelperLevel < LoadAssets.PlayerSlower.Length)
                ObjectSheet = LoadAssets.PlayerSlower[HelperLevel];
        }

        public override HelperData ConvertHelperToData()
        {
            return new SlowerData(HelperLevel, HelperIndex);
        }

        public override void Update(Level level)
        {
            if (IsSlowing == false)
            {
                if (CanSlow == true)
                {
                    // The max number of attempts the Slower can have to slow enemies
                    // After this point, the slower will simply give up and try again at the next attack
                    // This prevents an infinite or very long loop
                    int maxNumTries = level.GetEnemies.Count;

                    // The number of slow attempts the Slower has performed so far
                    int numSlowTries = 0;
                    
                    // Stores the randomly-generated enemy index
                    int randEnemyIndex;

                    // Loop until the Slower finds an enemy to slow or has tried too many times
                    while (IsSlowing == false && numSlowTries < maxNumTries)
                    {
                        // Get a random enemy index
                        randEnemyIndex = Rand.Next(0, level.GetEnemies.Count);

                        // Get the random enemy
                        LevelObject enemy = level.GetEnemies[randEnemyIndex];

                        // Check if this enemy can be set as the Slower's victim
                        CheckSlowEnemy(level, enemy);

                        // Increment the number of slow attempts by 1
                        numSlowTries += 1;
                    }

                    //Whether we failed to slow the enemy or not, start the cooldown
                    RefreshAttackTimer();
                }
            }
            else
            {
                AttackingAnim.Update();

                // If the enemy already died while the Slower was attacking, stop
                if (Victim.IsDying == true)
                    StopAttacking();
                else if (AttackingAnim.IsAnimationComplete == true)
                {
                    // Slow the designated target
                    (Victim as Enemy).ApplySlow(SlowAmount, SlowDur, HelperLevel);

                    // Stop the helper from shooting
                    StopAttacking();
                }
            }

            base.Update(level);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Animation drawanim = ((IsSlowing == false) ? Animation : AttackingAnim);

            drawanim.Draw(spriteBatch, ObjectSheet, Position, Direction.Right, Color.White, 0f, .998f);

            base.Draw(spriteBatch);
        }


    }
}