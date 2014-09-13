using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defend_Your_Castle
{
    public class Shop
    {
        // Reference to the player
        Player ShopPlayer;

        // List of available ShopItems
        List<ShopItem> ShopItems;

        public Shop(Player shopPlayer)
        {
            // Get the current player shopping
            ShopPlayer = shopPlayer;

            // Initialize the ShopItems list
            ShopItems = new List<ShopItem>();

            // Add ShopItems to the shop

        }

        public bool CanBuyItem(ShopItem item)
        {
            return (ShopPlayer.Gold >= item.Price);
        }

        public void BuyItem(ShopItem item)
        {
            // Check if the player can buy the item
            if (CanBuyItem(item))
            {
                // Play the "purchase" sound

                // Subtract the item price from the player's gold
                ShopPlayer.Gold -= item.Price;

                // Use the shop item
                item.UseItem();
            }
            else // The player doesn't have enough gold for the item
            {
                // Notify the player of this
            }
        }


    }
}
