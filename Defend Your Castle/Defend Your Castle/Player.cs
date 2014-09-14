﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

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

        // The player's currently-selected weapon
        public Weapon Weapon;

        // Stores the last time the player attacked
        public float PrevAttackTimer;

        // Stores the mouse state
        private MouseState mouseState;
        
        // Determines if the player can upgrade his castle. Should be used in the Shop to grey out the Upgrade icon
        public bool CanUpgradeCastle
        {
            get { return (CastleLevel != MaxCastleLevel); }
        }

        // Determines if the player can attack
        // This is not included in the Weapon class because the player would be able to reset the attack timer by switching weapons
        public bool CanAttack
        {
            get { return ((Game1.ActiveTime - PrevAttackTimer) >= Weapon.AttackSpeed); }
        }

        public Player(Animation animation)
        {   
            // Set the player's default castle level
            CastleLevel = 1;

            // Set the player's base health and maximum health
            Health = MaxHealth = 1500;

            // Start the player out with some gold
            Gold = 100;

            // Select the Sword weapon by default
            Weapon = new Sword();

            // Set the animation of the player
            Animation = animation;

            // Initialize the mouse state
            mouseState = new MouseState();
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

        public void Attack(GestureSample? gesture)
        {
            // Check to make sure the player can attack
            if (CanAttack)
            {
                // Play the weapon's attack sound
                SoundManager.PlaySound(Weapon.Sound);

                if (Input.IsTapInRect(Game1.TestEnemy.GetHurtbox.GetRect, gesture))
                {
                    // Perform the attack
                    Game1.TestEnemy.Die();
                }

                // Update the attack timer
                PrevAttackTimer = Game1.ActiveTime;
            }
        }

        public void Attack(MouseState mouseState)
        {
            // Check to make sure the player can attack
            if (CanAttack)
            {
                // Play the weapon's attack sound
                SoundManager.PlaySound(Weapon.Sound);

                if (Input.IsMouseInRect(Game1.TestEnemy.GetHurtbox.GetRect, mouseState))
                {
                    // Perform the attack
                    Game1.TestEnemy.Die();
                }

                // Update the attack timer
                PrevAttackTimer = Game1.ActiveTime;
            }
        }

        public override void Update()
        {
            // Get the last touch gesture (if any)
            GestureSample? gesture = Input.GetTouchGesture();

            if (Input.IsLeftMouseButtonDown(mouseState))
            {
                Attack(mouseState);
            }
            else if (Input.IsScreenTapped(gesture))
            {
                Attack(gesture);
            }

            // Get the mouse state
            mouseState = Mouse.GetState();

            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {


            base.Draw(spriteBatch);
        }


    }
}