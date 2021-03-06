﻿using System;
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
        private const int RangeIncrease = 75;
        private const float SpeedIncrease = 200f;//250f;

        //The range of the archer and its chance of hitting a nearby enemy; these can be increased with level
        private int AttackRange;
        private int AttackChance;

        private const int AttackTimeRange = 100;
        private float AttackTime;
        private float PrevAttack;

        public Archer(int index)
        {
            Victim = null;

            ObjectSheet = LoadAssets.PlayerArcher[HelperLevel];
            
            MaxLevel = 2;

            Animation = new Animation(new AnimFrame(new Rectangle(0, 0, 22, 35), 0f));
            AttackingAnim = new Animation(new AnimFrame(new Rectangle(23, 0, 24, 35), 200f, new Vector2(2, 0)), new AnimFrame(new Rectangle(48, 0, 26, 35), 500f, new Vector2(4, 0)));

            AttackRange = 75;//50;
            AttackChance = 5;//6;

            AttackTime = 900;//1000;
            PrevAttack = 0f;

            HelperIndex = index;
        }

        public bool IsAttacking
        {
            get { return (Victim != null); }
        }

        private bool CanAttack
        {
            get { return (Game1.ActiveTime >= PrevAttack); }
        }

        private float AttackDistance
        {
            get { return (Parent.GetPosition.X - AttackRange); }
        }

        private float GetAttackTime
        {
            get { return Rand.Next((int)AttackTime - AttackTimeRange, (int)AttackTime + (AttackTimeRange + 1)); }
        }

        private void RefreshAttackTimer()
        {
            PrevAttack = Game1.ActiveTime + GetAttackTime;
        }

        private bool CheckWeakness(LevelObject enemy)
        {
            //The Archer can hit more types of enemies as it levels up
            switch (HelperLevel)
            {
                case 0: return enemy.GetWeaponWeakness == (int)Player.WeaponTypes.Sword;
                case 1: return enemy.GetWeaponWeakness != (int)Player.WeaponTypes.Spear;
                default: return true;
            }
        }

        private void CheckAttackEnemy(Level level, LevelObject enemy)
        {
            //If the object is an enemy and is within a certain range, there is a chance of attacking it based on AttackChance
            if (enemy.GetObjectType == ObjectType.Enemy && CheckWeakness(enemy) == true && enemy.IsDying == false && enemy.IsInvincible == false && enemy.GetPosition.X >= AttackDistance)
            {
                int randnum = Rand.Next(0, AttackChance);

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

            Position = new Vector2(X, Parent.GetPosition.Y + 5);
        }

        //Make the archer attack faster, further, and more successfully
        protected override void IncreaseStats()
        {
            AttackChance -= ChanceIncrease;
            AttackRange += RangeIncrease;
            AttackTime -= SpeedIncrease;

            //Ensure that we don't access a value out of bounds
            if (HelperLevel < LoadAssets.PlayerArcher.Length)
                ObjectSheet = LoadAssets.PlayerArcher[HelperLevel];
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

                    //Whether we failed to attack the enemy or not, start the cooldown
                    RefreshAttackTimer();
                }
            }
            else
            {
                AttackingAnim.Update();

                //If the enemy already died while the archer was shooting, stop
                if (Victim.IsDying == true)
                    StopAttacking();
                else if (AttackingAnim.IsAnimationComplete == true)
                {
                    // Add a helper kill
                    (Parent as Player).AddHelperKill(level);
                    
                    // Kill the designated enemy
                    Victim.Die(level);
                    Victim.GrantGold(level, false);
                    
                    // Stop the helper from shooting
                    StopAttacking();
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
