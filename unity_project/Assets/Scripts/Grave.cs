using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grave : MonoBehaviour
{
    BoxCollider2D       _col;
    public Vector3      startPos;

    private void Awake()
    {
        _col = GetComponent<BoxCollider2D>();

        _col.enabled = false;
    }

    void Update()
    {
        if (startPos.y - 4.9f < transform.position.y)
        {
            _col.enabled = false;
        }
        else
        {
            _col.enabled = true;
        }
    }
}
