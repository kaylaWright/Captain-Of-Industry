using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Captain_Of_Industry
{
    enum ITEM_TYPE
    {
        CONSUMABLE,
        USABLE,
        EMPTY
    }

    abstract class InventoryObject
    {
        public InventoryObject(int _ID, ITEM_TYPE _type)
        {
            ID = _ID;
            type = _type;
        }
        public int ID; // object ID linked to inventory item list located in InventoryManager
        public float quality; // could be used for food rotting, etc.
        ITEM_TYPE type;
    }
}
