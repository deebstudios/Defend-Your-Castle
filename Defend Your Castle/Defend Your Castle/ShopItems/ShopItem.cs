using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defend_Your_Castle
{
    // Base class used to represent an item in the shop
    public class ShopItem
    {
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

        // The image used to represent the item
        protected AnimFrame Image;
        
        public int Price
        {
            get { return price; }
        }

        public ShopItem(Player shopPlayer)
        {
            // Get the current player shopping
            ShopPlayer = shopPlayer;
        }

        // Performs the action of the shop item
        public virtual void UseItem()
        {

        }


    }
}