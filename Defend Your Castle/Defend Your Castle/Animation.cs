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

        //The rate at which the animation plays - 1 is normal speed, > 1 is slower than normal, and < 1 is faster than normal
        private float PlayRate;

        //The full duration of the animation, excluding water
        private float FullAnimDuration;

        //Tells if the animation reverses or not after finishing
        private bool ShouldReverse;
        private bool CurReverse;

        //Tells if the animation is finished or not
        private bool AnimationEnd;

        private Animation()
        {
            Frames = new List<AnimFrame>();
            FullAnimDuration = 0;

            CurFrame = 0;
            CurReverse = false;

            PlayRate = 1f;

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

        public Animation(bool shouldreverse, params AnimFrame[] frames) : this(frames)
        {
            ShouldReverse = shouldreverse;
        }

        //The default origin for drawing a sprite, given a Vector2 value
        public static Vector2 DefaultOrigin(Vector2 TextureDimensions, Direction directionfacing)
        {
            //For X dimensions that don't divide by 2 evenly, we need to get the ceiling when the direction faced is Left and use the floor when the direction faced is Right
            int X = 0;
            if (directionfacing == Direction.Left) X = (int)(TextureDimensions.X / 2f);

            return (new Vector2(X, 0));
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

        public float GetSpeed
        {
            get { return PlayRate; }
        }

        public void RestoreAnimSpeed()
        {
            PlayRate = 1f;

            for (int i = 0; i < Frames.Count; i++)
                Frames[i].RestoreFrameSpeed();
        }

        public void SetAnimSpeed(float speed)
        {
            PlayRate = speed;

            for (int i = 0; i < Frames.Count; i++)
                Frames[i].SetFrameSpeed(speed);
        }

        //Pass in a positive amount to slow the animation and a negative amount to speed it up
        public void ChangeAnimSpeed(float amount)
        {
            PlayRate += amount;

            for (int i = 0; i < Frames.Count; i++)
                Frames[i].ChangeFrameSpeed(amount);
        }

        //Restarts the animation
        public void Restart()
        {
            CurFrame = 0;
            CurrentAnimFrame.ResetFrame();
            AnimationEnd = false;
        }

        //Move onto the next frame
        private void NextFrame()
        {
            CurFrame++;
            if (CurFrame > MaxFrame)
            {
                if (ShouldReverse == false)
                {
                    CurFrame = 0;
                    AnimationEnd = true;
                    //CurLoop++;
                    //if (CurLoop >= MaxLoops) AnimationEnd = true;
                }
                else
                {
                    CurFrame = MaxFrame - 1;
                    if (CurFrame < 0) CurFrame = 0;
                    CurReverse = true;
                }
            }

            CurrentAnimFrame.ResetFrame();
        }

        //Move onto the next frame while the animation is currently in reverse
        private void NextFrameReverse()
        {
            CurFrame--;
            if (CurFrame < 0)
            {
                //We shouldn't reverse, meaning this animation was reset starting in reverse and thus should play only once
                if (ShouldReverse == false)
                {
                    CurFrame = 0;
                    AnimationEnd = true;
                    CurReverse = false;
                }
                //Otherwise, reverse it so it looks like a continuous loop, playing each frame only once (Ex. since it just played the first frame, move to the second)
                else
                {
                    CurFrame = 1;
                    if (CurFrame > MaxFrame) CurFrame = MaxFrame;
                    //CurLoop++;
                    //if (CurLoop >= MaxLoops) AnimationEnd = true;
                    CurReverse = false;
                    AnimationEnd = true;
                }
            }

            CurrentAnimFrame.ResetFrame();
        }

        public void Update()
        {
            if (CurrentAnimFrame.FrameComplete == true)
            {
                if (CurReverse == false) NextFrame();
                else NextFrameReverse();
            }
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

        //The rate at which the frame plays - 1 is normal speed, > 1 is slower than normal, and < 1 is faster than normal
        private float PlayRate;

        //How long this frame lasts
        private float FrameDuration;
        private float PrevDuration;

        private AnimFrame()
        {
            PrevDuration = 0f;

            PlayRate = 1f;

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
            get { return (FrameDuration * PlayRate); }
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
        private Vector2 TrueDrawPos(Vector2 Position, Direction directionfacing, float rotation)
        {
            Vector2 trueoffset = OffsetOrigin;

            //Due to the way SpriteEffects.FlipHorizontally works, we need to negate the X offset so it properly offsets the frame
            if (directionfacing == Direction.Left) trueoffset.X = -trueoffset.X;

            //Offset rotation so it doesn't change where the object is drawn
            //Currently, this works for only 90 degree rotations. Avoid setting the rotation to π * 2 degrees and instead set it to 0
            Vector2 rotationoffset = Vector2.Zero;
            if (rotation != 0f)
            {
                float pi90 = (float)MathHelper.PiOver2;
                float pi180 = (float)Math.PI;
                float pi270 = (float)Math.PI + MathHelper.PiOver2;

                //Adjust the position based on how these rotation values affect how the sprite draws
                if (rotation < pi270) rotationoffset.X = DrawSection.Width;
                if (rotation > pi90) rotationoffset.Y = DrawSection.Height;
            }

            return (Position + trueoffset + rotationoffset);
        }

        public void RestoreFrameSpeed()
        {
            PlayRate = 1f;
        }

        public void SetFrameSpeed(float speed)
        {
            PlayRate = speed;
        }

        public void ChangeFrameSpeed(float amount)
        {
            PlayRate += amount;
        }

        //Resets the frame's duration
        public void ResetFrame()
        {
            PrevDuration = Game1.ActiveTime;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D SpriteSheet, Vector2 Position, Direction directionfacing, Color statuscolor, float rotation, float depth)
        {
            Vector2 truepos = TrueDrawPos(Position, directionfacing, rotation);
            Vector2 origin = GetOrigin(directionfacing);

            spriteBatch.Draw(SpriteSheet, truepos, DrawSection, statuscolor, rotation, origin, 1f, GetFlip(directionfacing), depth);
        }
    }
}