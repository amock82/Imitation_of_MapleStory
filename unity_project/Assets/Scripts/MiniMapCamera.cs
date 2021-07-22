using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    Transform _cameraTf;    // 카메라의 Transform 정보
    Transform _playerTF;    // 플레이어의 Transform 정보

    SpriteRenderer _map;    // 맵 이미지

    Camera _camera;         // 미니맵 카메라      

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _cameraTf = this.transform;

        _map = GameObject.Find("MapImage").GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        _playerTF = Player.instance.transform;
    }

    void Update()
    {
        // 미니맵카메라의 위치는 플레이어의 위치를 기반으로 추적
        _cameraTf.position = new Vector3(_playerTF.position.x, _playerTF.position.y, -10);


        // 맵에따라 다른 값이므로, 추후에 맵정보에서 받아올 수치들 (반영안됨 - 맵이 한개라 필요하지 않은 작업)
        if (transform.position.x <= 0)
        {
            transform.position = new Vector3(0, transform.position.y, -10);
        }

        if (transform.position.x >= 0)
        {
            transform.position = new Vector3(0, transform.position.y, -10);
        }

        if (transform.position.y >= 4.5f)
        {
            transform.position = new Vector3(transform.position.x, 4.5f, -10);
        }

        if (transform.position.y <= -4.15f)
        {
            transform.position = new Vector3(transform.position.x, -4.15f, -10);
        }
    }
}
