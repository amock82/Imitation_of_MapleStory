using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    Transform       _cameraTf;
    Transform       _playerTF;

    Vector3         offset = new Vector3(0, 2, -10);
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
        Vector3 cameraPos = Player.instance.transform.position + offset;
        Vector3 lerpPos = Vector3.Lerp(_cameraTf.position, cameraPos, followSpeed);

        _cameraTf.position = lerpPos;

        // 맵에따라 다른 값이므로, 추후에 맵정보에서 받아올 수치들
        if (transform.position.x <= -10.65f)
        {
            transform.position = new Vector3(-10.65f, transform.position.y, -10);
        }

        if (transform.position.x >= 10.7f)
        {
            transform.position = new Vector3(10.7f, transform.position.y, -10);
        }

        if (transform.position.y >= 11f)
        {
            transform.position = new Vector3(transform.position.x, 11f, -10);
        }

        if (transform.position.y <= -8.6f)
        {
            transform.position = new Vector3(transform.position.x, -8.6f, -10);
        }
    }
}
