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
            Sword, Spear, Warhammer
        }

        // Reference to GamePage.xaml
        private GamePage gamePage;

        //The Y boundary for attacking; anything above this boundary will be the HUD, and enemies don't appear where the HUD is
        private const int HUDTopBounds = 125;
        //private const int HUDBottomBounds = 375;

        //The start and end of the castle entrance, respectively
        //They must be added to the player's Y position to get the true positions
        public const int GateStart = 197;
        public const int GateEnd = 225;

        //Sees whether the player has an invincibility powerup or not
        protected bool InvincibilityAvailable;

        //Checks whether the player's castle is fortified and takes reduced damage or not
        protected bool Fortified;
        protected float PercentDamage;

        // The amount of health of the player
        private int Health;

        // The max amount of health of the player
        private int MaxHealth;

        // The level of the player's castle
        private int CastleLevel;

        // The max level of the player's castle
        //private const int MaxCastleLevel = 3;

        // The amount of gold the player has
        public int Gold;

        //The player's weapons
        public Weapon[] Weapons;

        // The player's currently-selected weapon
        public int CurWeapon;

        // Stores the mouse state
        private MouseState mouseState;

        //The keyboard state
        private KeyboardState keyboardState;

        public Player(GamePage gamepage)
        {
            // Get the reference to GamePage.xaml
            gamePage = gamepage;

            // Set the player's default castle level
            CastleLevel = 1;

            ObjectSheet = LoadAssets.PlayerCastle;
            InvincibleSheet = LoadAssets.PlayerCastleInvincible;

            InvincibilityLength = 5000f;
            InvincibilityAvailable = false;
            InvincibilityFade = new Fade(Color.White, 10, 0, 255, Fade.InfiniteLoops, 0f);

            Fortified = false;
            PercentDamage = 1f;

            // Set the player's base health and maximum health
            Health = MaxHealth = 1000;

            // Start the player out with some gold
            Gold = 100;

            // Select the Sword weapon by default
            Weapons = new Weapon[] { new Sword(), new Spear(), new Warhammer() };

            CurWeapon = (int)WeaponTypes.Sword;

            // Set the animation of the player
            Animation = new Animation(new AnimFrame(new Rectangle(0, 0, ObjectSheet.Width, ObjectSheet.Height), 0f));

            // Set the position of the player
            Position = new Vector2(Game1.ScreenSize.X - Animation.CurrentAnimFrame.FrameSize.X, 100);
            
            // Update the UI with the health amount
            UpdateHealth();

            // Update the UI with the new gold amount
            UpdateGoldAmount();

            // Initialize the mouse state
            mouseState = new MouseState();

            //Initialize keyboard state
            keyboardState = new KeyboardState(Keys.Q, Keys.W, Keys.E);
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
        //public bool CanUpgradeCastle
        //{
        //    get { return (CastleLevel != MaxCastleLevel); }
        //}

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

        //Resets player helpers
        public void ResetHelpers()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                PlayerHelper helper = Children[i] as PlayerHelper;

                //Make the helper stop attacking
                if (helper != null)
                {
                    helper.StopAttacking();
                }
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
            UpdateHealthInShop();
        }

        //Calculates the total damage dealt to the player
        private int CalculateDamage(int damage)
        {
            //Keep a float for accuracy
            float newdamage = damage * PercentDamage;

            return (int)newdamage;
        }

        //Makes the player lose health when being attacked
        public void TakeDamage(int damage, Level level)
        {
            //Check if the player is invincible
            if (IsInvincible == false)
            {
                //Subtract an amount of damage
                Health -= CalculateDamage(damage);

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

        public override void Die(Level level)
        {
            // Call the base Die method
            base.Die(level);

            // Show the Game Over screen
            level.Game.screenManager.ChangeScreen(ScreenManager.Screens.GameOverScreen);

            // Change the game mode to Screen
            level.Game.ChangeGameState(GameState.Screen);

            //Play the game over song
            SoundManager.PlaySong(LoadAssets.GameOver, false);
        }

        public void IncreaseMaxHealth(int healthIncrease)
        {
            //Increase the player's current and max HP by the designated amount
            MaxHealth += healthIncrease;
            Heal(healthIncrease);
        }

        //Fortify the player's castle
        public void StrengthenCastle(int numtimes)
        {
            //0 is passed in when loading if the player didn't upgrade at all
            if (numtimes > 0)
            {
                float amount = StrengthenWalls.DamageReduction * numtimes;

                //Change the castle graphic
                ObjectSheet = LoadAssets.PlayerCastleFortified;

                PercentDamage -= amount;
                PercentDamage = (float)Math.Round(PercentDamage, 2);

                if (PercentDamage < StrengthenWalls.MaxDamageReduction) PercentDamage = StrengthenWalls.MaxDamageReduction;
            }
        }

        public void UpgradeCastle(int healthIncrease)
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

        //Uses the player's invincibility power-up
        public override void UseInvincibility()
        {
            base.UseInvincibility();
            InvincibilityAvailable = false;
        }

        //public void Attack(Level level, GestureSample? gesture)
        //{
        //    // Check to make sure the player can attack
        //    if (CanAttack == true)
        //    {
        //        //Make sure the attack is below the HUD boundary
        //        Rectangle touchrect = Input.GestureRect(gesture);

        //        if (touchrect.Y > HUDTopBounds && touchrect.Y < HUDBottomBounds)
        //        {
        //            // Play the weapon's attack sound
        //            CurrentWeapon.Attack();

        //            level.EnemyHit(touchrect);
        //        }
        //    }
        //}

        public void Attack(Level level, TouchLocation? touchLoc)
        {
            // Check to make sure the player can attack
            if (CanAttack == true && level.DidEnemiesSpawn == true)
            {
                //Make sure the attack is below the HUD boundary
                Rectangle touchrect = Input.GetTouchRect(touchLoc); //Input.GestureRect(gesture);

                if (touchrect.Y > HUDTopBounds)
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

            //Use the helper data to add the helpers to the player
            for (int i = 0; i < playerData.Helpers.Count; i++)
            {
                PlayerHelper helper = playerData.Helpers[i].CreateHelper();

                AddChild(helper);
                helper.SetPosition();
            }
        }

        public void Attack(Level level)
        {
            // Check to make sure the player can attack
            if (CanAttack == true && level.DidEnemiesSpawn == true)
            {
                //Make sure the attack is below the HUD boundary
                Rectangle clickrect = Input.MouseRect(mouseState);

                if (clickrect.Y > HUDTopBounds)
                {
                    // Play the weapon's attack sound
                    CurrentWeapon.Attack();

                    level.EnemyHit(clickrect);
                }
            }
        }

        public void AddHelperKill(Level level)
        {
            // Increment the number of helper kills in the level by 1
            level.NumHelperKills += 1;
        }

        public override void Update(Level level)
        {
            // Get the last touch gesture (if any)
            TouchLocation? touchLoc = Input.GetTouchLocation();
            //GestureSample? gesture = Input.GetTouchGesture();

            //Check for switching weapons via keyboard input
            if (Input.IsKeyDown(keyboardState, Keys.Q) == true)
            {
                SwitchWeapon(0);
                gamePage.HUD_WeaponSword.IsChecked = true;
            }
            else if (Input.IsKeyDown(keyboardState, Keys.W) == true)
            {
                SwitchWeapon(2);
                gamePage.HUD_WeaponWarhammer.IsChecked = true;
            }
            else if (Input.IsKeyDown(keyboardState, Keys.E) == true)
            {
                SwitchWeapon(1);
                gamePage.HUD_WeaponSpear.IsChecked = true;
            }

            //Check for hurting enemies
            if (Input.IsLeftMouseButtonDown(mouseState))
            {
                Attack(level);
            }
            else if (Input.IsScreenTapped(touchLoc) == true)// Input.IsScreenTapped(gesture))
            {
                Attack(level, touchLoc);
                //Attack(level, gesture);
            }

            //If the player is invincible, update the color effect
            if (IsInvincible == true) InvincibilityFade.Update();

            // Get the mouse state
            mouseState = Mouse.GetState();

            //Update the keyboard state
            keyboardState = Keyboard.GetState();

            base.Update(level);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            float depth = GetDrawDepth;

            //If the player is invincible, draw the invincible fort above on top of the normal one
            if (IsInvincible == true)
            {
                Animation.Draw(spriteBatch, InvincibleSheet, Position, Direction.Right, InvincibilityFade.GetFadeColor, 0f, depth + .0001f);
                //spriteBatch.Draw(InvincibleSheet, Position, null, InvincibilityFade.GetFadeColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, depth + .0001f);
            }

            Animation.Draw(spriteBatch, ObjectSheet, Position, Direction.Right, Color.White, 0f, depth);

            base.Draw(spriteBatch);
        }


    }
}