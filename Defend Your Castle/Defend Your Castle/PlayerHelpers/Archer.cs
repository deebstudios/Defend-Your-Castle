using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_Your_Castle
{
    //A close-range archer that helps the player by attacking enemies that approach the castle. It reacts on a timer
    //It does not attack Projectiles nor ranged enemies (they're too far) and can only hurt enemies that the Sword can
    //The Archer is a child of the Player
    public sealed class Archer : PlayerHelper
    {
        //The amount the Archer's stats increase by each level
        private const int ChanceIncrease = 1;
        private const int RangeIncrease = 50;
        private const float SpeedIncrease = 200f;

        //The enemy the archer is attacking
        private LevelObject Victim;

        private Animation AttackingAnim;

        //The range of the archer and its chance of hitting a nearby enemy; these can be increased with level
        private int AttackRange;
        private int AttackChance;

        private float AttackTime;
        private float PrevAttack;

        public Archer(int index)
        {
            Victim = null;

            ObjectSheet = LoadAssets.PlayerArcher;

            MaxLevel = 4;

            Animation = new Animation(new AnimFrame(new Rectangle(0, 0, 22, 35), 0f));
            AttackingAnim = new Animation(new AnimFrame(new Rectangle(23, 0, 24, 35), 500f, new Vector2(2, 0)), new AnimFrame(new Rectangle(48, 0, 26, 35), 300f, new Vector2(4, 0)));

            AttackRange = 50;
            AttackChance = 6;

            AttackTime = 1000;
            PrevAttack = 0f;

            HelperIndex = index;
            //TEMPORARY
            //Position = new Vector2(/*level.GetPlayer.GetPosition.X - 20*/Game1.ScreenSize.X - LoadAssets.PlayerCastle.Width - 20, 140);
        }

        public bool IsAttacking
        {
            get { return (Victim != null); }
        }

        private bool CanAttack
        {
            get { return (Game1.ActiveTime >= PrevAttack); }
        }

        private float AttackDistance(Level level)
        {
            return (level.GetPlayer.GetPosition.X - AttackRange);
        }

        private void RefreshAttackTimer()
        {
            PrevAttack = Game1.ActiveTime + AttackTime;
        }

        private void CheckAttackEnemy(Level level, LevelObject enemy)
        {
            //If the object is an enemy, can be killed by the Sword, and is within a certain range, there is a chance of attacking it based on AttackChance
            if (enemy.GetObjectType == ObjectType.Enemy && enemy.GetWeaponWeakness == (int)Player.WeaponTypes.Sword && enemy.IsDying == false && enemy.IsInvincible == false && enemy.GetPosition.X >= AttackDistance(level))
            {
                Random random = new Random();
                int randnum = random.Next(0, AttackChance);

                //We selected the enemy as our victim, so set it and start the attacking animation
                if (randnum == 0)
                {
                    Victim = enemy;
                    AttackingAnim.Restart();
                }

                //Whether we failed to attack the enemy or not, start the cooldown
                RefreshAttackTimer();
            }
        }

        private void StopShooting()
        {
            Victim = null;
        }

        public override void SetPosition()
        {
            float X = Parent.GetPosition.X + 48 + (HelperIndex * 55);

            //Find out how much to increase the Archer's X location based on its index
            //switch(HelperIndex)
            //{
            //    case 1: X += 55;
            //        break;
            //    case 2: X += 110;
            //        break;
            //    default: break;
            //}

            Position = new Vector2(X, 75);
        }

        //Make the archer attack faster, further, and more successfully
        public override void IncreaseStats()
        {
            AttackChance -= ChanceIncrease;
            AttackRange += RangeIncrease;
            AttackTime -= SpeedIncrease;
        }

        public override HelperData ConvertHelperToData()
        {
            return new ArcherData(HelperLevel, HelperIndex);
        }

        public override void Update(Level level)
        {
            if (IsAttacking == false)
            {
                if (CanAttack == true)
                {
                    for (int i = 0; i < level.GetEnemies.Count; i++)
                    {
                        LevelObject enemy = level.GetEnemies[i];

                        //Check to set this enemy as your victim
                        CheckAttackEnemy(level, enemy);
                        if (IsAttacking == true) break;
                    }
                }
            }
            else
            {
                AttackingAnim.Update();

                //If the enemy already died while the archer was shooting, stop
                if (Victim.IsDying == true)
                    StopShooting();
                else if (AttackingAnim.IsAnimationComplete == true)
                {
                    // Add a helper kill
                    (Parent as Player).AddHelperKill(level);
                    
                    // Kill the designated enemy
                    Victim.Die(level);
                    Victim.GrantGold(level, false);
                    
                    // Stop the helper from shooting
                    StopShooting();
                }
            }

            base.Update(level);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Animation drawanim = IsAttacking == false ? Animation : AttackingAnim;

            drawanim.Draw(spriteBatch, ObjectSheet, Position, Direction.Right, Color.White, 0f, .998f); 

            base.Draw(spriteBatch);
        }
    }
}
