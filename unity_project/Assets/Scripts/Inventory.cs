using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    GameObject _inven;

    Vector2 dragPoint;

    Vector2 upStdVec;
    Vector2 LeStdVec;

    Vector2 originalVec;

    private void Awake()
    {
        _inven = GameObject.Find("InventoryUI");

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

            Debug.Log(1);
        }
    }

    public void ExitIventory()
    {
        _inven.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Start");

        dragPoint = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_inven.transform.position.x - 97.5f < eventData.position.x && eventData.position.x < _inven.transform.position.x + 97.5f)
        {
            if (_inven.transform.position.y + 170 < eventData.position.y && eventData.position.y < _inven.transform.position.y + 190)
            {
                _inven.transform.position += (Vector3)(eventData.position - dragPoint);


                dragPoint = eventData.position;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("EndDrag");
    }
}
