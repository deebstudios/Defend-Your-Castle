using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defend_Your_Castle
{
    public sealed class EnemySpawning
    {
        // Reference to the level
        private Level level;

        // Stores the spawn chance for each enemy in the spawn list. The sum of all items in the list should be 100
        private List<int> EnemySpawnChances;

        // The amount of time (in milliseconds) it will take for the next enemy to spawn
        private float SpawnTime;

        // The time at which the next enemy will be spawned
        private float NextSpawnTime;

        public EnemySpawning(Level theLevel)
        {
            // Get the reference to the level
            level = theLevel;

            // Initialize the enemy spawn chance list
            EnemySpawnChances = new List<int>() { 33, 33, 34 };
        }

        public bool CanEnemySpawn
        {
            get { return (Game1.ActiveTime >= NextSpawnTime); }
        }

        public void CheckAddSpawnEnemy()
        {
            switch (level.GetLevelNum)
            {
                case 13:
                    // Add a new enemy to the enemy spawn list
                    AddSpawnEnemy(10);

                    break;
                default:
                    break;
            }
        }

        private void AddSpawnEnemy(int NewEnemySpawnChance)
        {
            // Calculate the number of points to reduce the other spawn chances to accommodate the new spawn chance
            // Round up to the nearest integer
            int ChanceDecrease = (int)Math.Ceiling((double)(NewEnemySpawnChance / EnemySpawnChances.Count));

            // Re-calculate the enemy spawn chances
            for (int i = 0; i < EnemySpawnChances.Count; i++)
            {
                // Decrease the enemy's spawn chance by the chance decrease
                EnemySpawnChances[i] -= ChanceDecrease;
            }

            // Get the remaining chance points, if any
            int RemainingPoints = 100 - GetSpawnChancesSum();

            // Store the index of the spawn chance
            int SpawnChanceIndex = 0;

            // Loop as long as there are remaining points
            while (RemainingPoints > 0)
            {
                // Go back to the beginning of the spawn chance list if the end is reached
                if (SpawnChanceIndex == EnemySpawnChances.Count) SpawnChanceIndex = 0;

                // Increase the enemy spawn chance by 1%
                EnemySpawnChances[SpawnChanceIndex] += 1;

                // Move onto the next enemy spawn chance
                SpawnChanceIndex += 1;

                // Decrement the remaining points by 1
                RemainingPoints -= 1;
            }

            // Add the new enemy spawn chance to the list
            EnemySpawnChances.Add(NewEnemySpawnChance);
        }

        private int GetSpawnChancesSum()
        {
            // Stores the sum of the enemy spawn chances
            int Sum = 0;

            // Loop through all of the enemy spawn chances
            for (int i = 0; i < EnemySpawnChances.Count; i++)
            {
                // Add the spawn chance to the sum
                Sum += EnemySpawnChances[i];
            }

            // Return the sum
            return Sum;
        }

        private int FindEnemyNumToSpawn(int RandNum)
        {
            // The cumulative percent to search
            int CumulativePercent = 0;

            // Loop through all of the enemy spawn chances - this acts as a restriction for which enemies can be spawned
            for (int i = 0; i < EnemySpawnChances.Count; i++)
            {
                // Add the enemy's spawn chance to the cumulative percentage
                CumulativePercent += EnemySpawnChances[i];

                // Check if the enemy can be spawned
                if (CumulativePercent > RandNum)
                {
                    // Return the enemy number
                    return i;
                }
            }

            // Spawn a random enemy if, for some reason, no enemy can be found
            return (new Random().Next(0, EnemySpawnChances.Count));
        }

        private Enemy FindEnemyToSpawn(int RandNum)
        {
            // Get the Enemy number to spawn
            int EnemyIndex = FindEnemyNumToSpawn(RandNum);

            switch (EnemyIndex)
            {
                case 1: // Spear Enemy
                    return (new SpearEnemy(level));
                case 2: //Armored enemy
                    return (new ArmoredEnemy(level));
                case 0: // Melee Enemy
                default:
                    return (new MeleeEnemy(level));
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
                level.AddEnemy(EnemyToSpawn);

                // NOTE: This calculation will need to be changed. It was just set up with these values for now
                // Set the minimum spawn time to depend on the level of the player
                // The minimum spawn time decreases by 45 milliseconds each level
                int MinSpawnTime = (3000 - (45 * (level.GetLevelNum - 1)));

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
            // Try to spawn a new enemy
            SpawnEnemy();
        }


    }
}