using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailUI : MonoBehaviour
{
    GameObject   _itemDetailUI;

    Text         _itemName;
    Text         _itemDetail;

    Image        _itemImage;

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
        if (_itemDetailUI.active == true)
        {
            _itemDetailUI.transform.position = Input.mousePosition + Vector3.up * -93 + Vector3.right * 150;
        }
    }

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
