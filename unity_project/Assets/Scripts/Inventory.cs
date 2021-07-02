using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Item> item;

    public void addItem(Item itemGet)
    {
        item.Add(itemGet);

        
    }
}
