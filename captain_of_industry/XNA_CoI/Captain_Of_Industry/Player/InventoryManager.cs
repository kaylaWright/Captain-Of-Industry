using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Captain_Of_Industry
{
    struct InventorySlot
    {
        public int ID;
        public int count;
        public ITEM_TYPE type;
    }

    class InventoryManager
    {
        private InventorySlot[] inventory = new InventorySlot[15]; // the 15 slots of the players inventory, each slot has an item ID and an item count
        private ConsumableItem tempConsumable;
        private UsableItem tempUsable;

        public InventoryManager()
        {
            for (int i = 0; i < 15; ++i)
            {
                inventory[i].ID = -1;
                inventory[i].count = 0;
                inventory[i].type = ITEM_TYPE.EMPTY;
            }
            // test item
            itemList.Add(new ConsumableItem(0, 5, 5, 5));
        }
        // this list contains every inventory object type in the game
        // it is populated by an external XML file
        public List<InventoryObject> itemList = new List<InventoryObject>();
    }
}
