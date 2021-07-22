using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotDrag : MonoBehaviour
{
    static public SlotDrag  instance;
    public Slot             slotDrag;   // 드래그되는 아이템의 정보가 담길 슬릇 객체

    public Image            _image;     // 드래그되는 아이템의 이미지

    bool                    isTrackingMouse = false;    // 드래그중일 때 True처리 (아이템이미지가 마우스를 추적하는중인지)

    float                   doubleClickTimer = 0;       // 더블클릭 타이머, 더블클릭 판단 기준

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        // 드래그 중이면 마우스로 잡은 아이템이 마우스를 추적하게 함
        if (isTrackingMouse == true)
        {
            transform.position = Input.mousePosition;
        }

        // 더블클릭 타이머가 0보다 클때, 흐르는 시간만큼 차감
        if (doubleClickTimer > 0)
        {
            doubleClickTimer -= Time.deltaTime;
        }
    }

    // 아이템 이미지의 마우스 추적 여부 반환함수
    public bool GetIsTrackingMouse()
    {
        return isTrackingMouse;
    }

    // 더블클릭 타이머 반환함수
    public float GetDoubleClickTimer()
    {
        return doubleClickTimer;
    }

    // 드래그중인 아이템의 이미지 반투명 셋팅
    public void SetImage(Image _itemDrag)
    {
        _image.sprite = _itemDrag.sprite;

        SetColor(0.5f);
    }

    // Color를 반투명 처리
    public void SetColor(float alpha)
    {
        Color color = _image.color;
        color.a = alpha;

        _image.color = color;
    }

    // 마우스 추적 여부 설정
    public void SetIsTrackingMouse(bool value)
    {
        isTrackingMouse = value;

        // true일 경우, 이미지의 반투명처리와 더블클릭 타이머 초기화
        if(value == true)
        {
            SetColor(0.5f);
            doubleClickTimer = 0.5f;
        }
        // false일 경우, 이미지를 완전 투명화 처리 (안보이게)
        else
        {
            SetColor(0);
        }
    }
}
