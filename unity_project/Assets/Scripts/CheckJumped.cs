using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckJumped : MonoBehaviour
{
    public static CheckJumped   instance;
    
    public BoxCollider2D        _col2D;

    private void Awake()
    {
        _col2D = GetComponent<BoxCollider2D>();

        instance = this;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Player.instance.GetIsClimb() == true)
        {
            Player.instance.SetIsGround(false);
        }
        else if (collision.transform.tag == "Ground" || collision.transform.tag == "Terrain")
        {
            Player.instance.SetIsGround(true);

            if (collision.transform.tag == "Terrain")
            {
                Player.instance.SetIsTerrain(true);
            }
        }
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (Player.instance.GetIsClimb() == true)
    //    {
    //        Player.instance.SetIsGround(false);
    //    }
    //    else if (collision.transform.tag == "Ground" || collision.transform.tag == "Terrain")
    //    {
    //        Player.instance.SetIsGround(true);

    //        if (collision.transform.tag == "Terrain")
    //        {
    //            Player.instance.SetIsTerrain(true);
    //        }
    //    }
    //}

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground" || collision.transform.tag == "Terrain")
        {
            Player.instance.SetIsGround(false);

            if (collision.transform.tag == "Terrain")
            {
                Player.instance.SetIsTerrain(false);
            }
        }
    }
}
