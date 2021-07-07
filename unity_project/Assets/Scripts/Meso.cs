using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Meso")]
public class Meso : ScriptableObject
{
    public int mesoAmount;
    public Sprite mesoImage;

    public Meso(int meso)
    {
        mesoAmount = meso;
    }
}
