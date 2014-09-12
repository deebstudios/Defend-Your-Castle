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
        public static Texture2D Sword;
        public static Texture2D Warhammer;

        public static Song TestSong;
        public static SoundEffect TestSound;

        public static void LoadContent(ContentManager Content)
        {
            LoadGraphics(Content);
        }

        private static void LoadGraphics(ContentManager Content)
        {
            Sword = Content.Load<Texture2D>("Alpha Sword");
            Warhammer = Content.Load<Texture2D>("Alpha Warhammer");

            TestSong = Content.Load<Song>("Music\\Mario Party - Peaceful Mushroom Village");
            TestSound = Content.Load<SoundEffect>("Sounds/test");
        }

        private static void LoadSounds(ContentManager Content)
        {

        }

        private static void LoadMusic(ContentManager Content)
        {

        }


    }
}