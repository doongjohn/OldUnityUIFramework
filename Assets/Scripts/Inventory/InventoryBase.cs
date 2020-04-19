﻿using System;
using System.Linq;
using UnityEngine;

public abstract class InventoryBase : MonoBehaviour
{
    #region Var: Inspector
    [SerializeField] protected InventoryUIBase inventoryUI;
    #endregion

    #region Var: Inventory Data
    protected Item[] items;
    #endregion

    #region Var: Properties
    public Type[] SlotTypes
    { get; private set; }
    public int Size
    { get; private set; }
    public int EmptySlots
    { get; protected set; }
    public bool IsFull => EmptySlots == 0 ? true : false;
    public bool IsEmpty => EmptySlots == Size ? true : false;
    #endregion

    #region Method: Unity
    protected virtual void Awake()
    {
        Init(1, typeof(Item));
    }
    #endregion

    #region Method: Initialize
    protected virtual void Init(int size, params Type[] slotTypes)
    {
        // Init Array
        items = new Item[size];

        // Init Values
        Size = size;
        EmptySlots = size;

        // Init Type Array
        SlotTypes = new Type[slotTypes.Length];
        Array.Copy(slotTypes, 0, SlotTypes, 0, slotTypes.Length);

        // Init UI
        inventoryUI?.Init(this, MoveItem);
    }
    #endregion

    #region Method: Helper
    // Check Valid
    public bool IsValidItem(Item item)
    {
        for (int i = 0; i < SlotTypes.Length; i++)
        {
            if (SlotTypes[i].IsAssignableFrom(item.GetType()))
                return true;
        }
        return false;
    }
    public bool IsValidIndex(int index)
    {
        return !(index < 0 || index > Size - 1);
    }

    // Check Item
    public bool ContainsItemID(Item item)
    {
        return GetIndex_ItemID(item) != -1 ? true : false;
    }
    public bool ContainsItem(Item item)
    {
        return GetIndex_Item(item) != -1 ? true : false;
    }
    public bool ContainsStackable(Item item)
    {
        return GetIndex_Stackable(item) != -1 ? true : false;
    }

    // Get Item
    public Item GetItem(int index)
    {
        return items[index];
    }

    // Get Index
    public int GetIndex_ItemID(Item item)
    {
        return Array.FindIndex(items, i => i != null ? i.ID == item.ID : false);
    }
    public int GetIndex_Item(Item item)
    {
        return Array.IndexOf(items, item);
    }
    public int GetIndex_Stackable(Item item)
    {
        return Array.FindIndex(items, i => (i != null && i.ID == item.ID && !i.IsMaxStack));
    }
    public int GetIndex_EmptySlot()
    {
        return Array.IndexOf(items, default);
    }
    #endregion

