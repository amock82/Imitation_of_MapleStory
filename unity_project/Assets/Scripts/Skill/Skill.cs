using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "New Skill/Skill")]
public class Skill : ScriptableObject
{
    // 스킬 유형 열거형 변수
    public enum SkillType
    {
        Active = 0, // 엑티브 스킬
        Passive,    // 패시브 스킬
        Buff        // 버프형 스킬
    }

    public string               skillName;      // 스킬 이름
    public SkillType            skillType;      // 스킬 유형
    public Sprite               skillImage;     // 스킬 이미지
    [TextArea] public string    skillDetail;    // 스킬 설명

    public GameObject           skillPrefab;    // 스킬 객체 

    public int                  multiTarget;    // 최대 타겟수
    public int                  consumeMp;      // MP소모량
    public float                skillDamage;    // 스킬 데미지 계수
}
