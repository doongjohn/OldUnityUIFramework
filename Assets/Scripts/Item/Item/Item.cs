using UnityEngine;

public class Item : MonoBehaviour
{
    #region Var: Inspector
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private string itemName = "Item";
    [SerializeField, TextArea] private string itemDesc = "This is an Item";
    [SerializeField] private int itemStackLimit = 0;
    #endregion

    #region Var: Properties
    // Item Info
    public string ID
    { get; protected set; }
    public string Name
    { 
        get => itemName; 
        protected set { itemName = value; } 
    }
    public string Desc
    {
        get => itemDesc;
        protected set { itemDesc = value; }
    }
    public Sprite Icon
    {
        get => itemIcon;
        protected set { itemIcon = value; }
    }

    // Item Count
    private int bk_ItemCount = 1;
    public int Count
    {
        get => bk_ItemCount;
        set { bk_ItemCount = itemStackLimit == 0 ? Mathf.Max(0, value) : Mathf.Clamp(value, 0, itemStackLimit); }
    }
    public int StackLimit
    {
        get => itemStackLimit;
        protected set { itemStackLimit = Mathf.Max(1, value); }
    }
    public bool IsMaxStack => StackLimit != 0 && Count == StackLimit;

    // Ref
    public InventoryBase Inventory
    { get; private set; }
    #endregion

    #region Method: Initialize
    public virtual void Init()
    {
        SetID(Name);
    }
    #endregion

    #region Method: Item
    public virtual void SetID(string id)
    {
        ID = id;
    }
    public virtual void OnAdd(InventoryBase inventory)
    {
        Inventory = inventory;
    }
    public virtual void OnRemove()
    {

    }

    public bool AddCount(Item other)
    {
        if (ID != other.ID || IsMaxStack)
            return false;

        if (StackLimit == 0)
        {
            Count += other.Count;
            other.Count = 0;
        }
        else
        {
            int countLeft = (StackLimit - Count);
            Count += other.Count;
            other.Count -= countLeft;
        }
        return true;
    }
    #endregion
}
