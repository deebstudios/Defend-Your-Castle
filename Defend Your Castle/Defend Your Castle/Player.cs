using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Defend_Your_Castle
{
    public class Player : LevelObject
    {
        // The amount of health of the player
        private int Health;

        // The max amount of health of the player
        private int MaxHealth;

        // The level of the player's castle
        private int CastleLevel;

        // The max level of the player's castle
        private const int MaxCastleLevel = 3;

        // The amount of gold the player has
        public int Gold;

        // Determines if the player can upgrade his castle. Should be used in the Shop to grey out the Upgrade icon
        public bool CanUpgradeCastle
        {
            get { return (CastleLevel != MaxCastleLevel); }
        }

        public Player(Animation animation)
        {   
            // Set the player's default castle level
            CastleLevel = 1;

            // Set the player's base health and maximum health
            Health = MaxHealth = 1500;

            // Start the player out with some gold
            Gold = 100;

            // Set the animation of the player
            Animation = animation;
        }
        
        // Heals the player
        public void Heal(int healAmount)
        {
            // Heal the player by the specified amount
            Health += healAmount;

            // Make sure the player doesn't heal over his max health
            if (Health > MaxHealth) Health = MaxHealth;
        }

        public void UpgradeCastle(int healthIncrease)
        {
            // Make sure the player can upgrade his castle
            if (CanUpgradeCastle)
            {
                // Increment the player's castle level by 1
                CastleLevel += 1;
                
                // Increase the player's max HP and HP
                Health += healthIncrease;
                MaxHealth += healthIncrease;

                // Change the castle animation
                // Instead of a switch, may be able to store an array of castle animations. This is probably
                // wasted memory though
                //switch (CastleLevel)
                //{
                //    case 1:
                //    case 2:
                //    case 3:

                //}
            }
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {


            base.Draw(spriteBatch);
        }


    }
}