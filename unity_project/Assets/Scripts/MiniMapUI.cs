using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiniMapUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    Vector2 dragPoint;
    float upStd;            // 위쪽 기준점
    float leftStd;          // 왼쪽 기준점

    Vector2 originalVec;

    private void Awake()
    {
        upStd = transform.position.y;
        leftStd = transform.position.x;

        originalVec = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragPoint = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.position.y <= upStd + GetComponent<RectTransform>().rect.height / 2)
        {
            originalVec += (eventData.position - dragPoint);

            transform.position = originalVec;

            if (leftStd - 10 <= transform.position.x && transform.position.x <= leftStd + 10)
                transform.position = new Vector2(leftStd, transform.position.y);
            if (upStd - 10 <= transform.position.y && transform.position.y <= upStd + 10)
                transform.position = new Vector2(transform.position.x, upStd);
        }

        dragPoint = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }
}
