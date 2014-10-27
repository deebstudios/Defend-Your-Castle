using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defend_Your_Castle
{
    public sealed class ArmoredSpearEnemy : SpearEnemy
    {
        public ArmoredSpearEnemy(Level level, float Y, float speedadd, int costume) : base(level, Y, speedadd, costume)
        {
            ObjectSheet = LoadAssets.RangedArmoredGoblin[costume];
            InvincibleSheet = LoadAssets.RangedArmoredGoblinInvincible;

            Gold = 150;

            WeaponWeakness = (int)Player.WeaponTypes.Warhammer;
            Range = 120;

            ProjectileDamage = 50;
        }
    }
}
