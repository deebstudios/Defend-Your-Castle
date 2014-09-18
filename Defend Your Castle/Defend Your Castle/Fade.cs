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
    public sealed class Fade
    {
        //An infinite fade
        public const int InfiniteFade = -1;

        private Color FadeColor;

        //The amount to fade and the min and max fade amounts
        private int FadeAmount;
        private int CurFade;
        private int MinFadeAmount;
        private int MaxFadeAmount;

        //Start value for tracking loops
        private int StartFade;

        //The number of times the fade loops
        private int Loops;
        private int CurLoops;

        //Time between fades
        private float FadeTime;
        private float PrevFade;

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

        public bool FadingIn
        {
            get { return (FadeAmount > 0); }
        }

        public bool FadingOut
        {
            get { return (FadeAmount < 0); }
        }

        //Gets the fade color; this is the value you want when drawing something
        public Color GetFadeColor
        {
            get { return new Color(FadeColor.R, FadeColor.G, FadeColor.B, CurFade); }
        }

        //Tells if the fade is complete or not
        public bool DoneFading
        {
            get { return (Loops != InfiniteFade && CurLoops >= Loops); }
        }

        public void FlipFade()
        {
            FadeAmount = -FadeAmount;
        }

        private void RefreshFade()
        {
            PrevFade = Game1.ActiveTime + FadeTime;
        }

        public void Update()
        {
            //If we're not done fading, check if the fade time is up
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

                //If the fade amount is the amount we started with, we successfully looped once
                if (FadeAmount == StartFade) CurLoops++;

                RefreshFade();
            }
        }
    }
}
