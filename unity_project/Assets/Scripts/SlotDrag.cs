using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotDrag : MonoBehaviour
{
    static public SlotDrag instance;
    public Slot slotDrag;

    public Image _image;

    bool isTrackingMouse = false;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (isTrackingMouse == true)
        {
            transform.position = Input.mousePosition;

            Debug.Log(2);
        }
    }

    public void SetImage(Image _itemDrag)
    {
        _image.sprite = _itemDrag.sprite;

        SetColor(0.5f);
    }

    public void SetColor(float alpha)
    {
        Color color = _image.color;
        color.a = alpha;

        _image.color = color;
    }

    public bool GetIsTrackingMouse()
    {
        return isTrackingMouse;
    }

    public void SetIsTrackingMouse(bool value)
    {
        isTrackingMouse = value;

        if(value == true)
        {
            SetColor(0.5f);
        }
        else
        {
            SetColor(0);
        }
    }
}
