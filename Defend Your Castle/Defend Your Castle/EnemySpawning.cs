using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defend_Your_Castle
{
    public sealed class EnemySpawning
    {
        //The number of levels required for the min and max speed enemies increase their base speed by to go up by 1
        //Ex. If MinSpeedIncrease = 5, every 5 levels the minimum amount enemies increase their base speed by goes up by 1
        private const int MinSpeedIncrease = 10;
        private const int MaxSpeedIncrease = 7;

        // Reference to the level
        private Level level;

        // Stores the spawn chance for each enemy in the spawn list. The sum of all items in the list should be 100
        private List<int> EnemySpawnChances;

        // The spawn delay (in milliseconds) for the spawn timer. Enemies will spawn after the level start animation
        private const float SpawnDelay = 2000f;

        // The spawn delay timer
        private float PrevSpawnDelay;
        
        // The amount of time (in milliseconds) it will take for the next enemy to spawn
        private float SpawnTime;

        // The time at which the next enemy will be spawned
        private float NextSpawnTime;

        //Our random number generator
        private Random RandGenerator;

        public EnemySpawning(Level theLevel)
        {
            // Get the reference to the level
            level = theLevel;

            RandGenerator = new Random();

            // Initialize the enemy spawn chance list
            EnemySpawnChances = new List<int>() { 25, 25, 25, 25 };
        }

        // Determines if any enemies can start spawning for the level
        private bool CanStartSpawning
        {
            get { return (Game1.ActiveTime >= PrevSpawnDelay); }
        }
        
        // Determines if an enemy can spawn
        private bool CanEnemySpawn
        {
            get { return (Game1.ActiveTime >= NextSpawnTime); }
        }

        // Resets the spawn delay timer when the next level begins
        public void ResetSpawnDelayTimer()
        {
            PrevSpawnDelay = (Game1.ActiveTime + SpawnDelay);
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
            return (RandGenerator.Next(0, EnemySpawnChances.Count));
        }

        private Enemy FindEnemyToSpawn(int RandNum)
        {
            // Get the Enemy number to spawn
            int EnemyIndex = FindEnemyNumToSpawn(RandNum);

            // Get the Y position at which the enemy will spawn
            float Y = (RandGenerator.Next(Player.GateStart, Player.GateEnd)) + level.GetPlayer.GetPosition.Y;

            //Increase the speed the enemies move at based on level
            int minspeedinc = level.GetLevelNum / MinSpeedIncrease;
            int maxspeedinc = level.GetLevelNum / MaxSpeedIncrease;

            // Choose a random value between the minimum speed increase and the maximum speed increase, both inclusive
            int speedincrease = RandGenerator.Next(minspeedinc, maxspeedinc + 1);

            switch (EnemyIndex)
            {
                case 1: // Spear Enemy
                    return (new SpearEnemy(level, Y, speedincrease));
                case 2: //Armored enemy
                    return (new ArmoredEnemy(level, Y, speedincrease));
                case 3: //Flying enemy
                    int flyheight = RandGenerator.Next(FlyingEnemy.MinFlyingHeight, FlyingEnemy.MaxFlyingHeight + 1);
                    return (new FlyingEnemy(level, Y, flyheight, speedincrease));
                //case 4: //Armored Spear enemy
                //    return (new ArmoredSpearEnemy(level, Y, speedincrease));
                case 0: // Melee Enemy
                default:
                    return (new MeleeEnemy(level, Y, speedincrease));
            }
        }

        public void SpawnEnemy()
        {
            // Check if an enemy can be spawned
            if (CanEnemySpawn == true)
            {
                // Find an enemy to spawn
                Enemy EnemyToSpawn = FindEnemyToSpawn(RandGenerator.Next(1, 100));

                // Add the enemy that should be spawned
                level.AddEnemy(EnemyToSpawn);

                // NOTE: This calculation will need to be changed. It was just set up with these values for now
                // Set the minimum spawn time to depend on the level of the player
                // The minimum spawn time decreases by 48 milliseconds each level
                int MinSpawnTime = (3000 - (48 * (level.GetLevelNum - 1)));

                // Set the maximum spawn time to be 1.5x the minimum spawn time
                // Add 1 to include the maximum spawn time
                int MaxSpawnTime = (int)(MinSpawnTime * 1.5f) + 1;

                // Randomly generate the next spawn time for the enemy
                SpawnTime = RandGenerator.Next(MinSpawnTime, MaxSpawnTime);

                // Set the next time an enemy will be spawned
                NextSpawnTime = Game1.ActiveTime + SpawnTime;
            }
        }

        public void Update()
        {
            // Check if enemies can start spawning
            if (CanStartSpawning == true)
                // Try to spawn a new enemy
                SpawnEnemy();
        }


    }
}