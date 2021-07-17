using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "New Skill/Skill")]
public class Skill : ScriptableObject
{
    public enum SkillType
    {
        Active = 0,
        Passive,
        Buff
    }

    public string               skillName;
    public SkillType            skillType;
    public Sprite               skillImage;
    [TextArea] public string    skillDetail;

    public GameObject           skillPrefab;

    public int                  multiTarget;
    public int                  consumeMp;
    public float                skillDamage;
}
