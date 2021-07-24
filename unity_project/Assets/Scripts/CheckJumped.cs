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

    private void OnCollisionEnter2D(Collision2D collision)  // 플레이어의 바닥충돌체의 조건에 따른 ON/OFF
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground" || collision.transform.tag == "Terrain")
        {
            Player.instance.SetIsGround(true);

            if (collision.transform.tag == "Terrain")
            {
                Player.instance.SetIsTerrain(true);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)  // 충돌에서 벗어나는 대상이 바닥/지형이면 플레이어 바닥 충돌체 OFF
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
