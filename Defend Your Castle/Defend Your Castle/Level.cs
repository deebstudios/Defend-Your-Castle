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
        //The player; the player's children are the things that are helpful to the player
        //The player and its children persist between levels
        private Player player;

        //The list of enemies and other things harmful to the player
        private List<LevelObject> Enemies;

        public Level(Player play)
        {
            player = play;

            Enemies = new List<LevelObject>();
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
                if (Enemies[i].CanGetHit(rect) == true)
                {
                    Enemies[i].Die(this);
                    break;
                }
            }
        }

        public void Update()
        {
            UpdateEnemies();

            //Update the player
            player.Update(this);
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
