using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Captain_Of_Industry
{
    class ConsumableItem : InventoryObject
    {
        public float healthVal, sanityVal, wealthVal; // how this consumable affects each statistic
        public ConsumableItem(int _ID, int _hp, int _san, int _weal) : base(_ID, ITEM_TYPE.CONSUMABLE)
        {
            healthVal = _hp;
            sanityVal = _san;
            wealthVal = _weal;
        }
    }
}
