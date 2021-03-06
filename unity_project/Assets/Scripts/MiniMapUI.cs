using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiniMapUI : MonoBehaviour, IDragHandler, IBeginDragHandler
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

    // 미니맵 드래그 시작 시 호출
    public void OnBeginDrag(PointerEventData eventData)
    {
        dragPoint = eventData.position;
    }

    // 미니맵 UI 드래그 시 위치 이동
    public void OnDrag(PointerEventData eventData)
    {
        if (0 <= eventData.position.y && eventData.position.y <= 768 && 0 <= eventData.position.x && eventData.position.x <= 1366)
        {
            originalVec += (eventData.position - dragPoint);

            transform.position = originalVec;

            // 왼쪽 상단 근처에서는 고정위치값을 가지게 함
            if (leftStd - 10 <= transform.position.x && transform.position.x <= leftStd + 10)
                transform.position = new Vector2(leftStd, transform.position.y);
            if (upStd - 10 <= transform.position.y && transform.position.y <= upStd + 10)
                transform.position = new Vector2(transform.position.x, upStd);
        }

        dragPoint = eventData.position;
    }
}
