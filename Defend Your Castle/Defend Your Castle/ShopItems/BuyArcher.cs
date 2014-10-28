using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defend_Your_Castle
{
    public sealed class BuyArcher : ShopItem
    {
        public BuyArcher(Player shopPlayer, Shop shop) : base(shopPlayer, shop)
        {
            // Set the properties of the item
            Name = "Archer";

            MaxLevel = 3;

            price = 15000;

            Description = "A loyal helper that defends your fort from enemies.";

            // Get the path to the image of the item
            ImagePath = "Content/Graphics/ShopIcons/Big ArcherIcon.png";
        }

        public override void UseItem()
        {
            base.UseItem();

            //Create an archer
            Archer archer = new Archer(CurLevel - 1);

            for (int i = 0; i < TheShop.GetArcherLevel; i++)
            {
                archer.IncreaseLevel();
            }

            ShopPlayer.AddChild(archer);
            archer.SetPosition();
        }
    }
}
