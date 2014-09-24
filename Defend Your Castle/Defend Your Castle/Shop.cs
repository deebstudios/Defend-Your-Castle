using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defend_Your_Castle
{
    public class Shop
    {
        // Reference to GamePage.xaml
        private GamePage gamePage;

        // Reference to the player
        Player ShopPlayer;

        // List of available Upgrades
        List<ShopItem> ShopUpgrades;

        // List of available Prepare/Repair ShopItems
        List<ShopItem> ShopPrepareRepairs;

        // List of available Items
        List<ShopItem> ShopItems;

        public Shop(GamePage gamepage, Player shopPlayer)
        {
            // Get the reference to GamePage.xaml
            gamePage = gamepage;

            // Get the current player shopping
            ShopPlayer = shopPlayer;

            // Initialize the three ShopItems lists. Change these lists to modify the items in the shop
            
            // Upgrades
            ShopUpgrades = new List<ShopItem>() { new CastleUpgrade(shopPlayer) };

            // Prepare/Repair
            ShopPrepareRepairs = new List<ShopItem>() { new RepairWalls(shopPlayer) };

            // Items
            ShopItems = new List<ShopItem>();

            // Assign the shop items to the Shop
            AssignShopItems();
        }

        public void AssignShopItems()
        {
            gamePage.Shop_UpgradesList.ItemsSource = ShopUpgrades;
            gamePage.Shop_PrepareRepairList.ItemsSource = ShopPrepareRepairs;
            gamePage.Shop_ItemsList.ItemsSource = ShopItems;
        }

        public void BuyItem(ShopItem item)
        {
            // Check if the player can buy the item
            if (item.CanBuy(ShopPlayer) == true)
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
