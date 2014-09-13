using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_Your_Castle
{
    public class Player : LevelObject
    {
        // The amount of health of the player
        private int Health;

        // The max amount of health of the player
        private int MaxHealth;

        // Stores how much the player's castle's max health will increase for each level
        private int HealthIncrease;

        // Stores the player's castle's max health for each castle level
        private int[] HealthLevels;

        // The level of the player's castle
        private int CastleLevel;

        // The max level of the player's castle
        private int MaxCastleLevel;

        // Determines if the player can upgrade his castle. Should be used in the Shop to grey out the Upgrade icon
        public bool CanUpgradeCastle
        {
            get { return (CastleLevel != MaxCastleLevel); }
        }

        public Player(Animation animation)
        {
            // Set the player's default castle level
            CastleLevel = 1;

            // Set the max castle level
            MaxCastleLevel = 3;

            // Set the health increase per level
            HealthIncrease = 1500;

            // Initialize the HealthLevels array
            HealthLevels = new int[MaxCastleLevel];

            // Set the values of each health level
            for (int i = 0; i < MaxCastleLevel; i++)
            {
                HealthLevels[i] = (i * HealthIncrease);
            }

            // Set the player's health and maximum health
            Health = MaxHealth = HealthLevels[(CastleLevel - 1)];

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

        public void UpgradeCastle()
        {
            // Make sure the player can upgrade his castle
            if (CanUpgradeCastle)
            {
                // Increment the player's castle level by 1
                CastleLevel += 1;
                
                // Upgrade the player's max HP
                MaxHealth = HealthLevels[(CastleLevel - 1)];

                // Heal the player by the health increase amount
                Health += HealthIncrease;
                
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


    }
}