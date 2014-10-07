using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_Your_Castle
{
    //A helper class for fading things in and out
    public class Fade
    {
        //A number designated an infinite number of loops for a fade
        public const int InfiniteLoops = -1;

        protected Color FadeColor;

        //The amount to fade and the min and max fade amounts
        protected int FadeAmount;
        protected int CurFade;
        protected int MinFadeAmount;
        protected int MaxFadeAmount;

        //Start value for tracking loops
        protected int StartFade;

        //The number of times the fade loops
        protected int Loops;
        protected int CurLoops;

        //Time between fades
        protected float FadeTime;
        protected float PrevFade;

        public Fade(Color startingcolor, int fadeamount, int minfade, int maxfade, int loops, float fadetime)
        {
            FadeColor = startingcolor;

            MinFadeAmount = minfade;
            MaxFadeAmount = maxfade;

            FadeAmount = fadeamount;
            if (FadeAmount < 0) CurFade = MaxFadeAmount;
            else CurFade = MinFadeAmount;

            StartFade = FadeAmount;

            Loops = loops;
            CurLoops = 0;

            FadeTime = fadetime;
            RefreshFade();
        }

        //An empty fade
        public static Fade Empty
        {
            get { return new Fade(Color.White, 0, 0, 0, 0, 0f); }
        }

        public bool FadingIn
        {
            get { return (FadeAmount > 0); }
        }

        public bool FadingOut
        {
            get { return (FadeAmount < 0); }
        }

        public int GetCurFade
        {
            get { return CurFade; }
        }

        //Gets the actual color the fade is fading
        public Color GetColor
        {
            get { return FadeColor; }
        }

        //Gets the fade color; this is the value you want when drawing something
        public Color GetFadeColor
        {
            get { return (new Color((int)FadeColor.R, (int)FadeColor.G, (int)FadeColor.B) * (float)(CurFade / 255f)); }
        }

        //Tells if the fade is complete or not
        public virtual bool DoneFading
        {
            get { return (Loops != InfiniteLoops && CurLoops >= Loops); }
        }

        //Gets the color of the fade added or subtracted to the fade value
        public Color GetColorPlusFade(bool add)
        {
            int fadeval = CurFade;

            if (add == false) fadeval = -fadeval;

            return new Color(FadeColor.R + fadeval, FadeColor.G + fadeval, FadeColor.B + fadeval);
        }

        public void FlipFade()
        {
            FadeAmount = -FadeAmount;

            //If the fade amount is the amount we started with, we successfully looped once
            if (FadeAmount == StartFade) CurLoops++;
        }

        //Restarts the fade
        public void RestartFade()
        {
            FadeAmount = StartFade;
            if (FadeAmount < 0) CurFade = MaxFadeAmount;
            else CurFade = MinFadeAmount;

            CurLoops = 0;

            RefreshFade();
        }

        private void RefreshFade()
        {
            PrevFade = Game1.ActiveTime + FadeTime;
        }

        public void Update()
        {
            //Check if the fade time is up
            if (DoneFading == false && Game1.ActiveTime >= PrevFade)
            {
                CurFade += FadeAmount;

                //Make sure not to exceed the max or be less than the min
                if (CurFade >= MaxFadeAmount)
                {
                    CurFade = MaxFadeAmount;
                    FlipFade();
                }
                else if (CurFade <= MinFadeAmount)
                {
                    CurFade = MinFadeAmount;
                    FlipFade();
                }

                RefreshFade();
            }
        }
    }
}
