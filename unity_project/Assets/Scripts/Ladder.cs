using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    void Start()
    {
        
    }
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "CheckJump")
        {
            float moveV = Input.GetAxisRaw("Vertical");

            if (moveV > 0 || moveV < 0)
            {
                Player.instance.SetIsClimb(true);

                Player.instance.transform.position = new Vector2(GetComponent<BoxCollider2D>().offset.x, Player.instance.transform.position.y);

                CheckJumped.instance._col2D.enabled = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "CheckJump" && Player.instance.GetIsClimb() == true)
        {
            Player.instance.SetIsClimb(false);

            Rigidbody2D _rig = Player.instance.GetComponent<Rigidbody2D>();

            _rig.velocity = new Vector2(_rig.velocity.x, 0);
        }
    }
}
