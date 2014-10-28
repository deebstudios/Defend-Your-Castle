using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defend_Your_Castle
{
    // Base class used to represent an item in the shop
    //When an item can no longer be bought, grey it out
    public class ShopItem
    {
        //A number denoting that there is no limit to how many times the player can buy a particular item
        public static int InfinitePurchases = -1;

        // Reference to the player. The reference is the same as ShopItem's ShopPlayer
        protected Player ShopPlayer;

        // Reference to the shop
        protected Shop TheShop;

        // The name of the item
        protected String name;

        // A brief description of the item
        protected String description;

        // The price of the item
        protected int price;

        //The level and max level of the shop item (how many times it can be upgraded)
        protected int MaxLevel;
        protected int CurLevel;

        // The path to the image used to represent the item
        public String ImagePath
        {
            get;
            set;
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        public int Price
        {
            get { return price; }
        }

        public String PriceString
        {
            get { return price + " Gold"; }
        }

        // Format: CurLevel / MaxLevel
        public String LevelString
        {
            get
            {
                // Get the string representation of the current level 
                String levelString = CurLevel.ToString();

                // Get the string representation of the max level
                // Use "∞" if the max level equals InfinitePurchases
                levelString += " / " + ((MaxLevel > InfinitePurchases) ? MaxLevel.ToString() : "∞");

                // Return the level string
                return levelString;
            }
        }

        public int GetCurrentLevel
        {
            get { return CurLevel; }
        }

        public ShopItem(Player shopPlayer, Shop shop)
        {
            // Get the current player shopping
            ShopPlayer = shopPlayer;

            // Get the shop
            TheShop = shop;

            MaxLevel = InfinitePurchases;
            CurLevel = 0;
        }

        public virtual bool CanBuy(Player ShopPlayer)
        {
            return (ShopPlayer.Gold >= price && (MaxLevel == InfinitePurchases || CurLevel < MaxLevel));
        }

        // Performs the action of the shop item
        public virtual void UseItem()
        {
            CurLevel++;
        }

        // Performs the action of the shop item when used on the HUD (used by consumables only)
        public virtual void UseItemInGame()
        {
            
        }

        public void SetCurrentLevel(int level)
        {
            CurLevel = level;
        }


    }
}