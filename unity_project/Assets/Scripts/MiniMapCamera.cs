using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    Transform _cameraTf;
    Transform _playerTF;

    SpriteRenderer _map;

    Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _cameraTf = this.transform;

        _map = GameObject.Find("MapImage").GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        _playerTF = Player.instance.transform;

        Debug.Log(_map.size);

        Debug.Log(_camera.sensorSize);
    }

    void Update()
    {
        _cameraTf.position = new Vector3(_playerTF.position.x, _playerTF.position.y, -10);


        // 맵에따라 다른 값이므로, 추후에 맵정보에서 받아올 수치들
        if (transform.position.x <= -7.3f)
        {
            transform.position = new Vector3(-7.3f, transform.position.y, -10);
        }

        if (transform.position.x >= 8.22f)
        {
            transform.position = new Vector3(8.22f, transform.position.y, -10);
        }

        if (transform.position.y >= 9.5f)
        {
            transform.position = new Vector3(transform.position.x, 9.5f, -10);
        }

        if (transform.position.y <= -7.5f)
        {
            transform.position = new Vector3(transform.position.x, -7.5f, -10);
        }
    }
}
