using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defend_Your_Castle
{
    public class CastleUpgrade : ShopItem
    {
        // The amount of health to upgrade the castle
        private int HealthIncrease;

        public CastleUpgrade(Player shopPlayer) : base(shopPlayer)
        {
            // Set the properties of the item
            Name = "Fortify Castle";
            Description = "Upgrade your castle to a more premium metal. +" + HealthIncrease + " Health.";

            // Get the image of the item
            //Image = LoadAssets.ImagePathHere

            // Set the health increase
            HealthIncrease = 1500;
        }

        public override void UseItem()
        {
            // Use the castle upgrade to upgrade the player's castle
            ShopPlayer.UpgradeCastle(HealthIncrease);
        }


    }
}