using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    BoxCollider2D       _col;

    private void Awake()
    {
        _col = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "CheckJump")       // 플레이어가 사다리에 매달리는 판정을 검사
        {
            float moveV = Input.GetAxisRaw("Vertical");     

            if ((moveV > 0  && (Player.instance.GetGroundTimer() > 1.5f || _col.offset.y > Player.instance.transform.position.y)) || (moveV < 0 && Player.instance.GetIsGround() == true)) 
            {
                Player.instance.SetIsClimbCrt(true);

                CheckJumped.instance._col2D.enabled = false;
            }

            if (Player.instance.GetIsClimb())
            {
                Player.instance.transform.position = new Vector2(GetComponent<BoxCollider2D>().offset.x, Player.instance.transform.position.y);
            }
            //Debug.Log(_col.offset.y + " / " + Player.instance.transform.position.y);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "CheckJump" && Player.instance.GetIsClimb() == true)   //플레이어가 사다리에서 벗어났을 때, 상태 변경
        {
            Player.instance.SetIsClimbCrt(false);
            Player.instance._ani.SetBool("IsClimb", false);

            Rigidbody2D _rig = Player.instance.GetComponent<Rigidbody2D>();

            _rig.velocity = new Vector2(_rig.velocity.x, 0);
        }
    }
}
