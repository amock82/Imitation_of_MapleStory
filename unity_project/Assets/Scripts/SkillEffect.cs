using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect : MonoBehaviour
{
    public Skill        _skillPrefap;       // 스킬 정보

    int                 multiTarget;        // 최대 타겟수

    private void Awake()
    {
        multiTarget = _skillPrefap.multiTarget;

        Player.instance.AddCurMp(-_skillPrefap.consumeMp);  // 스킬 MP 소모량만큼 플레이어의 MP를 소모시킴
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 적과 닿았고, 이 스킬이 최대 공격가능한 몬스터보다 적게 명중시켰을 경우
        if (collision.tag == "EnemyBody" && multiTarget > 0)
        {
            Enemy enemy = collision.GetComponentInParent<Enemy>();

            enemy.SetIsHit(true);   // 적의 애니매이션 변경 플래그
            enemy.direction = 0;    // 적 이동방향 0으로 (경직)

            int dir;                // 적을 밀쳐내는 방향

            // 스킬이 시전된 방향을 기준으로 적중된 적이 밀쳐진 방향과 바라보는 방향을 추정
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

            // 적의 체력이 가해지는 데미지 보다 높다면 넉백
            if (enemy.GetCurHp() > Player.instance.GetAtk() * _skillPrefap.skillDamage / 100)
            {
                enemy._rig.AddForce(new Vector2(100 * dir, 0), ForceMode2D.Force);
            }
            
            // 적의 체력이 0이 아닐 경우(이미 죽은 적이 아닌 경우에만) 데미지를 주고 적중가능 타겟수를 1개 줄인다.
            if (enemy.GetCurHp() != 0)
            {
                enemy.OnDamage(Player.instance.GetAtk() * _skillPrefap.skillDamage / 100);

                multiTarget -= 1;

                enemy.SetTarget(Player.instance.transform);     // 적의 타겟을 플래이어로 지정
            }
        }
    }

    // 애니매이션 이벤트에서 사용, 스킬 이미지에 따른 Collider의 On/Off
    public void OnCol()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public void OffCol()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
