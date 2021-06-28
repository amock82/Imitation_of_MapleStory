using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{


    private void OnTriggerStay2D(Collider2D collision)
    {
        Player instance = Player.instance;

        if(collision.tag == "Player")
        {
            if (Player.instance.GetHitTimer() <= 0)
            {
                //instance.SetIsGround(false);

                if (collision.transform.position.x < transform.position.x)
                {
                    instance.StartCoroutine(instance.Hitjudgment(-1, instance.verForce, instance.horForce));
                    //_rig.velocity = new Vector2(Time.deltaTime * 100 - _rig.velocity.x * 100, Time.deltaTime * 100 + _rig.velocity.y);
                }

                if (collision.transform.position.x > transform.position.x)
                {
                    //_rig.AddForce(new Vector2(Time.deltaTime * (-horForce) - _rig.velocity.x * 100, Time.deltaTime * verForce), ForceMode2D.Impulse);
                    instance.StartCoroutine(instance.Hitjudgment(1, instance.verForce, instance.horForce));
                }

                //hitTimer = hitDelay;

                instance.OnDamage(10);
            }
        }
    }
}
