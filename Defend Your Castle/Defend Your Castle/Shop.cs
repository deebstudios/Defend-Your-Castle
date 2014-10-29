using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            ShopUpgrades = new List<ShopItem>() { new CastleUpgrade(shopPlayer, this), new FortifyCastle(shopPlayer, this),
                                                  new RepairWalls(shopPlayer, this), new RepairWallsx10(shopPlayer, this) };

            // Prepare/Repair
            ShopPrepareRepairs = new List<ShopItem>() { new BuyArcher(shopPlayer, this), new BuySlower(shopPlayer, this),
                                                        new UpgradeArcher(shopPlayer, this), new UpgradeSlower(shopPlayer, this) };

            // Items
            ShopItems = new List<ShopItem>() { new Invincibility(shopPlayer, this) };

            // Assign the shop items to the Shop
            AssignShopItems();
        }

        public int GetArcherLevel
        {
            get { return ShopPrepareRepairs[2].GetCurrentLevel; }
        }

        public int GetSlowerLevel
        {
            get { return ShopPrepareRepairs[3].GetCurrentLevel; }
        }

        public int GetStrengthenWallsLevel
        {
            get { return ShopUpgrades[1].GetCurrentLevel; }
        }

        public void AssignShopItems()
        {
            gamePage.Shop_UpgradesList.ItemsSource = ShopUpgrades;
            gamePage.Shop_PrepareRepairList.ItemsSource = ShopPrepareRepairs;
            gamePage.Shop_ItemsList.ItemsSource = ShopItems;
        }

        public void BuyItem(ShopItem item, Button TheButton)
        {
            // Check if the player can buy the item
            if (item.CanBuy(ShopPlayer) == true)
            {
                // Play the "purchase" sound
                SoundManager.PlaySound(LoadAssets.PurchaseItem);

                // Subtract the item price from the player's gold
                ShopPlayer.Gold -= item.Price;

                // Use the shop item
                item.UseItem();

                // Update the Shop UI with the new gold amount
                ShopPlayer.UpdateGoldAmountInShop();
            }
            else if (ShopPlayer.Gold < item.Price) // The player doesn't have enough gold for the item
            {
                // Notify the player of this
                gamePage.Shop_NotEnoughGoldFlyout.ShowAt(TheButton);
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

        public void AddConsumablesToHUD()
        {
            // Loop through all the consumables
            for (int i = 0; i < ShopItems.Count; i++)
            {
                // Check if the consumable was purchased
                if (ShopItems[i].GetCurrentLevel > 0)
                {
                    // Add the consumable to the HUD
                    AddConsumableToHUD(ShopItems[i]);
                }
            }
        }

        public void AddConsumableToHUD(ShopItem consumable)
        {
            // Add the consumable to the consumables GridView on the HUD
            gamePage.HUD_ConsumablesList.Items.Add(consumable);
        }

        public void RemoveConsumablesFromHUD()
        {
            // Clear all the items from the HUD
            gamePage.HUD_ConsumablesList.Items.Clear();
        }


    }
}