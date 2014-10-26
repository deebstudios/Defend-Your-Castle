using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Defend_Your_Castle
{
    //A ShopItem that makes the player's castle stronger and take less damage from attacks
    public sealed class FortifyCastle : ShopItem
    {
        public FortifyCastle(Player shopPlayer, Shop shop) : base(shopPlayer, shop)
        {
            // Set the properties of the item
            Name = "Strengthen Walls";

            MaxLevel = 1;

            price = 25000;

            Description = "Strengthen your fort's walls to better defend against enemy attacks! +10% reduced damage from all attacks";

            // Get the path to the image of the item
            ImagePath = "Content/Graphics/ShopIcons/FortifyCastleIcon.png";
        }

        public override void UseItem()
        {
            base.UseItem();

            //Fortify the player castle
            ShopPlayer.FortifyCastle();
        }
    }
}
