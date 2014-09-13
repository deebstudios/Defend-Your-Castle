using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_Your_Castle
{
    //An animation for objects
    //NOTE: We may not need the commented fields, but if we do this will be modified to use them
    public sealed class Animation
    {
        //The animation frames
        private List<AnimFrame> Frames;

        //The current frame
        private int CurFrame;

        //How many times the animation loops
        //private int MaxLoops;
        //private int CurLoop;

        //The full duration of the animation, excluding water
        private float FullAnimDuration;

        //Tells if the animation reverses or not after finishing
        //private bool ShouldReverse;
        //private bool CurReverse;

        //Tells if the animation is finished or not
        private bool AnimationEnd;

        private Animation()
        {
            Frames = new List<AnimFrame>();
            FullAnimDuration = 0;

            CurFrame = 0;
            
            AnimationEnd = false;
        }

        public Animation(params AnimFrame[] frames) : this()
        {
            Frames.AddRange(frames);

            //Get the full duration here to cut down on time later (Ex. creating hitboxes that last an entire animation)
            //Also set all the frames animations to this one
            for (int i = 0; i < Frames.Count; i++)
            {
                FullAnimDuration += Frames[i].GetDuration;
            }
        }

        //The default origin for drawing a sprite, given a Vector2 value
        public static Vector2 DefaultOrigin(Vector2 TextureDimensions, Direction directionfacing)
        {
            //For X dimensions that don't divide by 2 evenly, we need to get the ceiling when the direction faced is Left and use the floor when the direction faced is Right
            float X = TextureDimensions.X / 2f;
            //if (X != (int)X)
            //{
            //    if (directionfacing == Direction.Left) X = (int)Math.Ceiling(X);
            //    else X = (int)Math.Floor(X);
            //}

            return (new Vector2((int)X, (int)TextureDimensions.Y));
        }

        //Indexer for accessing frames
        public AnimFrame this[int frame]
        {
            get
            {
                if (frame >= 0 && frame <= MaxFrame) return Frames[frame];
                else return null;
            }
        }

        public int CurrentFrame
        {
            get { return CurFrame; }
        }

        public int MaxFrame
        {
            get { return (Frames.Count - 1); }
        }

        public AnimFrame CurrentAnimFrame
        {
            get { return Frames[CurFrame]; }
        }

        //Tells if the animation is active (Only applies if the animation is a one-time animation that is reset before being played, like an attack animation)
        public bool IsActive
        {
            get { return (AnimationEnd == false); }
        }

        //Checks if the animation is done
        public bool IsAnimationComplete
        {
            get { return AnimationEnd; }
        }

        //Move onto the next frame
        private void NextFrame()
        {
            CurFrame++;
            if (CurFrame > MaxFrame)
            {
                //if (ShouldReverse == false)
                //{
                    CurFrame = 0;
                    AnimationEnd = true;
                    //CurLoop++;
                    //if (CurLoop >= MaxLoops) AnimationEnd = true;
                //}
                //else
                //{
                //    CurFrame = MaxFrame - 1;
                //    if (CurFrame < 0) CurFrame = 0;
                //    CurReverse = true;
                //}
            }

            CurrentAnimFrame.ResetFrame();
        }

        public void Update()
        {
            if (CurrentAnimFrame.FrameComplete == true)
                NextFrame();
        }

        //Pass in the spritesheet
        public void Draw(SpriteBatch spriteBatch, Texture2D spritesheet, Vector2 Position, Direction directionfacing, Color color, float rotation, float Layer)
        {
            if (spritesheet != null)
                CurrentAnimFrame.Draw(spriteBatch, spritesheet, Position, directionfacing, color, rotation, Layer);
        }
    }

    //An animation frame; it can also be used independently if only one frame needs to be drawn and you don't want to make an animation for it
    public sealed class AnimFrame
    {
        //The part of the spritesheet to draw
        private Rectangle DrawSection;

        //The amount to offset the default drawing origin
        private Vector2 OffsetOrigin;

        //How long this frame lasts
        private float FrameDuration;
        private float PrevDuration;

        private AnimFrame()
        {
            PrevDuration = 0f;

            DrawSection = Rectangle.Empty;
            OffsetOrigin = Vector2.Zero;
        }

        public AnimFrame(Rectangle framebox, float frameduration) : this()
        {
            DrawSection = framebox;

            FrameDuration = frameduration;
        }

        public AnimFrame(Rectangle framebox, float frameduration, Vector2 offsetorigin) : this(framebox, frameduration)
        {
            //Negate the offset origin so we can pass in positive values and still have it draw correctly
            OffsetOrigin = -offsetorigin;
        }

        //Tells if this frame is complete
        public bool FrameComplete
        {
            get { return (Math.Round(Game1.ActiveTime - PrevDuration, 1) >= GetDuration); }
        }

        public float GetDuration
        {
            get { return FrameDuration; }
        }

        //The size of the animation frame
        public Vector2 FrameSize
        {
            get { return new Vector2(DrawSection.Width, DrawSection.Height); }
        }

        //Gets whether the frame should be flipped or not based on the direction it's facing
        private SpriteEffects GetFlip(Direction directionfacing)
        {
            return (directionfacing == Direction.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
        }

        //Gets the origin to draw the sprite
        private Vector2 GetOrigin(Direction directionfacing)
        {
            Vector2 origin = Animation.DefaultOrigin(new Vector2(DrawSection.Width, DrawSection.Height), directionfacing);
            return origin;
        }

        //Gets the location to draw the frame, with all other factors taken into account
        private Vector2 TrueDrawPos(Vector2 Position, Direction directionfacing)
        {
            Vector2 trueoffset = OffsetOrigin;

            //Due to the way SpriteEffects.FlipHorizontally works, we need to negate the X offset so it properly offsets the frame
            if (directionfacing == Direction.Left) trueoffset.X = -trueoffset.X;

            return (Position + trueoffset);
        }

        //Resets the frame's duration
        public void ResetFrame()
        {
            PrevDuration = Game1.ActiveTime;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D SpriteSheet, Vector2 Position, Direction directionfacing, Color statuscolor, float rotation, float depth)
        {
            Vector2 truepos = TrueDrawPos(Position, directionfacing);
            Debug.WriteLine(truepos);

            Vector2 origin = GetOrigin(directionfacing);
            //Debug.WriteLine(origin);

            spriteBatch.Draw(SpriteSheet, truepos, DrawSection, statuscolor, rotation, origin, 1f, GetFlip(directionfacing), depth);
        }
    }
}