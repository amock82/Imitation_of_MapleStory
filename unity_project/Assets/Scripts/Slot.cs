using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item     item;
    public int      itemCount;
    public Image    itemImage;
    public Text     _text;

    private void Awake()
    {
        _text = GetComponentInChildren<Text>();
        //GetComponentsInChildren<Image>()
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(1);
            if (item != null)
            {
                //item.itemAmount += 1;

                Item t = Inventory.instance.item[2].Find(x => x = item);

                Inventory.instance.item[2].Remove(item);
            }
        }
    }

    public void PutItem(Item itemGet)
    {
        item = itemGet;
        itemCount = itemGet.itemAmount;
        itemImage.sprite = item.itemImage;

        _text.text = itemCount.ToString();

        itemImage.SetNativeSize();

        itemImage.gameObject.SetActive(true);
    }

    public void AddItem(int amount)
    {
        item.itemAmount += amount;
    }

    public void PullItem()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;

        _text.text = "";

        itemImage.gameObject.SetActive(false);
    }
}
