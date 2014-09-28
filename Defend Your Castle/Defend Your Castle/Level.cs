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
        // Stores the reference to Game1
        private Game1 Game;

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

        public Level(Player play, Game1 game)
        {
            player = play;
            Game = game;

            Enemies = new List<LevelObject>();

            // Set the level number to 1 (for testing)
            LevelNum = 1;

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

        public void StartNextLevel()
        {
            // Increase the level number by 1
            LevelNum += 1;

            // Check if a new enemy can be added to the spawn list, and add the enemy if so
            EnemySpawn.CheckAddSpawnEnemy();

            // Set the level end time
            LevelEndTime = Game1.ActiveTime + LevelDuration;

            // Set the game state to InGame
            Game.ChangeGameState(GameState.InGame);

            // Update the player's HP and Gold on the UI
            player.UpdateHealth();
            player.UpdateGoldAmount();
        }

        public void EndLevel()
        {
            // Clear the enemies list
            Enemies.Clear();

            //Stop the Player's Invincibility if it is active
            player.StopInvincibility();

            // Set the level complete text
            player.GetGamePage.LevelEnd_LevelCompleteText.Text = "Level " + LevelNum + " Complete";

            // Set the game state to Shop
            Game.ChangeGameState(GameState.Shop);
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

        //Returns the player reference
        public Player GetPlayer
        {
            get { return player; }
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
            for (int i = 0; i < Enemies.Count; i++)
            {
                //Check for the object's weapon weakness
                if (Enemies[i].CanGetHit(rect) == true && player.CurWeapon >= Enemies[i].GetWeaponWeakness)
                {
                    Enemies[i].Die(this);
                    break;
                }
            }
        }

        public void Update()
        {
            // Check if the level can be ended
            CheckEndLevel();

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
            DrawEnemies(spriteBatch);

            //Draw the player
            player.Draw(spriteBatch);
        }


    }
}
