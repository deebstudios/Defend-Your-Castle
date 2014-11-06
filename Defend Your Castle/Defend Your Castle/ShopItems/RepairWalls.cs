using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_Your_Castle
{
    public class RepairWalls : ShopItem
    {
        protected int HealAmount;

        public RepairWalls(Player shopPlayer, Shop shop) : base(shopPlayer, shop)
        {
            Name = "Repair Walls";

            HealAmount = 400;

            price = 250;

            Description = "Repair your fort's walls.\n+" + HealAmount + " Health";

            // Set the displayed level
            SetDisplayedLevel();

            // Get the path to the image of the item
            ImagePath = "Content/Graphics/ShopIcons/Big RepairWallsIcon.png";
        }

        public sealed override bool CanBuy(Player ShopPlayer)
        {
            return (base.CanBuy(ShopPlayer) == true && ShopPlayer.GetHealth < ShopPlayer.GetMaxHealth);
        }

        public sealed override void UseItem()
        {
            base.UseItem();

            ShopPlayer.Heal(HealAmount);
        }
    }
}