    #region Method: Inventory
    public virtual void Clear(int size)
    {
        Array.Clear(items, 0, Size);
        EmptySlots = Size;

        // Clear UI
        inventoryUI?.Clear();
    }
    public virtual void AddItem(Item item, int index = -1)
    {
        if (item == null)
            return;

        void AddNew(int to)
        {
            items[to] = item;
            item.OnAdd(this);
            EmptySlots--;
        }

        // Add To Index
        if (IsValidIndex(index) && items[index] == null)
        {
            AddNew(index);

            // Update UI
            inventoryUI?.UpdateSlot(index);
        }
        else
        {
            // Add To Existing Item
            if (ContainsStackable(item))
            {
                int stackableIndex = GetIndex_Stackable(item);
                if (items[stackableIndex].AddCount(item))
                {
                    if (item.Count == 0)
                    {
                        Destroy(item.gameObject);
                        item = null;
                    }

                    // Update UI
                    inventoryUI?.UpdateSlot(stackableIndex);
                }
            }

            if (item == null)
                return;

            // Add To Empty Slot
            if (!IsFull)
            {
                int emptyIndex = GetIndex_EmptySlot();
                AddNew(emptyIndex);

                // Update UI
                inventoryUI?.UpdateSlot(emptyIndex);
            }
            // Drop Item
            else
            {
                DropItem(item, item.Count);
            }
        }
    }
    public virtual bool TryAddItem(Item item, int index = -1)
    {
        if (item == null)
            return false;

        void AddNew(int to)
        {
            items[to] = item;
            item.OnAdd(this);
            EmptySlots--;
        }

        // Add To Index
        if (IsValidIndex(index) && items[index] == null)
        {
            AddNew(index);

            // Update UI
            inventoryUI?.UpdateSlot(index);
            return true;
        }
        else
        {
            // Add To Existing Item
            if (ContainsStackable(item))
            {
                int stackableIndex = GetIndex_Stackable(item);
                if (items[stackableIndex].AddCount(item))
                {
                    if (item.Count == 0)
                        Destroy(item.gameObject);

                    // Update UI
                    inventoryUI?.UpdateSlot(stackableIndex);
                    return true;
                }
            }

            if (item == null)
                return false;

            // Add To Empty Slot
            if (!IsFull)
            {
                int emptyIndex = GetIndex_EmptySlot();
                AddNew(emptyIndex);

                // Update UI
                inventoryUI?.UpdateSlot(emptyIndex);
                return true;
            }
            // Drop Item
            else
            {
                return false;
            }
        }
    }
    public virtual void RemoveItem(int index, int amount = 1)
    {
        if (IsEmpty || !IsValidIndex(index) || items[index] == null)
            return;

        amount = Mathf.Max(0, amount);
        items[index].Count -= amount;

        if (items[index].Count == 0)
        {
            Destroy(items[index].gameObject);
            items[index] = null;
            EmptySlots++;
        }

        // Update UI
        inventoryUI?.UpdateSlot(index);
    }
    public virtual void RemoveItem(Item item, int amount = 1)
    {
        if (IsEmpty || GetIndex_Item(item) == -1)
            return;

        int index = GetIndex_Item(item);

        amount = Mathf.Max(0, amount);
        items[index].Count -= amount;

        if (items[index].Count == 0)
        {
            Destroy(items[index].gameObject);
            items[index] = null;
            EmptySlots++;
        }

        // Update UI
        inventoryUI?.UpdateSlot(index);
    }
    public virtual void DeleteItem(int index)
    {
        if (IsEmpty || !IsValidIndex(index) || items[index] == null)
            return;

        Destroy(items[index].gameObject);
        items[index] = null;
        EmptySlots++;

        // Update UI
        inventoryUI?.UpdateSlot(index);
    }
    public virtual bool MoveItem(int index, InventoryBase targetInventory, int targetIndex)
    {
        if (!IsValidIndex(index)
        || targetInventory == null
        || !targetInventory.IsValidIndex(targetIndex)
        || !targetInventory.IsValidItem(items[index])
        || (targetInventory.items[targetIndex] != null && !IsValidItem(targetInventory.items[targetIndex])))
            return false;

        // Add ExistingItem
        if (targetInventory.items[targetIndex]?.AddCount(items[index]) ?? false)
        {
            if (items[index].Count == 0)
                DeleteItem(index);

            // Update UI
            inventoryUI?.UpdateSlot(index);
            targetInventory.inventoryUI?.UpdateSlot(targetIndex);
            return true;
        }

        // Move Item
        if (this != targetInventory && targetInventory.items[targetIndex] == null)
        {
            targetInventory.AddItem(ItemDB.Clone(items[index]), targetIndex);
            DeleteItem(index);
        }
        // Swap Item
        else
        {
            Item fromBuffer = items[index];
            items[index] = targetInventory.items[targetIndex];
            targetInventory.items[targetIndex] = fromBuffer;

            if (this == targetInventory)
            {
                // Update UI
                inventoryUI?.MoveSlot(index, targetIndex);
            }
            else
            {
                items[index].OnAdd(this);
                targetInventory.items[targetIndex].OnAdd(targetInventory);

                // Update UI
                inventoryUI?.UpdateSlot(index);
                targetInventory.inventoryUI?.UpdateSlot(targetIndex);
            }
        }

        return true;
    }

    public abstract void DropItem(Item item, int amount = 1);
    #endregion
}
