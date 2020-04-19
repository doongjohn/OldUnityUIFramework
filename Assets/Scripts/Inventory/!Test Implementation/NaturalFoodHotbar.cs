using UnityEngine;

public class NaturalFoodHotbar : InventoryBase
{
    protected override void Awake()
    {
        Init(4, typeof(NaturalFoodItem));
    }

    public override void DropItem(Item item, int amount = 1)
    {
        // Code Here...
    }
}
