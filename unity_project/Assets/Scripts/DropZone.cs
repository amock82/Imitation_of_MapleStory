using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IPointerDownHandler
{
    // 아이템을 잡고 인벤토리 밖의 다른 UI와 겹치지 않게 드랍한 경우
    public void OnPointerDown(PointerEventData eventData)
    {
        if (SlotDrag.instance.GetIsTrackingMouse() == true)
        {
            if (SlotDrag.instance.slotDrag != null)
            {
                // 아이템이 2개 이상이면, 버릴 갯수를 결정하는 UI 출력
                DropInput.instance.Call();

                SlotDrag.instance.SetIsTrackingMouse(false);
            }
        }
    }
}
