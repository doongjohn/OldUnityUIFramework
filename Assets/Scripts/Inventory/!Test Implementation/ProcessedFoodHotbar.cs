using UnityEngine;

public class ProcessedFoodHotbar : InventoryBase
{
    protected override void Awake()
    {
        Init(5, typeof(ProcessedFoodItem));
    }

    public override void DropItem(Item item, int amount = 1)
    {
        // Code Here...
    }
}
