using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_Your_Castle
{
    //A cloud generator for levels; it chooses which clouds appear, moves them on the screen, removes them when needed, and creates new clouds
    public sealed class CloudGenerator
    {
        //The number of cloud types; also the max number of clouds on screen at once
        public const int MaxCloudTypes = 3;

        //The base amount of the cloud timer and the amount the cloud timer varies
        public const int BaseCloudTimer = 700;
        public const int CloudTimerVariation = 500;

        //The lowest and highest Y values the clouds can appear in
        private const int LowestY = 0;
        private const int HighestY = 70;

        //The min and max X values a cloud can spawn
        public static int MinX;
        public static int MaxX;

        //Random generator for clouds
        private Random Rand;

        //The clouds
        private List<Cloud> Clouds;

        //See how many clouds we should have max on a level
        private int NumClouds;

        //The direction and speed of the wind, dictating which direction and how fast the clouds will move
        private float WindSpeed;

        //The timer for generating clouds; it varies each level
        private float CloudTimer;
        private float PrevCloudTimer;

        //Constructor
        public CloudGenerator()
        {
            Clouds = new List<Cloud>();

            WindSpeed = 0f;

            NumClouds = 0;
            CloudTimer = BaseCloudTimer;
            PrevCloudTimer = 0f;

            Rand = new Random();

            Refresh();
        }

        static CloudGenerator()
        {
            //When clouds spawn, spawn them so they're out of view
            MinX = -LoadAssets.BGClouds[1].Width;
            MaxX = (int)Game1.ScreenSize.X;
        }

        //Determines if the cloud generator should generate a new cloud
        private bool CanGenerate
        {
            get { return (Clouds.Count < NumClouds && Game1.ActiveTime >= PrevCloudTimer); }
        }

        //Resets the cloud timer
        private void ResetCloudTimer()
        {
            PrevCloudTimer = Game1.ActiveTime + Rand.Next(BaseCloudTimer - CloudTimerVariation, BaseCloudTimer + CloudTimerVariation + 1);
        }

        //Clears the cloud generator
        public void Clear()
        {
            Clouds.Clear();
        }

        //Creates a new cloud
        public void GenerateCloud(bool levelstart = false)
        {
            //Choose a random texture
            Texture2D cloudtex = LoadAssets.BGClouds[Rand.Next(0, /*MaxCloudTypes*/2)];

            //Find our X value; default to the min x value
            float x = MinX;

            //If the wind speed is 0 or we just started the level, start with some clouds already onscreen
            if (levelstart == true || WindSpeed == 0f)
            {
                //Divide the screen in 3 so the clouds don't spawn on or near the same X position
                int maxxdivide = MaxX / MaxCloudTypes;

                //Find min and max x range
                int minx = Clouds.Count * maxxdivide;
                int maxx = minx + maxxdivide;

                x = Rand.Next(minx, maxx + 1);
            }
            //Otherwise if the wind is moving left, start the cloud on the right side of the screen
            else if (WindSpeed < 0f) x = MaxX;

            //Choose a random Y value
            float y = Rand.Next(LowestY, HighestY);

            //Create the cloud and add it to the list
            Cloud newcloud = new Cloud(cloudtex, new Vector2(x, y));
            Clouds.Add(newcloud);

            //Reset the cloud timer
            ResetCloudTimer();
        }

        //Called at the start of each level
        public void Refresh()
        {
            //Clear all clouds from the screen
            Clear();

            //Choose a random number of clouds from 0 to the max # of clouds and a random wind speed from -1 to 1
            NumClouds = Rand.Next(0, MaxCloudTypes + 1);
            WindSpeed = Rand.Next(-1, 2);

            //We don't want stationary clouds, so if the wind speed is 0, make it -.5 or .5 instead
            if (WindSpeed == 0)
            {
                //Default to -.5
                WindSpeed = -.5f;

                //Check for a random number; if it's 1, set the windspeed to .5
                int randnum = Rand.Next(0, 2);
                if (randnum == 1) WindSpeed = .5f;
            }
            //CloudTimer = Rand.Next((int)(BaseCloudTimer - CloudTimerVariation), (int)(BaseCloudTimer + CloudTimerVariation) + 1);

            //Start the level with a random number of clouds from 0 to the number of total clouds that will appear onscreen at once in this level
            int randcloudsspawn = Rand.Next(0, NumClouds + 1);
            for (int i = 0; i < randcloudsspawn; i++)
            {
                GenerateCloud(true);
            }
        }

        //May need the level reference for something
        public void Update(Level level)
        {
            //Update the clouds
            for (int i = 0; i < Clouds.Count; i++)
            {
                Clouds[i].Update(WindSpeed, level);
                //If wind direction is 0, that means the clouds stay in place and thus won't be removed
                if (Clouds[i].ShouldRemove(WindSpeed) == true)
                {
                    Clouds.RemoveAt(i);
                    i--;

                    //Reset the cloud timer to create a new one
                    ResetCloudTimer();
                }
            }

            //Check if we can generate a new cloud, and do so if possible
            if (CanGenerate == true)
                GenerateCloud();
        }

        public void Draw(SpriteBatch spriteBatch, Level level)
        {
            //Draw the clouds
            for (int i = 0; i < Clouds.Count; i++)
            {
                Clouds[i].Draw(spriteBatch, level);
            }
        }

        //A cloud used by the CloudGenerator
        //We don't derive from LevelObject because it's a much simpler object that exists in the background and doesn't interact with anything
        private sealed class Cloud
        {
            //The depth to draw the clouds
            private const float CloudDepth = Level.CelestialDepth + .00001f;

            //The texture the cloud uses
            private Texture2D CloudTex;

            //The location of the cloud
            private Vector2 CloudLoc;

            //Constructor
            public Cloud(Texture2D cloudtex, Vector2 cloudloc)
            {
                CloudTex = cloudtex;
                CloudLoc = cloudloc;
            }

            //Gets the cloud's Y position
            public float GetCloudY
            {
                get { return CloudLoc.Y; }
            }

            //Clouds underneath other clouds will be drawn on top
            private float GetCloudDepth
            {
                get { return CloudDepth + (CloudLoc.Y / 10000f); }
            }

            //Determines if the cloud should be removed from the CloudGenerator or not
            public bool ShouldRemove(float windspeed)
            {
                if (windspeed < 0f)
                    return (CloudLoc.X <= CloudGenerator.MinX);
                else if (windspeed > 0f)
                    return (CloudLoc.X >= CloudGenerator.MaxX);
                //If wind speed is 0, that means the clouds stay in place and thus won't be removed
                else return false;
            }

            public void Update(float windspeed, Level level)
            {
                CloudLoc.X += windspeed;
            }

            public void Draw(SpriteBatch spriteBatch, Level level)
            {
                int fade = 255 - ((int)level.GetNightFade.GetCurFade + 15);

                Color cloudcolor = new Color(fade, fade, fade);

                //The color will change with night
                spriteBatch.Draw(CloudTex, CloudLoc, null, cloudcolor, 0f, Vector2.Zero, 1f, SpriteEffects.None, GetCloudDepth);
            }
        }
    }
}
