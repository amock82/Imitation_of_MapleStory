using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Player.instance.transform.position.x < GetComponent<BoxCollider2D>().offset.x)
            {
                Player.instance.SetIsWall(true, true);
            }
            else
            {
                Player.instance.SetIsWall(true, false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player.instance.SetIsWall(false);
        }
    }
}
