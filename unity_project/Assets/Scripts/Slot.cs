using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item     item;
    public int      itemCount;
    public Image    itemImage;

    public void AddItem(Item itemGet, int countGet)
    {
        item = itemGet;
        itemCount = countGet;
        itemImage.sprite = item.itemImage;
    }
}
