using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grave : MonoBehaviour
{
    BoxCollider2D       _col;       // 충돌체
    public Vector3      startPos;   // 비석 생성 위치

    private void Awake()
    {
        _col = GetComponent<BoxCollider2D>();

        _col.enabled = false;   // 충돌체 off
    }

    void Update()
    {
        // 비석 생성 위치로부터 일정 거리 이상 떨어지면 충돌체 On
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
