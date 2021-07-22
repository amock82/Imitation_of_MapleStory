using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using UnityEngine;

// 아이템을 주울 수 있게 아이템 객체에 추가한 스크립트
public class ItemPickUp : MonoBehaviour
{
    // 둘중 하나만 사용됨, item과 meso의 정보가 전부 들어있다면, item이 우선됨
    public Item         item;                   // 아이템 정보
    public Meso         meso;                   // 아이템(메소) 정보

    SpriteRenderer      _spriteRenderer;        // 스프라이트 렌더러

    Rigidbody2D         _rig;                   // 아이템 리지드바디
    BoxCollider2D       _col;                   // 아이템 콜라이더

    Transform           _target = null;         // 플레이어의 위치

    bool                isUp = true;            // 땅에 떨어진 아이템의 상하운동 상태
    bool                isLoot = false;         // 주워진 아이템은 True

    int                 count = 1;              // 아이템 갯수
    int                 mesoAmount = 10;        // 메소량

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _rig = GetComponent<Rigidbody2D>();
        _col = GetComponent<BoxCollider2D>();

        _rig.AddForce(Vector2.up * 200, ForceMode2D.Force);

        // 스프라이트를 아이템의 이미지로 셋팅
        if (item != null)
        {
            _spriteRenderer.sprite = item.itemImage;
        }
        else
        {
            _spriteRenderer.sprite = meso.mesoImage;
        }
    }

    private void Update()
    {
        // target은 아이템을 줍기가 실행되면 셋팅됨
        // target이 존재하면
        if (_target != null)
        {
            Vector2 dir = _target.position - transform.position + Vector3.up * 0.5f;

            // 아이템을 플레이어쪽으로 이동
            transform.Translate(dir * Time.deltaTime * 3);
        }
        else
        {
            // 떨어진 아이템을 상하운동하게 만듬
            // 실제로는 객체를 이동시키는 것이 아니라, 
            // 콜라이더의 Size.y를 조절하여 상하운동하는것처럼 보이게 함
            if (isUp == true)
            {
                _col.size += Vector2.up * Time.deltaTime * 0.2f;

                if(_col.size.y > 0.46f)
                {
                    isUp = false;
                }
            }
            if (isUp == false)
            {
                _col.size -= Vector2.up * Time.deltaTime * 0.2f;

                if (_col.size.y < 0.26f)
                {
                    isUp = true;
                }
            }
        }

        // 아이템이 드랍될 때, 힘이 가해지는 방향은 위쪽,
        // 아이템이 하강운동할 때에 콜라이더를 켜서 바닥과 충돌할 수 있게 함.
        // 아이템이 드롭될 때, 천장(지형)에 부딪히는 것을 방지
        if (_rig.velocity.y > 0)
        {
            _col.enabled = false;
        }
        else
        {
            _col.enabled = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // 대상의 태그가 Player이며 / PickUp버튼이 눌리고 / 이 아이템이 주워진 상태가 아니라면
            if (Input.GetButton("PickUp") && isLoot == false)
            {
                if (InputManager.instance.GetIsPickable())
                {
                    // InputManager에는 아이템을 줍는데 딜레이를 설정함. 관련함수
                    InputManager.instance.SetIsPickable(false);

                    // 아이템이 주워짐
                    isLoot = true;

                    // 아이템을 인벤토리에 추가. (메소일 경우, 보유한 메소에 추가)
                    if (item != null)
                    {
                        Inventory.instance.addItem(Instantiate(item), count);
                    }
                    else
                    {
                        Inventory.instance.SetAddMeso(mesoAmount);
                    }
                    
                    // 아이템 객체가 따라다닐 타겟 설정
                    _target = collision.transform;

                    // 중력 영향 해제
                    _rig.gravityScale = 0;

                    // 0.5초후에 객체를 파괴
                    Destroy(gameObject, 0.5f);
                }
            }
        }
    }

    // 아이템 갯수 반환
    public int GetCount()
    {
        return count;
    }

    // 메소량 반환
    public int GetMesoAmount()
    {
        return mesoAmount;
    }

    // 아이템 갯수 설정
    public void SetCount(int value)
    {
        count = value;
    }

    // 메소량 설정
    public void SetMesoAmount(int value)
    {
        mesoAmount = value;
    }
}
