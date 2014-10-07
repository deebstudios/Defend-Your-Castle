﻿using System;
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
        // Stores the reference to Game1
        private Game1 Game;

        //The constant that helps determine the rate that the fade changes and the fade that changes day to night
        //The NightFactor determines if a day starts out in day or vice versa
        private const float FadeRate = 220f;
        private const float CelestialDepth = .01f;
        private Fade NightFade;
        private int NightFactor;

        //Starting Y positions of the sun and moon, respectively
        private const float SunY = 50f;
        private const float MoonY = 305f;

        // Stores the level number of the level
        private int LevelNum;

        //The player; the player's children are the things that are helpful to the player
        //The player and its children persist between levels
        private Player player;

        // The time at which the level will end
        private float LevelEndTime;

        //The list of enemies and other things harmful to the player
        private List<LevelObject> Enemies;

        // Instance of the EnemySpawning class - used to spawn enemies
        private EnemySpawning EnemySpawn;

        // Stores the amount of Gold the player has when the level starts
        // Used to calculate the amount of Gold the player earned in the level
        private int StartingGold;

        // Stores the number of times the player has attacked by tapping/clicking. Used to calculate the player's accuracy rate
        private int NumAttacks;

        // Stores the number of enemies the player has killed by tapping/clicking. Also used to calculate the player's accuracy rate
        private int NumPlayerKills;

        // Stores the number of enemies the player's helpers have killed
        public int NumHelperKills;

        // Returns the number of total kills the player earned in the round
        private int NumTotalKills
        {
            get { return (NumPlayerKills + NumHelperKills); }
        }

        // Returns the player's accuracy rate, rounded to the nearest whole number
        private double PlayerAccuracyRate
        {
            get
            {
                // Return 0 if the player hasn't attacked
                if (NumAttacks == 0) return 0;

                // Divide the number of player kills by the number of attacks to get the accuracy rate
                return Math.Round((((double)NumPlayerKills / (double)NumAttacks) * 100d), 2);
            }
        }

        // Returns the amount of gold the player has earned in the level
        private int GoldEarned
        {
            get { return (player.Gold - StartingGold); }
        }

        public Level(Player play, Game1 game)
        {
            player = play;
            Game = game;

            Enemies = new List<LevelObject>();

            // Set the level number to 1 (for testing)
            LevelNum = 1;

            //Start out at day
            StartDayNight(true);
            CreateNightFade();

            // Set the starting gold to the player's gold amount
            StartingGold = player.Gold;

            // Initialize the EnemySpawning class
            EnemySpawn = new EnemySpawning(this);

            // Set the level end time
            LevelEndTime = Game1.ActiveTime + LevelDuration;
        }

        public List<LevelObject> GetEnemies
        {
            get { return Enemies; }
        }

        // The duration of the level (in milliseconds)
        private float LevelDuration
        {
            // NOTE: This calculation will need to be changed
            get { return (30000 + ((LevelNum - 1) * 1000)); }
        }

        //Creates the night fade based on how long the level lasts
        private void CreateNightFade()
        {
            //Get how long to wait each fade for the level
            float leveltime = LevelDuration / FadeRate;

            //Starts sky blue and goes down to a value closer to purple (darken the sky)
            //We can reverse it and make the level start at night by simply reversing the value of NightFactor
            NightFade = new FadeOnce(Color.LightSkyBlue, NightFactor, 0, (int)FadeRate, leveltime);
        }

        //Choose whether to start the level in the day or at night
        private void StartDayNight(bool day)
        {
            NightFactor = (day == true ? 1 : -1);
        }

        public void StartNextLevel()
        {
            // Increase the level number by 1
            LevelNum += 1;

            //Refresh the night fade
            CreateNightFade();

            // Check if a new enemy can be added to the spawn list, and add the enemy if so
            EnemySpawn.CheckAddSpawnEnemy();

            // Set the level end time
            LevelEndTime = Game1.ActiveTime + LevelDuration;

            // Reset the tracked information
            ResetTrackedInfo();

            // Update the player's HP and Gold on the UI
            player.UpdateHealth();
            player.UpdateGoldAmount();

            // Set the game state to InGame
            Game.ChangeGameState(GameState.InGame);
        }
        
        // Resets the information that is tracked during the level
        private void ResetTrackedInfo()
        {
            // Store the amount of gold the player has
            StartingGold = player.Gold;

            // Set the number of attacks and kills to 0
            NumAttacks = NumPlayerKills = NumHelperKills = 0;
        }

        public void CheckEndLevel()
        {
            // Check if the level should be ended
            if (Game1.ActiveTime >= LevelEndTime)
            {
                // End the level
                EndLevel();
            }
        }

        public void EndLevel()
        {
            // Clear the enemies list
            Enemies.Clear();

            // Stop the Player's Invincibility if it is active
            player.EndInvincibility();

            // Set the level complete text
            player.GetGamePage.LevelEnd_LevelCompleteText.Text = "Level " + LevelNum + " Complete";

            // Display the player's level stats
            player.GetGamePage.LevelEnd_Kills.Text = "Kills: " + NumPlayerKills;
            player.GetGamePage.LevelEnd_HelperKills.Text = "Helper Kills: " + NumHelperKills;
            player.GetGamePage.LevelEnd_TotalKills.Text = "Total Kills: " + NumTotalKills;
            player.GetGamePage.LevelEnd_AccuracyRate.Text = "Accuracy Rate: " + PlayerAccuracyRate + "%";
            player.GetGamePage.LevelEnd_GoldEarned.Text = "Gold Earned: " + GoldEarned;

            // Begin the animation to display the level stats
            player.GetGamePage.LevelEnd_Anim.Begin();
            
            // Set the game state to Shop
            Game.ChangeGameState(GameState.Shop);
        }

        //Returns the player reference
        public Player GetPlayer
        {
            get { return player; }
        }

        //Adds a player-helping object to the player
        public void AddPlayerHelper(PlayerHelper helper)
        {
            player.AddChild(helper);
            helper.SetPosition(this);
        }

        //Adds a player-harming object to the level; these objects always have no parents and are added here if removed from their parents
        public void AddEnemy(LevelObject enemy)
        {
            Enemies.Add(enemy);
        }

        // Return the level number
        public int GetLevelNum
        {
            get { return LevelNum; }
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
            // Increment the player's number of attacks by 1
            NumAttacks += 1;

            for (int i = 0; i < Enemies.Count; i++)
            {
                //Check for the object's weapon weakness
                if (Enemies[i].CanGetHit(rect) == true && player.CurrentWeapon.CanHit(Enemies[i].GetWeaponWeakness))
                {
                    Enemies[i].Die(this);

                    // Increment the player's kill count by 1
                    NumPlayerKills += 1;

                    break;
                }
            }
        }

        public void Update()
        {
            // Check if the level can be ended
            CheckEndLevel();

            //Update the night fade
            NightFade.Update();

            // Update the enemies
            UpdateEnemies();

            //Update the player
            player.Update(this);

            // Update the enemy spawn
            EnemySpawn.Update();
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
            Vector2 BGscale = new Vector2(Game1.ScreenSize.X / LoadAssets.LevelBG.Width, Game1.ScreenSize.Y / LoadAssets.LevelBG.Height);
            Color BGcolor = new Color(255 - NightFade.GetCurFade, 255 - NightFade.GetCurFade, 255 - NightFade.GetCurFade);

            //Draw the background
            spriteBatch.Draw(LoadAssets.LevelBG, Vector2.Zero, null, BGcolor, 0f, Vector2.Zero, BGscale, SpriteEffects.None, CelestialDepth + .0001f);
            spriteBatch.Draw(LoadAssets.ScalableBox, Vector2.Zero, null, NightFade.GetColorPlusFade(false), 0f, Vector2.Zero, new Vector2(Game1.ScreenSize.X, Game1.ScreenSize.Y), SpriteEffects.None, 0f);
            spriteBatch.Draw(LoadAssets.DaySun, new Vector2(Game1.ScreenHalf.X + 75, SunY + NightFade.GetCurFade), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, CelestialDepth);
            spriteBatch.Draw(LoadAssets.NightMoon, new Vector2(Game1.ScreenHalf.X + 77, (MoonY - (MoonY - FadeRate)) - NightFade.GetCurFade), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, CelestialDepth);

            DrawEnemies(spriteBatch);

            //Draw the player
            player.Draw(spriteBatch);
        }

        public void LoadLevelData(LevelData levelData)
        {
            // Loop until we reach the level the player should be at
            for (int i = 1; i < levelData.LevelNum; i++)
            {
                // Increment the level number by 1
                LevelNum += 1;

                // Try to add an enemy spawn. This will not work if the level number is set directly because of the switch statement
                EnemySpawn.CheckAddSpawnEnemy();
            }


        }


    }
}
