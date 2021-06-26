using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (Player.instance.GetHitTimer() <= 0)
            {
                Player.instance.OnDamage(10);
            }
        }
    }
}
