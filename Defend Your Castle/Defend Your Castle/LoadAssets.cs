using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Defend_Your_Castle
{
    //A class for loading all of our assets
    public static class LoadAssets
    {
        private const string GraphicsDir = "Graphics/";
        private const string SoundDir = "Sounds/";
        private const string MusicDir = "Music/";

        //Level graphics
        public static Texture2D LevelBG;
        public static Texture2D DaySun;
        public static Texture2D NightMoon;

        public static Texture2D Sword;
        public static Texture2D Spear;
        public static Texture2D Warhammer;
        public static Texture2D PlayerCastle;
        public static Texture2D PlayerCastleInvincible;
        public static Texture2D PlayerArcher;

        //Enemy graphics
        public static Texture2D GoldCoinEffect;
        public static Texture2D EnemySpear;
        public static Texture2D GoblinSheet;
        public static Texture2D GoblinInvincibleSheet;
        public static Texture2D SpearGoblinSheet;
        public static Texture2D SpearGoblinInvincibleSheet;
        public static Texture2D FlyingGoblinSheet;
        public static Texture2D FlyingGoblinInvincibleSheet;
        public static Texture2D ArmoredGoblinSheet;
        public static Texture2D ArmoredGoblinInvincibleSheet;

        //Shop graphics
        public static Texture2D GoldCoin;
        public static Texture2D ShopIcon;
        public static Texture2D FortifyWalls;
        public static Texture2D Invincibility;
        public static Texture2D RepairWalls;
        public static Texture2D RepairWallsx10;

        //Fonts
        public static SpriteFont bmpFont;

        //Debug graphics
        public static Texture2D ScalableBox;
        public static Texture2D testanim;

        public static Song TestSong;
        public static SoundEffect TestSound;

        public static void LoadContent(ContentManager Content)
        {
            LoadGraphics(Content);
            LoadSounds(Content);
            LoadMusic(Content);

            bmpFont = Content.Load<SpriteFont>("Fonts/SOR2 Bitmap Font");
        }

        private static Texture2D LoadGraphic(ContentManager Content, string filename)
        {
            return Content.Load<Texture2D>(GraphicsDir + filename);
        }

        private static void LoadGraphics(ContentManager Content)
        {
            //Level graphics
            DaySun = LoadGraphic(Content, "Sun");
            NightMoon = LoadGraphic(Content, "Moon");

            Sword = Content.Load<Texture2D>(GraphicsDir + "Alpha Sword");
            Spear = Content.Load<Texture2D>(GraphicsDir + "Spear");
            Warhammer = Content.Load<Texture2D>(GraphicsDir + "Alpha Warhammer");
            PlayerCastle = Content.Load<Texture2D>(GraphicsDir + "PlayerCastle");
            PlayerCastleInvincible = LoadGraphic(Content, "PlayerCastleInvincible");
            PlayerArcher = LoadGraphic(Content, "ArcherSheet");

            //Enemy graphics
            GoldCoinEffect = LoadGraphic(Content, "Gold Coin Effect");
            EnemySpear = LoadGraphic(Content, "Enemy Spear");
            GoblinSheet = LoadGraphic(Content, "Goblin Sheet");
            GoblinInvincibleSheet = LoadGraphic(Content, "GoblinInvincible Sheet");
            SpearGoblinSheet = LoadGraphic(Content, "Spear Goblin Sheet");
            SpearGoblinInvincibleSheet = LoadGraphic(Content, "Spear Goblin Sheet Invincible");
            FlyingGoblinSheet = LoadGraphic(Content, "Flying Goblin Sheet");
            FlyingGoblinInvincibleSheet = LoadGraphic(Content, "Flying Goblin Sheet Invincible");
            ArmoredGoblinSheet = LoadGraphic(Content, "Armored Goblin Sheet");
            ArmoredGoblinInvincibleSheet = LoadGraphic(Content, "Armored Goblin Sheet Invincible");

            //Shop graphics
            GoldCoin = LoadGraphic(Content, "Gold Coin");
            ShopIcon = Content.Load<Texture2D>(GraphicsDir + "ShopIcons/ShopIcon");
            FortifyWalls = Content.Load<Texture2D>(GraphicsDir + "ShopIcons/FortifyWallsIcon");
            Invincibility = Content.Load<Texture2D>(GraphicsDir + "ShopIcons/InvincibilityIcon2");
            RepairWalls = Content.Load<Texture2D>(GraphicsDir + "ShopIcons/RepairWallsIcon");
            RepairWallsx10 = Content.Load<Texture2D>(GraphicsDir + "ShopIcons/RepairWallsx10Icon");

            //Debug graphics
            ScalableBox = Content.Load<Texture2D>(GraphicsDir + "ScalableBox");
            testanim = Content.Load<Texture2D>(GraphicsDir + "TestAnim");
        }

        private static void LoadSounds(ContentManager Content)
        {
            TestSound = Content.Load<SoundEffect>(SoundDir + "test");
        }

        private static void LoadMusic(ContentManager Content)
        {
            TestSong = Content.Load<Song>(MusicDir + "Mario Party - Peaceful Mushroom Village");
        }


    }
}