using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public enum ItemType        // 아이템 유형 열거형
    {
        Equipment,      // 장비
        Consumable,     // 소비
        Loot,           // 기타
        SetUp,          // 설치
        Cash,           // 캐시
        Cosmetic        // 치장
    }

    public string       itemName;           // 아이템 이름
    public ItemType     itemType;           // 아이템 유형
    public Sprite       itemImage;          // 아이템 이미지
    public GameObject   itemPrefab;         // 아이템 객체

    public int          itemAmount;         // 아이템 갯수
    public int          itemMaxAmount;      // 아이템 최대 갯수
}
