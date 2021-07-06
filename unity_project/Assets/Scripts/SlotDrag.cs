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

    private void Awake()
    {
        instance = this;
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
}
