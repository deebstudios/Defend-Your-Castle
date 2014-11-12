using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defend_Your_Castle
{
    public sealed class RepairWallsx10 : RepairWalls
    {
        public RepairWallsx10(Player shopPlayer, Shop shop) : base(shopPlayer, shop)
        {
            Name = "Repair Wallsx10";

            HealAmount *= 10;

            price *= 10;

            Description = "+" + HealAmount + " Health.";
            // "Greatly repair your fort's walls.\n+" + HealAmount + " Health";

            // Set the displayed level
            SetDisplayedLevel();

            // Get the path to the image of the item
            ImagePath = "Content/Graphics/ShopIcons/Big RepairWallsx10Icon.png";
        }
    }
}
