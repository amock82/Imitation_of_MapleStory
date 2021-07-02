using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    GameObject      _inven;

    Vector2         dragPoint;

    float           upStd;            // 위쪽 기준점
    float           leftStd;          // 왼쪽 기준점

    Vector2         originalVec;

    private bool    isDrag = false; 

    private void Awake()
    {
        _inven = GameObject.Find("InventoryUI");

        upStd = transform.position.y;
        leftStd = transform.position.x;

        originalVec = transform.position;
    }

    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            if (_inven.active == false)
                _inven.SetActive(true);
            else if (_inven.active == true)
                _inven.SetActive(false);
        }
    }

    public void ExitInventory()
    {
        _inven.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragPoint = eventData.position;

        if (_inven.transform.position.x - 97.5f < eventData.position.x && eventData.position.x < _inven.transform.position.x + 97.5f)
        {
            if (_inven.transform.position.y + 170 < eventData.position.y && eventData.position.y < _inven.transform.position.y + 190)
                isDrag = true;
            else
                isDrag = false;
        }
        else
            isDrag = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDrag == true)
        {
            if (eventData.position.y <= 768)
            {
                originalVec += (eventData.position - dragPoint);

                _inven.transform.position = originalVec;

                dragPoint = eventData.position;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
    }
}
