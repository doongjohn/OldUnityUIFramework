using System.Collections.Generic;
using UnityEngine;

public class ItemDB : MonoBehaviour
{
    [SerializeField] private Item[] items;

    public static ItemDB Inst
    { get; private set; }
    public static Dictionary<string, Item> Items
    { get; private set; }

    private void Awake()
    {
        Inst = this;

        if (Items == null)
            Items = new Dictionary<string, Item>();
        else
            Items.Clear();

        for (int i = 0; i < items.Length; i++)
        {
            Items.Add(items[i].name, items[i]);
        }
    }

    public static Item Spawn(string name, int count = 1)
    {
        Item item = Instantiate(Items[name].gameObject).GetComponent<Item>();
        item.transform.SetParent(Inst.transform, false);
        item.name = name;

        item.Init();
        item.Count = Mathf.Max(1, count);
        return item;
    }
    public static Item Clone(Item item)
    {
        Item clone = Instantiate(Items[item.Name].gameObject).GetComponent<Item>();
        clone.transform.SetParent(Inst.transform, false);
        clone.name = item.Name;

        clone.Init();
        clone.SetID(item.ID);
        clone.Count = Mathf.Max(1, item.Count);
        return clone;
    }
}
