using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragUI : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    Vector2 dragPoint;
    Vector2 originalVec;

    private void Awake()
    {
        originalVec = transform.position;
    }

    // UI 드래그 시작 시 호출
    public void OnBeginDrag(PointerEventData eventData)
    {
        dragPoint = eventData.position;
    }

    // UI 드래그 시 위치 이동
    public void OnDrag(PointerEventData eventData)
    {
        if (0 <= eventData.position.y && eventData.position.y <= 768 && 0 <= eventData.position.x && eventData.position.x <= 1366)
        {
            originalVec += (eventData.position - dragPoint);

            transform.position = originalVec;
        }

        dragPoint = eventData.position;
    }

    public void InitOriginalVec()
    {
        originalVec = transform.position;
    }
}
