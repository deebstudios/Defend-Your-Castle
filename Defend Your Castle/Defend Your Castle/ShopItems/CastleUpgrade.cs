using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defend_Your_Castle
{
    public sealed class CastleUpgrade : ShopItem
    {
        // The amount of health to upgrade the castle
        private int HealthIncrease;

        public CastleUpgrade(Player shopPlayer, Shop shop) : base(shopPlayer, shop)
        {
            // Set the properties of the item
            Name = "Reinforce Fort";
            
            // Set the health increase
            HealthIncrease = 250;

            price = 750;

            Description = "Increase the durability of your fort's walls. +" + HealthIncrease + " Max Health";

            // Set the displayed level
            SetDisplayedLevel();

            // Get the path to the image of the item
            ImagePath = "Content/Graphics/ShopIcons/Big FortifyWallsIcon.png";
        }

        public override void UseItem()
        {
            base.UseItem();

            // Use the castle upgrade to upgrade the player's castle
            ShopPlayer.UpgradeCastle(HealthIncrease);

            // Update the player's HP on the Shop
            ShopPlayer.UpdateHealthInShop();
        }


    }
}