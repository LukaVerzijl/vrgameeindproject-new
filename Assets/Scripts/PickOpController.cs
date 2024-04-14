using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickOpController : MonoBehaviour
{
    public Item item = new Item("ItemName", 1);

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Inventory.Instance.AddItem(item);
            Destroy(gameObject);
        }
    }
}
