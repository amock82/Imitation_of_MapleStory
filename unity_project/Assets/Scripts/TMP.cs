using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TMP : MonoBehaviour
{
    float           moveSpeed;
    float           alphaSpeed;
    float           lifeTime = 1.0f;

    Color           alpha;

    void Start()
    {
        moveSpeed = 0.2f;
        alphaSpeed = 2.0f;

        alpha = GetComponent<TextMeshProUGUI>().color;

        Destroy(this.gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));

        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        GetComponent<TextMeshProUGUI>().color = alpha;
    }
}
