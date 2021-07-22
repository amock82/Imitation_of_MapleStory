using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Meso")]
public class Meso : ScriptableObject
{
    public int mesoAmount;      // 메소량
    public Sprite mesoImage;    // 메소 이미지

    // 메소 생성자 (메소량 지정)
    public Meso(int meso)
    {
        mesoAmount = meso;
    }
}
