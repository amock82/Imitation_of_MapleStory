using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect : MonoBehaviour
{
    public Skill        _skillPrefap;

    int multiTarget;

    private void Awake()
    {
        multiTarget = _skillPrefap.multiTarget;

        Player.instance.AddCurMp(-_skillPrefap.consumeMp);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyBody" && multiTarget > 0)
        {
            Enemy enemy = collision.GetComponentInParent<Enemy>();

            enemy.SetIsHit(true);
            enemy.direction = 0;

            int dir;

            if (GetComponentInParent<Transform>().rotation.y == 1)
            {
                dir = -1;
                enemy._spRen.flipX = true;
            }
            else
            {
                dir = 1;
                enemy._spRen.flipX = false;
            }

            if (enemy.GetCurHp() > Player.instance.GetAtk() * _skillPrefap.skillDamage / 100)
            {
                enemy._rig.AddForce(new Vector2(100 * dir, 0), ForceMode2D.Force);
            }
            
            if (enemy.GetCurHp() != 0)
            {
                enemy.OnDamage(Player.instance.GetAtk() * _skillPrefap.skillDamage / 100);

                multiTarget -= 1;

                enemy.SetTarget(Player.instance.transform); 
            }
        }
    }

    public void OnCol()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public void OffCol()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
