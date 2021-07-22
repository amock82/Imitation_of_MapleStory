using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Consumable")]
// 소비 아이템 스크립트
public class ItemConsumable : ScriptableObject
{
    // 아이템 사용 유형
    public enum UseType
    { 
        HP = 0,
        MP
    }

    public int          effectAmount;   // 사용 효과량
    public UseType      useType;        // 사용 유형

    public void Use(float magnification = 1)
    {
        // 사용 유형에 따라 다른 작용
        if (useType == UseType.HP)
        {
            // 체력을 효과량만큼 증가
            Player.instance.AddCurHp((int)(effectAmount * magnification));
        }
        else if (useType == UseType.MP)
        {
            // 마나를 효과량만큼 증가
            Player.instance.AddCurMp((int)(effectAmount * magnification));
        }    
    }
}
