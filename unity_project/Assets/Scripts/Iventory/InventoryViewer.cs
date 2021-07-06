using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryViewer : MonoBehaviour
{
    public Slot[] _slot;

    private void Awake()
    {
        _slot = GetComponentsInChildren<Slot>();
    }
}
