using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defend_Your_Castle
{
    public sealed class Invincibility : ShopItem
    {
        public Invincibility(Player shopPlayer, Shop shop) : base(shopPlayer, shop)
        {
            Name = "Invincibility";

            // Set the max number of uses to 1
            MaxLevel = 1;

            price = 1000;

            Description = "5 seconds of invulnerability. One allowed at a time";
            // "Shields your fort with an\nimpervious energy for 5 seconds.\nYou can have only one at a time.";

            // Set the displayed level
            SetDisplayedLevel();

            // Get the path to the image of the item
            ImagePath = "Content/Graphics/ShopIcons/Big InvincibilityIcon.png";
        }

        public override void UseItem()
        {
            base.UseItem();

            // Add the consumable to the HUD
            TheShop.AddConsumableToHUD(this);
        }

        public override void UseItemInGame()
        {
            // Use invincibility on the player
            ShopPlayer.UseInvincibility();
            
            // Decrease the current level of the invincibility by 1
            SetCurrentLevel(CurLevel - 1);
        }


    }
}