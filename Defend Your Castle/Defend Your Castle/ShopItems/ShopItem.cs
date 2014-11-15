using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Defend_Your_Castle
{
    // Base class used to represent an item in the shop
    //When an item can no longer be bought, grey it out
    public class ShopItem : INotifyPropertyChanged
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

        // The base price of the item
        protected int BasePrice;

        // The price of the item
        protected int price;

        // The amount to increase the cost of the item after each level
        protected int PriceIncrease;

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
            set
            {
                price = value;
                OnPropertyChanged("PriceString");
            }
        }

        public String PriceString
        {
            // Display "Maxed Out" if the player has fully upgraded the ShopItem; otherwise, display the price
            get { return ((CurLevel < MaxLevel || MaxLevel == InfinitePurchases) ? price.ToString() : "Maxed Out"); }
        }

        // The string representation of the current level of the ShopItem
        // Format: CurLevel / MaxLevel
        private String levelString;

        // Format: CurLevel / MaxLevel
        // This is both a getter and a setter property so the OnPropertyChanged interface can be implemented
        public String LevelString
        {
            get
            { return levelString; }
            set
            {
                levelString = value;
                OnPropertyChanged("LevelString");
            }
        }

        public int GetCurrentLevel
        {
            get { return CurLevel; }
        }

        public int GetMaxLevel
        {
            get { return MaxLevel; }
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
            // Increment the current level by 1
            SetCurrentLevel(CurLevel + 1);
        }

        // Performs the action of the shop item when used on the HUD (used by consumables only)
        public virtual void UseItemInGame()
        {
            
        }

        public void SetCurrentLevel(int level)
        {
            // Set the new level
            CurLevel = level;

            // Make sure the current level doesn't go below 0
            if (CurLevel < 0) CurLevel = 0;

            // Set the new price of the item
            Price = BasePrice + (CurLevel * PriceIncrease);

            // Set the displayed level
            SetDisplayedLevel();
        }

        public void SetDisplayedLevel()
        {
            // Set the level string to "CurLevel / MaxLevel"
            // If the item can be purchased an infinite number of times, set the level string to an empty String
            LevelString = ((MaxLevel <= InfinitePurchases) ? String.Empty : CurLevel.ToString() + " / " + MaxLevel.ToString());
        }

        // Declare the PropertyChanged event
        // PropertyChanged implementation courtesy of MSDN: http://msdn.microsoft.com/en-us/library/ms743695(v=vs.110).aspx
        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event 
        protected void OnPropertyChanged(String name)
        {
            // Set the EventHandler to the PropertyChanged event
            PropertyChangedEventHandler handler = PropertyChanged;

            // Make sure the event exists
            if (handler != null)
            {
                // It does, so invoke the PropertyChanged event
                handler(this, new PropertyChangedEventArgs(name));
            }
        }


    }
}