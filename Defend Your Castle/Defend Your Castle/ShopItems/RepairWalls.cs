using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_Your_Castle
{
    public sealed class RepairWalls : ShopItem
    {
        private int HealAmount;

        public RepairWalls(Player shopPlayer) : base(shopPlayer)
        {
            Name = "Repair Walls";

            HealAmount = 1000;

            Description = "Repair your castle walls. +" + HealAmount + " Health";
        }

        public override bool CanBuy(Player ShopPlayer)
        {
            return (base.CanBuy(ShopPlayer) == true && ShopPlayer.GetHealth < ShopPlayer.GetMaxHealth);
        }

        public override void UseItem()
        {
            base.UseItem();

            ShopPlayer.Heal(HealAmount);
        }
    }
}
