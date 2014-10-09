using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Defend_Your_Castle
{
    public abstract class PlayerHelper : LevelObject
    {
        //The max level of the helper
        protected int MaxLevel;

        //The level of the helper
        protected int HelperLevel;

        public PlayerHelper()
        {
            MaxLevel = 0;
            HelperLevel = 0;
        }

        public int GetMaxLevel
        {
            get { return MaxLevel; }
        }

        public int GetHelperLevel
        {
            get { return HelperLevel; }
        }
        
        //Sets the position of the helper
        public abstract void SetPosition();

        //Increase the level and stats of the helper
        public void IncreaseLevel()
        {
            HelperLevel++;
            //Ensure the helper cannot surpass its max level
            if (HelperLevel <= MaxLevel)
            {
                IncreaseStats();
            }
            else HelperLevel = MaxLevel;
        }

        //Increases the stats of the PlayerHelper
        public abstract void IncreaseStats();

        //For saving data; convert the player helper into a HelperData
        public abstract HelperData ConvertHelperToData();
    }
}
