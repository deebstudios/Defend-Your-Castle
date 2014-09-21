using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_Your_Castle
{
    //The Level where gameplay takes place; it handles all the LevelObjects
    public sealed class Level
    {
        // Stores the level number of the level
        private int LevelNum;

        //The player; the player's children are the things that are helpful to the player
        //The player and its children persist between levels
        private Player player;

        // The duration of the level (in milliseconds)
        private float LevelDuration;

        // The time at which the level will end
        private float LevelEndTime;

        //The list of enemies and other things harmful to the player
        private List<LevelObject> Enemies;

        // Stores the spawn chance for each enemy in the spawn list. The sum of all items in the list should be 100
        private List<int> EnemySpawnChances;

        // The amount of time (in milliseconds) it will take for the next enemy to spawn
        private float SpawnTime;

        // The time at which the next enemy will be spawned
        private float NextSpawnTime;

        public Level(Player play)
        {
            player = play;

            Enemies = new List<LevelObject>();

            // Initialize the enemy spawn chance list
            EnemySpawnChances = new List<int>() { 50, 50 };

            // NOTE: This calculation will need to be changed
            // Set the level duration
            LevelDuration = (30000 + ((LevelNum - 1) * 1000));

            // Set the level number to 1 (for testing)
            LevelNum = 1;

            //// Check if a new enemy should be added
            //if (LevelNum >= 13)
            //{
            //    // Add a new enemy to the enemy spawn list
            //    EnemySpawnList.Add();

            //    // Store the new enemy's spawn chance
            //    // NOTE: Test spawn chance
            //    int NewEnemySpawnChance = 10;

            //    // Calculate the number of points to reduce the other spawn chances to accommodate the new spawn chance
            //    // Round up to the nearest integer
            //    int ChanceDecrease = (int)Math.Ceiling((double)(NewEnemySpawnChance / EnemySpawnChances.Count));

            //    // Re-calculate the enemy spawn chances
            //    for (int i = 0; i < EnemySpawnChances.Count; i++)
            //    {
            //        // Decrease the enemy's spawn chance by the chance decrease
            //        EnemySpawnChances[i] -= ChanceDecrease;
            //    }

            //    // Get the remaining chance points, if any
            //    int RemainingPoints = 100 - GetSpawnChancesSum();

            //    // Store the index of the spawn chance
            //    int SpawnChanceIndex = 0;

            //    // Loop as long as there are remaining points
            //    while (RemainingPoints > 0)
            //    {
            //        // Go back to the beginning of the spawn chance list if the end is reached
            //        if (SpawnChanceIndex == EnemySpawnChances.Count) SpawnChanceIndex = 0;

            //        // Increase the enemy spawn chance by 1%
            //        EnemySpawnChances[SpawnChanceIndex] += 1;

            //        // Move onto the next enemy spawn chance
            //        SpawnChanceIndex += 1;

            //        // Decrement the remaining points by 1
            //        RemainingPoints -= 1;
            //    }

            //    // Add the new enemy spawn chance to the list
            //    EnemySpawnChances.Add(NewEnemySpawnChance);
            //}
        }

        //private int GetSpawnChancesSum()
        //{
        //    // Stores the sum of the enemy spawn chances
        //    int Sum = 0;

        //    // Loop through all of the enemy spawn chances
        //    for (int i = 0; i < EnemySpawnChances.Count; i++)
        //    {
        //        // Add the spawn chance to the sum
        //        Sum += EnemySpawnChances[i];
        //    }

        //    // Return the sum
        //    return Sum;
        //}

        //Returns the player reference
        public Player GetPlayer
        {
            get { return player; }
        }

        public bool CanEnemySpawn
        {
            get { return (Game1.ActiveTime >= NextSpawnTime); }
        }

        //Adds a player-helping object to the player
        public void AddPlayerHelper(LevelObject helper)
        {
            player.AddChild(helper);
        }

        //Adds a player-harming object to the level; these objects always have no parents and are added here if removed from their parents
        public void AddEnemy(LevelObject enemy)
        {
            Enemies.Add(enemy);
        }

        private void UpdateEnemies()
        {
            //Update enemies
            for (int i = 0; i < Enemies.Count; i++)
            {
                if (Enemies[i].IsDead == false)
                    Enemies[i].Update(this);
                else
                {
                    Enemies.RemoveAt(i);
                    i--;
                }
            }
        }

        //Make the player hit an enemy if it attacked
        public void EnemyHit(Rectangle rect)
        {
            for (int i = 0; i < Enemies.Count; i++)
            {
                //The "as" cast is temporary!
                if (Enemies[i].CanGetHit(rect) == true && player.CurWeapon >= Enemies[i].GetWeaponWeakness)
                {
                    Enemies[i].Die(this);
                    break;
                }
            }
        }

        private int FindEnemyNumToSpawn(int RandNum)
        {
            // The minimum number in the range at which the enemy can spawn
            int RangeMin = 1;

            // The maximum number in the range at which the enemy can spawn
            int RangeMax = 0;

            // Loop through all of the enemy spawn chances
            for (int i = 0; i < EnemySpawnChances.Count; i++)
            {
                // Add the enemy's spawn chance to the range maximum
                RangeMax += EnemySpawnChances[i];

                // Check if the enemy's spawn chance is within the range
                if (RandNum >= RangeMin && RandNum <= RangeMax)
                {
                    // Return the enemy index
                    return i;
                }

                // Set the range minimum for the next enemy
                // Ex: RangeMin = 1; RangeMax = 12
                //     After the following statement, RangeMin will = 13
                RangeMin = (RangeMax + 1);
            }

            // Spawn a random enemy if, for some reason, no enemy can be found
            return ((new Random()).Next(0, EnemySpawnChances.Count));
        }

        private Enemy FindEnemyToSpawn(int RandNum)
        {
            // Get the Enemy number to spawn
            int EnemyIndex = FindEnemyNumToSpawn(RandNum);

            switch (EnemyIndex)
            {
                case 0: // Enemy
                    return (new MeleeEnemy(this));
                case 1: // Spear Enemy
                    return (new SpearEnemy(this));
                default:
                    goto case 0;
            }
        }

        public void SpawnEnemy()
        {
            // Check if an enemy can be spawned
            if (CanEnemySpawn == true)
            {
                // Find an enemy to spawn
                Enemy EnemyToSpawn = FindEnemyToSpawn(new Random().Next(1, 100));

                // Add the enemy that should be spawned
                AddEnemy(EnemyToSpawn);

                // NOTE: This calculation will need to be changed. It was just set up with these values for now
                // Set the minimum spawn time to depend on the level of the player
                // The minimum spawn time decreases by 45 milliseconds each level
                int MinSpawnTime = (3000 - (45 * (LevelNum - 1)));

                // Set the maximum spawn time to be double the minimum spawn time
                // Add 1 to include the maximum spawn time
                int MaxSpawnTime = (MinSpawnTime * 2) + 1;

                // Randomly generate the next spawn time for the enemy
                SpawnTime = (new Random()).Next(MinSpawnTime, MaxSpawnTime);

                // Set the next time an enemy will be spawned
                NextSpawnTime = Game1.ActiveTime + SpawnTime;
            }
        }

        public void Update()
        {
            UpdateEnemies();

            //Update the player
            player.Update(this);

            // Try to spawn a new enemy
            SpawnEnemy();
        }

        private void DrawEnemies(SpriteBatch spriteBatch)
        {
            //Draw enemies
            for (int i = 0; i < Enemies.Count; i++)
            {
                if (Enemies[i].IsDead == false)
                    Enemies[i].Draw(spriteBatch);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawEnemies(spriteBatch);

            //Draw the player
            player.Draw(spriteBatch);
        }
    }
}
