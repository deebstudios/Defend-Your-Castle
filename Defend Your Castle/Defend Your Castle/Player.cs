using System;
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
        public enum WeaponTypes
        {
            Sword, Warhammer
        }

        // Reference to GamePage.xaml
        private GamePage gamePage;

        //The Y boundary for attacking; anything above this boundary will be the HUD, and enemies don't appear where the HUD is
        private const int HUDYBounds = 75;

        //Sees whether the player has an invincibility powerup or not
        protected bool InvincibilityAvailable;

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

        //The player's weapons
        public Weapon[] Weapons;

        // The player's currently-selected weapon
        public int CurWeapon;

        // Stores the mouse state
        private MouseState mouseState;

        public Player(GamePage gamepage)
        {
            // Get the reference to GamePage.xaml
            gamePage = gamepage;

            // Set the player's default castle level
            CastleLevel = 1;

            ObjectSheet = LoadAssets.PlayerCastle;

            InvincibilityLength = 5000f;
            InvincibilityAvailable = false;
            InvincibilityFade = new Fade(Color.White, 10, 0, 255, Fade.InfiniteLoops, 0f);

            // Set the player's base health and maximum health
            Health = MaxHealth = 1500;

            // Start the player out with some gold
            Gold = 100;

            // Select the Sword weapon by default
            Weapons = new Weapon[] { new Sword(), new Warhammer() };

            CurWeapon = (int)WeaponTypes.Sword;

            // Set the animation of the player
            Animation = new Animation(new AnimFrame(new Rectangle(0, 0, ObjectSheet.Width, ObjectSheet.Height), 0f));

            // Set the position of the player
            Position = new Vector2(Game1.ScreenSize.X - Animation.CurrentAnimFrame.FrameSize.X, 70);

            // Update the UI with the new gold amount
            UpdateGoldAmount();

            // Initialize the mouse state
            mouseState = new MouseState();
        }

        public GamePage GetGamePage
        {
            get { return gamePage; }
        }

        public int GetHealth
        {
            get { return Health; }
        }

        public int GetMaxHealth
        {
            get { return MaxHealth; }
        }

        public int GetCastleLevel
        {
            get { return CastleLevel; }
        }

        public bool HasInvincibility
        {
            get { return InvincibilityAvailable; }
        }

        //The weapon the player has
        public Weapon CurrentWeapon
        {
            get { return Weapons[CurWeapon]; }
        }

        // Determines if the player can upgrade his castle. Should be used in the Shop to grey out the Upgrade icon
        public bool CanUpgradeCastle
        {
            get { return (CastleLevel != MaxCastleLevel); }
        }

        // Determines if the player can attack
        // This is not included in the Weapon class because the player would be able to reset the attack timer by switching weapons
        public bool CanAttack
        {
            get { return CurrentWeapon.CanAttack; }
        }

        //Switch the Player's weapon
        public void SwitchWeapon(int newweapon)
        {
            if (newweapon >= 0 && newweapon < Weapons.Length)
            {
                if (Weapons[newweapon].CanUse == true)
                    CurWeapon = newweapon;
            }
        }

        //Set goldamount to a negative value when subtracting gold
        public void ReceiveGold(int goldamount)
        {
            Gold += goldamount;

            // Update the UI with the new gold amount
            UpdateGoldAmount();
        }

        public void UpdateGoldAmount()
        {
            // Set the Gold Amount TextBlock's Text to the amount of gold the player has
            gamePage.HUD_GoldAmount.Text = Gold.ToString();
        }

        public void UpdateGoldAmountInShop()
        {
            // Set the Gold Amount TextBlock's Text to the amount of gold the player has
            gamePage.Shop_GoldAmount.Text = Gold.ToString();
        }

        public void UpdateHealth()
        {
            // Set the Width of the inner HP bar to reflect the player's remaining HP
            gamePage.HUD_InnerHPBar.Width = (((double)Health / (double)MaxHealth) * gamePage.HUD_InnerHPBarWidth);
        }

        public void UpdateHealthInShop()
        {
            // Set the Width of the inner HP bar to reflect the player's remaining HP
            gamePage.Shop_InnerHPBar.Width = (((double)Health / (double)MaxHealth) * gamePage.Shop_InnerHPBarWidth);
            
            // Update the player's HP value
            gamePage.Shop_HPText.Text = Health + " / " + MaxHealth;
        }

        // Heals the player
        public void Heal(int healAmount)
        {
            // Heal the player by the specified amount
            Health += healAmount;

            // Make sure the player doesn't heal over his max health
            if (Health > MaxHealth) Health = MaxHealth;

            // Update the UI with the player's health
            UpdateHealth();
        }

        //Makes the player lose health when being attacked
        public void TakeDamage(int damage, Level level)
        {
            //Check if the player is invincible
            if (IsInvincible == false)
            {
                //Subtract an amount of damage
                Health -= damage;

                SoundManager.PlaySound(LoadAssets.TestSound);

                //Don't show negative health
                if (Health < 0)
                {
                    Health = 0;

                    //The death sequence would be in the overloaded Die() method
                    Die(level);
                }

                // Update the UI with the player's health
                UpdateHealth();
            }
        }

        public void IncreaseMaxHealth(int healthIncrease)
        {
            //Increase the player's current and max HP by the designated amount
            Health += healthIncrease;
            MaxHealth += healthIncrease;
        }

        public void UpgradeCastle(int healthIncrease)
        {
            // Make sure the player can upgrade his castle
            if (CanUpgradeCastle)
            {
                // Increment the player's castle level by 1
                CastleLevel += 1;

                IncreaseMaxHealth(healthIncrease);
                
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

        //Uses the player's invincibility power-up
        public override void UseInvincibility()
        {
            base.UseInvincibility();
            InvincibilityAvailable = false;
        }

        public void Attack(Level level, GestureSample? gesture)
        {
            // Check to make sure the player can attack
            if (CanAttack == true)
            {
                //Make sure the attack is below the HUD boundary
                Rectangle touchrect = Input.GestureRect(gesture);

                if (touchrect.Y > HUDYBounds)
                {
                    // Play the weapon's attack sound
                    CurrentWeapon.Attack();

                    level.EnemyHit(touchrect);
                }
            }
        }

        public void LoadPlayerData(PlayerData playerData)
        {
            // Set the player's properties to the stored data properties
            Health = playerData.Health;
            MaxHealth = playerData.MaxHealth;
            CastleLevel = playerData.CastleLevel;
            Gold = playerData.Gold;

            InvincibilityAvailable = playerData.Invincibility;

            //Use the helper data to add the helpers to the player
            for (int i = 0; i < playerData.Helpers.Count; i++)
            {
                AddChild(playerData.Helpers[i].CreateHelper());
            }
        }

        public void Attack(Level level)
        {
            // Check to make sure the player can attack
            if (CanAttack)
            {
                //Make sure the attack is below the HUD boundary
                Rectangle clickrect = Input.MouseRect(mouseState);

                if (clickrect.Y > HUDYBounds)
                {
                    // Play the weapon's attack sound
                    CurrentWeapon.Attack();

                    level.EnemyHit(clickrect);
                }
            }
        }

        public override void Update(Level level)
        {
            // Get the last touch gesture (if any)
            GestureSample? gesture = Input.GetTouchGesture();

            if (Input.IsLeftMouseButtonDown(mouseState))
            {
                Attack(level);
            }
            else if (Input.IsScreenTapped(gesture))
            {
                Attack(level, gesture);
            }

            //If the player is invincible, update the color effect
            if (IsInvincible == true) InvincibilityFade.Update();

            //TEMPORARY, test granting invincibility
            if (Input.IsRightMouseButtonDown(mouseState) == true)
                UseInvincibility();

            // Get the mouse state
            mouseState = Mouse.GetState();

            base.Update(level);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            float depth = GetDrawDepth;

            //If the player is invincible, draw the invincible fort above on top of the normal one
            if (IsInvincible == true)
            {
                spriteBatch.Draw(LoadAssets.PlayerCastleInvincible, Position, null, InvincibilityFade.GetFadeColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, depth + .0001f);
            }

            Animation.Draw(spriteBatch, ObjectSheet, Position, Direction.Right, Color.White, 0f, depth);

            base.Draw(spriteBatch);
        }


    }
}