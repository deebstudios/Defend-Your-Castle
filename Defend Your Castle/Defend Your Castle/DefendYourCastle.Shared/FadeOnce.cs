using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Defend_Your_Castle
{
    //A Fade that only fades in or out once
    public sealed class FadeOnce : Fade
    {
        public FadeOnce(Color startingcolor, float fadeamount, float minfade, float maxfade, float fadetime) : base(startingcolor, fadeamount, minfade, maxfade, 0, fadetime)
        {

        }

        public override bool DoneFading
        {
            get { return (FadeAmount != StartFade); }
        }
    }
}
