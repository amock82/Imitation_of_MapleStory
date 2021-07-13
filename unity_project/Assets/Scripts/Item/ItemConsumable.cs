using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Consumable")]
public class ItemConsumable : ScriptableObject
{
    public enum UseType
    { 
        HP = 0,
        MP
    }

    public int          effectAmount;
    public UseType      useType;

    public void Use(float magnification = 1)
    {
        if (useType == UseType.HP)
        {
            Player.instance.AddCurHp((int)(effectAmount * magnification));
        }
        else if (useType == UseType.MP)
        {
            Player.instance.AddCurMp((int)(effectAmount * magnification));
        }    
    }
}
