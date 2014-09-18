using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defend_Your_Castle
{
    public sealed class FortifyWalls : ShopItem
    {
        private int HealthIncrease;

        public FortifyWalls(Player shopPlayer) : base(shopPlayer)
        {
            // Set the properties of the item
            Name = "Fortify Walls";

            //Set the health increase
            HealthIncrease = 1000;

            Description = "Upgrade your castle walls. +" + HealthIncrease + " Max Health.";
        }

        public override void UseItem()
        {
            base.UseItem();

            //Increase player max health
            ShopPlayer.IncreaseMaxHealth(HealthIncrease);
        }
    }
}
