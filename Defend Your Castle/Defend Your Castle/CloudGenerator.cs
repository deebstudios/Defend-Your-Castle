using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_Your_Castle
{
    //A cloud generator for levels; it chooses which clouds appear, moves them on the screen, and removes them when needed
    public sealed class CloudGenerator
    {
        //The number of cloud types
        public const int CloudTypes = 3;

        //The clouds
        private List<Cloud> Clouds;

        //The direction the wind is going, dictating which direction the clouds will move
        private int WindDirection;

        public CloudGenerator()
        {
            Clouds = new List<Cloud>();

            WindDirection = 0;
        }

        //Clears the cloud generator
        public void Clear()
        {
            Clouds.Clear();
        }

        //Creates a new cloud
        public void GenerateCloud()
        {

        }

        //May need the level reference for something
        public void Update(Level level)
        {
            for (int i = 0; i < Clouds.Count; i++)
            {
                Clouds[i].Update(level);
                if (Clouds[i].ShouldRemove == true)
                {
                    Clouds.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Level level)
        {
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

            public Cloud(Texture2D cloudtex, Vector2 cloudloc)
            {
                CloudTex = cloudtex;
                CloudLoc = cloudloc;
            }

            //Determines if the cloud should be removed from the CloudGenerator or not
            public bool ShouldRemove
            {
                get { return true; }
            }

            public void Update(Level level)
            {

            }

            public void Draw(SpriteBatch spriteBatch, Level level)
            {
                //The color will change with night
                spriteBatch.Draw(CloudTex, CloudLoc, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, CloudDepth);
            }
        }
    }
}
