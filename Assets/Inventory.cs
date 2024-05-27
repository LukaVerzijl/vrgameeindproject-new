using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public List<Item> items = new List<Item>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void AddItem(Item itemToAdd)
    {
        bool itemExists = false;
        foreach (Item item in items)
        {
            if (item.name == itemToAdd.name)
            {
                item.count += itemToAdd.count;
                itemExists = true;
                break;
            }
        }
        if (!itemExists)
        {
            items.Add(itemToAdd);
        }
        Debug.Log(itemToAdd.count + " " + itemToAdd.name + " added to inventory");
    }

    public void RemoveItem(Item itemToRemove)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].name == itemToRemove.name)
            {
                items[i].count -= itemToRemove.count;
                if (items[i].count <= 0)
                {
                    items.RemoveAt(i);
                }
                break;
            }
        }
        Debug.Log(itemToRemove.count + " " + itemToRemove.name + " removed from inventory");
    }

    public int GetItemCount(string itemName)
    {
        foreach (Item item in items)
        {
            if (item.name == itemName)
            {
                return item.count;
            }
        }
        return 0;
    }
}
