using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    Transform       _cameraTf;
    Transform       _playerTF;

    Vector3         offset = new Vector3(0, 1, -10);
    float           followSpeed = 0.1f;

    private void Awake()
    {
        _cameraTf = this.transform;
    }

    void Start()
    {
        _playerTF = Player.instance.transform;
    }

    private void FixedUpdate()
    {
        // 메인카메라는 플레이어를 조금 늦게 추적하게 만듬
        Vector3 cameraPos = Player.instance.transform.position + offset;
        Vector3 lerpPos = Vector3.Lerp(_cameraTf.position, cameraPos, followSpeed);

        _cameraTf.position = lerpPos;

        // 맵에따라 다른 값이므로, 추후에 맵정보에서 받아올 수치들
        if (transform.position.x <= -9.7f)
        {
            transform.position = new Vector3(-9.7f, transform.position.y, -10);
        }

        if (transform.position.x >= 9.7f)
        {
            transform.position = new Vector3(9.7f, transform.position.y, -10);
        }

        if (transform.position.y >= 11.4f)
        {
            transform.position = new Vector3(transform.position.x, 11.4f, -10);
        }

        if (transform.position.y <= -8.75f)
        {
            transform.position = new Vector3(transform.position.x, -8.75f, -10);
        }
    }
}
