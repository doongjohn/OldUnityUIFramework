using System.Linq;
using UnityEngine;

public class FoodInventory : InventoryBase
{
    protected override void Awake()
    {
        Init(20, typeof(Item));
    }

    public override void DropItem(Item item, int amount = 1)
    {
        // Code Here...
    }

    // Test
    public void TestAddItems()
    {
        for (int i = 0; i < ItemDB.Items.Count; i++)
        {
            Item item = ItemDB.Spawn(ItemDB.Items.ElementAt(i).Key);
            AddItem(item);
        }
    }
}
