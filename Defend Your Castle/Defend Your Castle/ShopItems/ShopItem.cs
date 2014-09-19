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

        // The name of the item
        protected String Name;

        // A brief description of the item
        protected String Description;

        // The price of the item
        protected int price;

        // The stock of the item in the shop
        protected int Stock;

        // The path to the image used to represent the item
        public String ImagePath
        {
            get;
            set;
        }
        
        //The level and max level of the shop item (how many times it can be upgraded)
        protected int MaxLevel;
        protected int CurLevel;

        public int Price
        {
            get { return price; }
        }

        public ShopItem(Player shopPlayer)
        {
            // Get the current player shopping
            ShopPlayer = shopPlayer;

            MaxLevel = InfinitePurchases;
            CurLevel = 0;
        }

        public virtual bool CanBuy(Player ShopPlayer)
        {
            return (ShopPlayer.Gold >= price && CurLevel < MaxLevel);
        }

        // Performs the action of the shop item
        public virtual void UseItem()
        {
            CurLevel++;
        }


    }
}