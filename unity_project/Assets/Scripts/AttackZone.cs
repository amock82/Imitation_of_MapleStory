using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    // 최대 공격 가능 수
    public int multiTarget = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 접촉한 대상이 EnemyBody태그를 가지고 있으며, 공격 가능 수의 잔여가 남아있을 경우
        if(collision.tag == "EnemyBody" && multiTarget > 0)
        {
            Enemy enemy = collision.GetComponentInParent<Enemy>();

            // 적중한 적의 피격처리
            enemy.SetIsHit(true);
            enemy.direction = 0;

            int dir;

            // 공격 방향에 따른 적의 스프라이트, 넉백 방향 결정
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

            // 공격 데미지와 적의 체력을 비교하여, 적의 여분 체력이 더 적으면 넉백하지 않음
            if (enemy.GetCurHp() > Player.instance.GetAtk())
            {
                enemy._rig.AddForce(new Vector2(100 * dir, 0), ForceMode2D.Force);
            }
            
            if (enemy.GetCurHp() != 0)
            {
                enemy.OnDamage(Player.instance.GetAtk());

                multiTarget -= 1;

                enemy.SetTarget(Player.instance.transform);
            }
        }
    }
}
