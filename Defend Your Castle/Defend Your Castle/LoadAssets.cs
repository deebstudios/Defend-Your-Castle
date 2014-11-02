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
        public static Texture2D PlayerCastleFortified;
        public static Texture2D PlayerCastleInvincible;
        public static Texture2D[] PlayerArcher;
        public static Texture2D[] PlayerSlower;

        //Enemy graphics
        public static Texture2D GoldCoinEffect;
        public static Texture2D EnemySpear;
        public static Texture2D[] GoblinSheet;
        public static Texture2D GoblinInvincibleSheet;
        public static Texture2D[] SpearGoblinSheet;
        public static Texture2D SpearGoblinInvincibleSheet;
        public static Texture2D[] FlyingGoblinSheet;
        public static Texture2D FlyingGoblinInvincibleSheet;
        public static Texture2D[] ArmoredGoblinSheet;
        public static Texture2D ArmoredGoblinInvincibleSheet;
        public static Texture2D[] RangedArmoredGoblin;
        public static Texture2D RangedArmoredGoblinInvincible;

        //Shop graphics
        public static Texture2D GoldCoin;
        public static Texture2D FortifyWalls;
        public static Texture2D Invincibility;
        public static Texture2D RepairWalls;
        public static Texture2D RepairWallsx10;
        public static Texture2D BuyArcher;
        public static Texture2D UpgradeArcher;

        //Fonts
        public static SpriteFont DYFFont;

        //Music
        public static SoundEffect TitleScreenMusic;
        public static SoundEffect ShopMusic;
        public static SoundEffect GameOver;
        public static SoundEffect Victory;

        //Sounds
        public static SoundEffect WeaponSwing;
        public static SoundEffect PurchaseItem;
        public static SoundEffect GoblinDeath;
        public static SoundEffect LevelComplete;

        //Debug graphics
        public static Texture2D ScalableBox;

        public static void LoadContent(ContentManager Content)
        {
            LoadGraphics(Content);
            LoadSounds(Content);
            LoadMusic(Content);

            DYFFont = Content.Load<SpriteFont>("Fonts/DYFFont");
        }

        private static Texture2D LoadGraphic(ContentManager Content, string filename)
        {
            return Content.Load<Texture2D>(GraphicsDir + filename);
        }

        private static void LoadGraphics(ContentManager Content)
        {
            //Level graphics
            LevelBG = LoadGraphic(Content, "Level Background");
            DaySun = LoadGraphic(Content, "Sun");
            NightMoon = LoadGraphic(Content, "Moon");

            Sword = Content.Load<Texture2D>(GraphicsDir + "Alpha Sword");
            Spear = Content.Load<Texture2D>(GraphicsDir + "Spear");
            Warhammer = Content.Load<Texture2D>(GraphicsDir + "Alpha Warhammer");
            PlayerCastle = Content.Load<Texture2D>(GraphicsDir + "PlayerCastle");
            PlayerCastleFortified = Content.Load<Texture2D>(GraphicsDir + "PlayerCastleFortified");
            PlayerCastleInvincible = LoadGraphic(Content, "PlayerCastleInvincible");
            PlayerArcher = new Texture2D[3] { LoadGraphic(Content, "Archer"), LoadGraphic(Content, "ArcherLvl2"), LoadGraphic(Content, "ArcherLvl3") };
            PlayerSlower = new Texture2D[3] { LoadGraphic(Content, "SlowerSheet"), LoadGraphic(Content, "SlowerSheet2"), LoadGraphic(Content, "SlowerSheet3") };

            //Enemy graphics
            GoldCoinEffect = LoadGraphic(Content, "Gold Coin Effect");
            EnemySpear = LoadGraphic(Content, "Enemy Spear (Complete)");
            GoblinSheet = new Texture2D[3] { LoadGraphic(Content, "Goblin Sheet"), LoadGraphic(Content, "Goblin Sheet 2"), LoadGraphic(Content, "Goblin Sheet 3") };
            GoblinInvincibleSheet = LoadGraphic(Content, "GoblinInvincible Sheet");
            SpearGoblinSheet = new Texture2D[3] { LoadGraphic(Content, "RangedGoblinSheet"), LoadGraphic(Content, "RangedGoblinSheet 2"), LoadGraphic(Content, "RangedGoblinSheet 3") };
            SpearGoblinInvincibleSheet = LoadGraphic(Content, "RangedGoblinSheetInvincible");
            FlyingGoblinSheet = new Texture2D[3] { LoadGraphic(Content, "Flying Goblin Sheet"), LoadGraphic(Content, "Flying Goblin Sheet 2"), LoadGraphic(Content, "Flying Goblin Sheet 3") };
            FlyingGoblinInvincibleSheet = LoadGraphic(Content, "Flying Goblin Sheet Invincible");
            ArmoredGoblinSheet = new Texture2D[3] { LoadGraphic(Content, "Armored Goblin Sheet"), LoadGraphic(Content, "Armored Goblin Sheet 2"), LoadGraphic(Content, "Armored Goblin Sheet 3") };
            ArmoredGoblinInvincibleSheet = LoadGraphic(Content, "Armored Goblin Sheet Invincible");
            RangedArmoredGoblin = new Texture2D[3] { LoadGraphic(Content, "RangedArmoredGoblin"), LoadGraphic(Content, "RangedArmoredGoblin 2"), LoadGraphic(Content, "RangedArmoredGoblin 3") };
            RangedArmoredGoblinInvincible = LoadGraphic(Content, "RangedArmoredGoblinInvincible");

            //Shop graphics
            GoldCoin = LoadGraphic(Content, "Gold Coin");
            FortifyWalls = Content.Load<Texture2D>(GraphicsDir + "ShopIcons/FortifyWallsIcon");
            Invincibility = Content.Load<Texture2D>(GraphicsDir + "ShopIcons/InvincibilityIcon2");
            RepairWalls = Content.Load<Texture2D>(GraphicsDir + "ShopIcons/RepairWallsIcon");
            RepairWallsx10 = Content.Load<Texture2D>(GraphicsDir + "ShopIcons/RepairWallsx10Icon");
            BuyArcher = LoadGraphic(Content, "ShopIcons/ArcherIcon");
            UpgradeArcher = LoadGraphic(Content, "ShopIcons/UpgradeArcherIcon");

            //Debug graphics
            ScalableBox = Content.Load<Texture2D>(GraphicsDir + "ScalableBox");
        }

        private static void LoadSounds(ContentManager Content)
        {
            GoblinDeath = Content.Load<SoundEffect>(SoundDir + "Goblin Death (sihil__ogre1)");
            PurchaseItem = Content.Load<SoundEffect>(SoundDir + "Purchase Item (d-w__coins-01)");
            WeaponSwing = Content.Load<SoundEffect>(SoundDir + "Weapon Swing (qubodup__sharp-swosh-18_01)");
            LevelComplete = Content.Load<SoundEffect>(SoundDir + "Level Complete (Bart Kelsey - orchestra)");
        }

        private static void LoadMusic(ContentManager Content)
        {
            TitleScreenMusic = Content.Load<SoundEffect>(MusicDir + "Home Base Groove");
            ShopMusic = Content.Load<SoundEffect>(MusicDir + "Itty Bitty 8 Bit");
            GameOver = Content.Load<SoundEffect>(MusicDir + "Game Over (spazzo-1493__game-over)");
            Victory = Content.Load<SoundEffect>(MusicDir + "Funk Game Loop");
        }


    }
}