using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defend_Your_Castle.ShopItems
{
    public sealed class RepairWallsx10 : RepairWalls
    {
        public RepairWallsx10(Player shopPlayer, Shop shop) : base(shopPlayer, shop)
        {
            HealAmount = 10000;

            Description = "Greatly repair your fort's walls.\n+" + HealAmount + " Health";

            // Get the path to the image of the item
            ImagePath = "Content/Graphics/ShopIcons/Big RepairWallsx10Icon.png";
        }
    }
}
