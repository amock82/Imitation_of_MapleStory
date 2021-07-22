using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Dynamic;

public class Slot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item     item;           // 슬릇에 배치될 아이템
    public int      itemCount;      // 슬릇에 배치될 아이템의 갯수
    public Image    itemImage;      // 슬릇에 배치될 아이템 이미지
    public Text     _text;          // 아이템 갯수 출력 Text

    public bool     isDown = false; // 슬릇이 클릭되었는가

    private void Awake()
    {
        _text = GetComponentInChildren<Text>();
    }

    // 슬릇에 아이템을 배치하는 함수
    public void PutItem(Item itemGet)
    {
        item = itemGet;
        itemCount = itemGet.itemAmount;
        itemImage.sprite = item.itemImage;

        // 아이템 이름이 "Empty"라면 Text 비우기
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

    // 배치된 아이템의 갯수 증가
    public void AddItem(int amount)
    {
        item.itemAmount += amount;
    }

    // 슬릇 비우기 (빈 아이템 객체로 바꿈)
    public void PullItem()
    {
        item = Inventory.instance.emptyItem;
        itemCount = 0;
        itemImage.sprite = item.itemImage;

        _text.text = "";

        itemImage.gameObject.SetActive(false);
    }

    // 마우스 버튼 Down시에 실행됨
    public void OnPointerDown(PointerEventData eventData)
    {
        // 아이템이 들어있는 슬릇에 첫 클릭을 한 경우
        if (item != null && SlotDrag.instance.GetIsTrackingMouse() == false && isDown == false)
        {
            // 아이템이 빈 아이템 객체가 아닐경우
            if (item.itemName != "Empty")
            {
                // 아이템을 잡음 (마우스 위치를 추척하게함)
                SlotDrag.instance.slotDrag = this;
                SlotDrag.instance.SetImage(itemImage);

                SlotDrag.instance.SetIsTrackingMouse(true);
            }

            isDown = true;
        }
        // 이미 아이템을 잡은 상태이며, 기타 아이템이 아니며, 더블클릭 판정이 날 경우
        else if (SlotDrag.instance.GetIsTrackingMouse() == true && SlotDrag.instance.GetDoubleClickTimer() > 0 && SlotDrag.instance.slotDrag.item.itemType != Item.ItemType.Loot)
        {
            // 같은 슬릇을 더블클릭 한 경우
            if (item == SlotDrag.instance.slotDrag.item )
            {
                // 아이템 유형이 소비아이템인 경우
                if (item.itemType == Item.ItemType.Consumable)
                {
                    // 아이템 사용
                    item.itemConsumable.Use();

                    // 갯수 줄임
                    SlotDrag.instance.slotDrag.item.itemAmount -= 1;
                }
            }
            // 다른 슬릇과 더블클릭 판정이 난 경우
            // 일반적인 아이템 배치변경
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
        // 일반적인 아이템 배치변경
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
    }

    public void OnPointerEnter(PointerEventData eventData)      // 슬릇에 아이템이 있는경우 마우스를 올리면 아이템 정보 확인 UI 띄우기
    {
        if (item.itemName != "Empty")
        {
            ItemDetailUI.instance.SetUIActive(true);

            ItemDetailUI.instance.SetItemName(item.itemName);
            ItemDetailUI.instance.SetItemDetail(item.itemDetail);     

            ItemDetailUI.instance.SetItemImage(itemImage.sprite);
        }
    }

    // 슬릇에서 마우스가 벗어나면, 아이템 표기 UI Off
    public void OnPointerExit(PointerEventData eventData)
    {
        ItemDetailUI.instance.SetUIActive(false);
    }
}
