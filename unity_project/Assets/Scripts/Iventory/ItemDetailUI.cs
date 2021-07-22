using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailUI : MonoBehaviour
{
    GameObject   _itemDetailUI;     // 아이템 표기 UI

    Text         _itemName;         // 아이템 이름
    Text         _itemDetail;       // 아이템 설명

    Image        _itemImage;        // 아이템 이미지

    public static ItemDetailUI instance;

    private void Awake()
    {
        _itemDetailUI = GameObject.Find("ItemDetailUI");

        _itemName = _itemDetailUI.GetComponentInChildren<Text>();
        _itemDetail = _itemDetailUI.GetComponentsInChildren<Text>()[1];

        _itemImage = _itemDetailUI.GetComponentsInChildren<Image>()[3];

        instance = this;

        _itemDetailUI.SetActive(false);
    }

    private void Update()
    {
        // 아이템 표기 UI가 활성화 되있으면, 마우스 포인터 위치를 기반으로 이동시킴
        if (_itemDetailUI.active == true)
        {
            _itemDetailUI.transform.position = Input.mousePosition + Vector3.up * -93 + Vector3.right * 150;
        }
    }

    //////////////////////////////////////////////////////
    // 아이템 표기 UI에 아이템의 정보들을 갱신하는 함수들

    public void SetUIActive(bool value)
    {
        _itemDetailUI.SetActive(value);
    }

    public void SetItemName(string value)
    {
        _itemName.text = value;
    }

    public void SetItemDetail(string value)
    {
        _itemDetail.text = value;
    }

    public void SetItemImage(Sprite sprite)
    {
        _itemImage.sprite = sprite;
    }

    public void UITransform()
    {
        _itemDetailUI.transform.position = Input.mousePosition + Vector3.up * -93 + Vector3.right * 150;
    }
}
