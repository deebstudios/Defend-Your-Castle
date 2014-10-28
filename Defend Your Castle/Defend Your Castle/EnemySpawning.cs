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
        private const int MinSpeedIncrease = 20;
        private const int MaxSpeedIncrease = 9;
        private const int MaxSpeedBonus = 6;

        //The starting level, number of levels the max number of enemies that can spawn increases, and the max number of enemies that can spawn at once
        private const int StartMoreSpawn = 4;
        private const int MaxNumIncrease = 8;
        public const int MaxNumSpawn = 3;

        //The first level enemies are able to spawn with invincibility and the maximum invincibility duration
        private const int FirstInvLevel = 15;
        private const float MaxInvDuration = 1900f;
        private const float InvDecrease = 300f;

        //The chance for an enemy to be invincible
        private const int InvChance = 10;

        public const int StartNewEnem = 3;
        public const int NextNewEnem = 5;

        // Reference to the level
        private Level level;

        // Stores the spawn chance for each enemy in the spawn list. The sum of all items in the list should be 100
        private List<int> EnemySpawnChances;
        private int ListIndex;

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
            EnemySpawnChances = new List<int>() { 25, 20, 20, 15, 20 };
            ListIndex = 1;
        }

        // Determines if any enemies can start spawning for the level
        public bool CanStartSpawning
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

        private void RefreshSpawnTime()
        {
            // Set the minimum spawn time to depend on the level the player is on
            // The minimum spawn time decreases by 48 milliseconds each level
            int MinSpawnTime = (2750 - (48 * (level.GetLevelNum - 1)));

            // Set the maximum spawn time to be 1.2x the minimum spawn time
            // Add 1 to include the maximum spawn time
            int MaxSpawnTime = (int)(MinSpawnTime * 1.2f) + 1;

            // Randomly generate the next spawn time for the enemy
            SpawnTime = RandGenerator.Next(MinSpawnTime, MaxSpawnTime);

            // Set the next time an enemy will be spawned
            NextSpawnTime = Game1.ActiveTime + SpawnTime;
        }

        //Increases the index of the EnemySpawnChances list to include more enemies
        public void AddNewSpawnEnemy(int numnewenemies)
        {
            //Make sure we don't go out of bounds when adding. When loading in data, we'd pass a value greater than 1 here
            ListIndex += numnewenemies;

            if (ListIndex > EnemySpawnChances.Count) ListIndex = EnemySpawnChances.Count;
        }

        public void CheckAddSpawnEnemy()
        {
            switch (level.GetLevelNum)
            {
                case 3:
                case 8:
                case 13:
                case 18:
                    // Add a new enemy to the enemy spawn list
                    AddNewSpawnEnemy(1);
                    break;
                default:
                    break;
            }
        }

        /*private void AddSpawnEnemy(int NewEnemySpawnChance)
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
        }*/

        private int GetSpawnChancesSum()
        {
            // Stores the sum of the enemy spawn chances
            int Sum = 0;

            // Loop through all of the enemy spawn chances
            for (int i = 0; i < ListIndex; i++)
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
            for (int i = 0; i < ListIndex; i++)
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
            return (RandGenerator.Next(0, ListIndex));
        }

        private Enemy FindEnemyToSpawn(int RandNum, int index)
        {
            // Get the Enemy number to spawn
            int EnemyIndex = FindEnemyNumToSpawn(RandNum);

            //If more than one enemy spawns at a time, move it to the left to avoid overlap. The amount depends on the index
            float XMove = -(index * 17);

            // Get the Y position at which the enemy will spawn
            float Y = (RandGenerator.Next(Player.GateStart, Player.GateEnd)) + level.GetPlayer.GetPosition.Y;

            //Increase the speed the enemies move at based on level
            int minspeedinc = level.GetLevelNum / MinSpeedIncrease;
            int maxspeedinc = level.GetLevelNum / MaxSpeedIncrease;

            //Cap the speeds
            if (minspeedinc > MaxSpeedBonus) minspeedinc = MaxSpeedBonus;
            if (maxspeedinc > MaxSpeedBonus) maxspeedinc = MaxSpeedBonus;

            float randdecimal = (float)Math.Round(RandGenerator.NextDouble(), 2);

            // Choose a random value between the minimum speed increase and the maximum speed increase, both inclusive
            float speedincrease = randdecimal + RandGenerator.Next(minspeedinc, maxspeedinc + 1);

            //Get a random costume (recolor) for the enemy to be
            int costume = RandGenerator.Next(0, 3);

            Enemy enem = null;

            switch (EnemyIndex)
            {
                case 1: // Spear Enemy
                    enem = new SpearEnemy(level, XMove, Y, speedincrease, costume);
                    break;
                case 2: //Armored enemy
                    enem = new ArmoredEnemy(level, XMove, Y, speedincrease, costume);
                    break;
                case 3: //Armored Spear enemy
                    enem = new ArmoredSpearEnemy(level, XMove, Y, speedincrease, costume);
                    break;
                case 4: //Flying enemy
                    int flyheight = RandGenerator.Next(FlyingEnemy.MinFlyingHeight, FlyingEnemy.MaxFlyingHeight + 1);
                    enem = new FlyingEnemy(level, XMove, Y, flyheight, speedincrease, costume);
                    break;
                case 0: // Melee Enemy
                default:
                    enem = new MeleeEnemy(level, XMove, Y, speedincrease, costume);
                    break;
            }

            //Check the level to see if the enemy can spawn with invincibility
            if (level.GetLevelNum >= FirstInvLevel)
            {
                //There's a chance of having the enemy be invincible
                int invchance = RandGenerator.Next(0, InvChance);

                if (invchance == 0)
                {
                    //Scale invincibility length with bonus speed. At 0 bonus speed, it's MaxInvDuration
                    float invduration = MaxInvDuration - (speedincrease * InvDecrease);

                    ///*We want the invincibility value to be at around 200f on the last level due to how fast enemies move
                    //  It should decrease a little every few levels*/
                    //
                    ////Decrease by 100 ms every level
                    //int leveldiff = (level.GetLevelNum - FirstInvLevel) * 50;
                    //float invduration = MaxInvDuration - leveldiff;
                    //
                    ////If the enemy is ranged, subtract 100 ms from the duration since they don't have to travel as far
                    //if (EnemyIndex == 0 || EnemyIndex == 4) invduration -= 100f;

                    enem.SetInvincible(invduration);
                }
            }

            return enem;
        }

        public void SpawnEnemy()
        {
            // Check if an enemy can be spawned
            if (CanEnemySpawn == true)
            {
                //Check how many enemies to spawn; 1 minimum
                int numenemies = 1;
                if (level.GetLevelNum >= StartMoreSpawn)
                {
                    //Add more enemies
                    int addenem = ((level.GetLevelNum - StartMoreSpawn) / MaxNumIncrease) + 1;
                    if (addenem > MaxNumSpawn) addenem = MaxNumSpawn;

                    numenemies += RandGenerator.Next(0, addenem + 1);
                }

                for (int i = 0; i < numenemies; i++)
                {
                    // Find an enemy to spawn
                    Enemy EnemyToSpawn = FindEnemyToSpawn(RandGenerator.Next(0, GetSpawnChancesSum()), i);

                    // Add the enemy that should be spawned
                    level.AddEnemy(EnemyToSpawn);
                }

                RefreshSpawnTime();
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