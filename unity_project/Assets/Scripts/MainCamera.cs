using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    Transform           _cameraTf;
    Transform           _playerTF;
    
    private void Awake()
    {
        _cameraTf = this.transform;      
    }

    void Start()
    {
        _playerTF = Player.instance.transform;
    }

    void Update()
    {
        _cameraTf.position = new Vector3(_playerTF.position.x, _playerTF.position.y + 3, -10);
    }
}
