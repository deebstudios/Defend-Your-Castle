using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Defend_Your_Castle
{
    public class Shop
    {
        // Reference to GamePage.xaml
        private GamePage gamePage;

        // Reference to the player
        private Player ShopPlayer;

        // List of available Upgrades
        public List<ShopItem> ShopUpgrades;

        // List of available Prepare/Repair ShopItems
        public List<ShopItem> ShopPrepareRepairs;

        // List of available Items
        public List<ShopItem> ShopItems;

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

                // Update the Shop UI with the new gold amount
                ShopPlayer.UpdateGoldAmountInShop();
            }
            else // The player doesn't have enough gold for the item
            {
                // Notify the player of this
            }
        }

        public void LoadShopData(ShopData shopData)
        {
            for (int i = 0; i < shopData.ShopUpgrades.Count; i++)
            {
                ShopUpgrades[i].SetCurrentLevel(shopData.ShopUpgrades[i].CurLevel);
            }

            for (int i = 0; i < shopData.ShopPrepareRepairs.Count; i++)
            {
                ShopPrepareRepairs[i].SetCurrentLevel(shopData.ShopPrepareRepairs[i].CurLevel);
            }

            for (int i = 0; i < shopData.ShopItems.Count; i++)
            {
                ShopItems[i].SetCurrentLevel(shopData.ShopItems[i].CurLevel);
            }
        }

        public void AddConsumableToHUD(ShopItem consumable)
        {
            // Add the consumable to the consumables GridView on the HUD
            gamePage.HUD_ConsumablesList.Items.Add(consumable);
        }


    }
}