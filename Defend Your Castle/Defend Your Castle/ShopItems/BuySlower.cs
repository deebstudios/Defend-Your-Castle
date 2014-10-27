using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defend_Your_Castle
{
    public sealed class BuySlower : ShopItem
    {
        public BuySlower(Player shopPlayer, Shop shop) : base(shopPlayer, shop)
        {
            // Set the properties of the item
            Name = "Slower";

            MaxLevel = 3;
            
            price = 10000;

            Description = "A loyal helper that slows enemy movement and attack speeds.";

            // Get the path to the image of the item
            ImagePath = "Content/Graphics/ShopIcons/SlowerIcon.png";
        }

        public override void UseItem()
        {
            base.UseItem();

            //Create a slower
            Slower slower = new Slower(CurLevel - 1);

            for (int i = 0; i < TheShop.GetSlowerLevel; i++)
            {
                slower.IncreaseLevel();
            }

            ShopPlayer.AddChild(slower);
            slower.SetPosition();
        }
    }
}
