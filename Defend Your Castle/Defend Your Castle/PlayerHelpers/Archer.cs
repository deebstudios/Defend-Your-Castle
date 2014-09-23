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
    public sealed class Archer : LevelObject
    {
        //The enemy the archer is attacking
        private LevelObject Victim;

        private Animation AttackingAnim;

        //The range of the archer and its chance of hitting a nearby enemy; these can be increased with level
        private int AttackRange;
        private int AttackChance;

        private float AttackTime;
        private float PrevAttack;

        public Archer(Level level)
        {
            Victim = null;

            ObjectSheet = LoadAssets.PlayerArcher;

            Animation = new Animation(new AnimFrame(new Rectangle(0, 0, 9, 16), 0f));
            AttackingAnim = new Animation(new AnimFrame(new Rectangle(9, 0, 7, 16), 500f), new AnimFrame(new Rectangle(16, 0, 7, 16), 300f));

            AttackRange = 50;
            AttackChance = 6;

            AttackTime = 1000;
            PrevAttack = 0f;

            //TEMPORARY
            Position = new Vector2(level.GetPlayer.GetPosition.X - 20, 140);
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
            if (enemy.GetObjectType == ObjectType.Enemy && enemy.GetWeaponWeakness == (int)Player.WeaponTypes.Sword && enemy.IsDying == false && enemy.GetPosition.X >= AttackDistance(level))
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
                    Victim.Die(level);
                    StopShooting();
                }
            }

            base.Update(level);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            Animation drawanim = IsAttacking == false ? Animation : AttackingAnim;

            drawanim.Draw(spriteBatch, ObjectSheet, Position, Direction.Right, Color.White, 0f, 1f); 

            base.Draw(spriteBatch);
        }
    }
}
