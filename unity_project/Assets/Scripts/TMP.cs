using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// TextMeshPro 스크립트
public class TMP : MonoBehaviour
{
    float           moveSpeed;          // 이동속도
    float           alphaSpeed;         // 투명화 속도
    float           lifeTime = 1.0f;    // 라이프타임 (라이프타임이 지나면 객체 삭제)

    Color           alpha;              // 색상 저장값

    void Start()
    {
        moveSpeed = 0.2f;
        alphaSpeed = 2.0f;

        alpha = GetComponent<TextMeshProUGUI>().color;

        // 라이프타임 이후에 삭제
        Destroy(this.gameObject, lifeTime);
    }

    void Update()
    {
        // 위로 이동속도에 기반하여 이동
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));

        // 투명화 속도에 기반하여 Text색상 투명화
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        GetComponent<TextMeshProUGUI>().color = alpha;
    }
}
