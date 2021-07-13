using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject _monPrefep;
    public Transform[] _respawnPoint;

    int monNum = 0;

    float respawnTimer = 0;
    float respawnDelay = 10;

    public static MapManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        respawnTimer -= Time.deltaTime;

        if (respawnTimer < 0)
        {
            MonRespawn();

            respawnTimer = respawnDelay;
        }
    }

    public void MonRespawn()
    {
        bool[] isRespawn = new bool[_respawnPoint.Length];

        for (int i = 0; i < _respawnPoint.Length; i++)
        {
            isRespawn[i] = false;
        }

        for (; monNum < _respawnPoint.Length;)
        {
            int ran = Random.Range(0, _respawnPoint.Length);

            if (isRespawn[ran] == false)
            {
                Instantiate(_monPrefep, _respawnPoint[ran].position, _respawnPoint[ran].rotation);
                monNum++;

                isRespawn[ran] = true;
            }
        }
    }

    public void MonDie()
    {
        monNum--;
    }
}
