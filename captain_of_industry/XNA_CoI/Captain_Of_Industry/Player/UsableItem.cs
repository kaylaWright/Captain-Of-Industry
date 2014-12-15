using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Captain_Of_Industry
{
    class UsableItem : InventoryObject
    {
        // Animation representing the player
        private Animation animation;

        public UsableItem (int _ID) : base (_ID, ITEM_TYPE.USABLE)
        {
            
        }
    }
}
