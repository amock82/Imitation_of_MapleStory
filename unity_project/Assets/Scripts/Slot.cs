using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Dynamic;

public class Slot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Item     item;
    public int      itemCount;
    public Image    itemImage;
    public Text     _text;

    public bool     isDown = false;

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

        if (itemGet.itemName != "Empty")
        {
            _text.text = itemCount.ToString();
        }
        else
        {
            _text.text = "";
        }

        itemImage.SetNativeSize();

        itemImage.gameObject.SetActive(true);



    }

    public void AddItem(int amount)
    {
        item.itemAmount += amount;
    }

    public void PullItem()
    {
        item = Inventory.instance.emptyItem;
        itemCount = 0;
        itemImage.sprite = item.itemImage;

        _text.text = "";

        itemImage.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (item != null && SlotDrag.instance.GetIsTrackingMouse() == false && isDown == false)
        {
            if (item.itemName != "Empty")
            {
                SlotDrag.instance.slotDrag = this;
                SlotDrag.instance.SetImage(itemImage);
                //SlotDrag.instance.transform.position = eventData.position;

                SlotDrag.instance.SetIsTrackingMouse(true);
            }

            isDown = true;
        }
        else if (SlotDrag.instance.GetIsTrackingMouse() == true && SlotDrag.instance.GetDoubleClickTimer() > 0 && SlotDrag.instance.slotDrag.item.itemType != Item.ItemType.Loot)
        {
            if (item == SlotDrag.instance.slotDrag.item )
            {
                if (item.itemType == Item.ItemType.Consumable)
                {
                    item.itemConsumable.Use();

                    Debug.Log("use");

                    SlotDrag.instance.slotDrag.item.itemAmount -= 1;
                }
            }
            else
            {
                Item tempItem = ScriptableObject.CreateInstance<Item>();

                tempItem.ChangeItem(item);

                item.ChangeItem(SlotDrag.instance.slotDrag.item);

                //SlotDrag.instance.slotDrag.item.ChangeItem(Inventory.instance.emptyItem);
                SlotDrag.instance.slotDrag.item.ChangeItem(tempItem);
            }

            SlotDrag.instance.SetIsTrackingMouse(false);
        }
        else if (SlotDrag.instance.GetIsTrackingMouse() == true)
        {
            if (item != null && SlotDrag.instance.slotDrag != null && item != SlotDrag.instance.slotDrag.item)
            {
                Item tempItem = ScriptableObject.CreateInstance<Item>();

                tempItem.ChangeItem(item);

                item.ChangeItem(SlotDrag.instance.slotDrag.item);

                SlotDrag.instance.slotDrag.item.ChangeItem(tempItem);
            }

            SlotDrag.instance.SetIsTrackingMouse(false);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDown = false;

        Debug.Log(1);
    }
}
