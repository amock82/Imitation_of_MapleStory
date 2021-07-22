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

    private bool    isDrag = false;     // 인벤토리 창이 드래그 되고있는가?
    private bool    isOpened = false;   // 인벤토리가 열려있는가?

    public static InventoryDrag instance;

    private void Awake()
    {
        _inven = GameObject.Find("InventoryUI");

        upStd = transform.position.y;
        leftStd = transform.position.x;

        originalVec = transform.position;

        instance = this;
    }

    private void Start()    // 게임 시작시 인벤토리를 닫고 시작함
    {
        _inven.SetActive(false);
        isOpened = false;
    }

    public void ExitInventory()     // 인벤토리 닫기 함수
    {
        _inven.SetActive(false);
        isOpened = false;

        ItemDetailUI.instance.SetUIActive(false);       // 아이템 정보 UI를 같이 끔
        SlotDrag.instance.SetIsTrackingMouse(false);    // 마우스로 잡은 아이템이 있다면 UI를 같이 끔
    }

    public void OpenInventory()     // 인벤토리 열기 함수
    {
        _inven.SetActive(true);
        isOpened = true;
    }

    public void OnBeginDrag(PointerEventData eventData) // 인벤토리 창 드래그 시작
    {
        dragPoint = eventData.position;

        // 지정된 범위 이내(인벤토리 상단부)에서만 적용되게 함
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

    public void OnDrag(PointerEventData eventData)      // 인벤토리 창 드래깅
    {
        // 드래그를 시작한 지점을 기준으로 창을 옮기기
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

    public void OnEndDrag(PointerEventData eventData)   // 인벤토리 창 드래그 종료
    {
        isDrag = false;
    }

    // 인벤토리가 열려있는지 반환하는 함수
    public bool GetIsOpened()
    {
        return isOpened;
    }
}
