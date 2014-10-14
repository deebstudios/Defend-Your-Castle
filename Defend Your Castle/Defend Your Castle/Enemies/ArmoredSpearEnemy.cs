using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defend_Your_Castle
{
    public sealed class ArmoredSpearEnemy : SpearEnemy
    {
        public ArmoredSpearEnemy(Level level, float Y, int speedadd) : base(level, Y, speedadd)
        {
            ObjectSheet = LoadAssets.RangedArmoredGoblin;
            InvincibleSheet = LoadAssets.RangedArmoredGoblinInvincible;

            WeaponWeakness = (int)Player.WeaponTypes.Warhammer;
            Range = 120;

            ProjectileDamage = 40;
        }
    }
}
