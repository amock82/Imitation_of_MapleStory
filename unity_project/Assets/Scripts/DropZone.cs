using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if (SlotDrag.instance.GetIsTrackingMouse() == true)
        {
            if (SlotDrag.instance.slotDrag != null)
            {
                DropInput.instance.Call();

                SlotDrag.instance.SetIsTrackingMouse(false);
            }
        }
    }
}
