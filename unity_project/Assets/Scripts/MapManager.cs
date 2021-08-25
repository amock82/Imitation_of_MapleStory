using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public string mapName;
    public string regionName;
    public Sprite mapIcon;

    public GameObject _monPrefep;       // 몬스터 객체 (원본)
    public Transform[] _respawnPoint;   // 몬스터 리스폰 위치 배열

    int monNum = 0;                     // 몬스터 마리 수

    float respawnTimer = 0;             // 몬스터 리스폰 타이머
    float respawnDelay = 10;            // 몬스터 리스폰 딜레이

    public static MapManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        // 타이머를 돌려서 딜레이마다 몬스터 리스폰
        respawnTimer -= Time.deltaTime;

        if (respawnTimer < 0)
        {
            MonRespawn();

            respawnTimer = respawnDelay;
        }
    }

    public void MonRespawn()
    {
        bool[] isRespawn = new bool[_respawnPoint.Length];  // 스폰 여부 부울값 (각 리스폰 포인트마다)

        // 스폰 여부 값 초기화
        for (int i = 0; i < _respawnPoint.Length; i++)
        {
            isRespawn[i] = false;
        }

        // 몬스터 수가 리스폰 위치 배열의 크기보다 작은 동안 반복
        for (; monNum < _respawnPoint.Length;)
        {
            // 랜덤값 받아오기 (0 ~ 리스폰 위치 배열 크기)
            int ran = Random.Range(0, _respawnPoint.Length);

            // 스폰 여부 값 배열의 ran번째가 false 인 경우 몬스터 리젠
            // 전체 스폰지점 에서 랜덤하게 몬스터가 리스폰 되게 함
            if (isRespawn[ran] == false)
            {
                // 몬스터 생성 후, 몬스터 수 1 증가
                Instantiate(_monPrefep, _respawnPoint[ran].position, _respawnPoint[ran].rotation);
                monNum++;

                isRespawn[ran] = true;
            }
        }
    }

    // 몬스터 사망시 호출, 몬스터 수 1 차감
    public void MonDie()
    {
        monNum--;
    }

    public string GetMapName()
    {
        return mapName;
    }

    public string GetRegionName()
    {
        return regionName;
    }
}
