using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item         item;
    public Meso         meso;

    SpriteRenderer      _spriteRenderer; 

    Rigidbody2D         _rig;
    BoxCollider2D       _col;

    Transform           _target = null;         // 플레이어의 위치

    bool                isUp = true;            // 땅에 떨어진 아이템의 상하운동 상태
    bool                isLoot = false;         // 주워진 아이템은 True

    int                 count = 1;
    int                 mesoAmount = 10;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _rig = GetComponent<Rigidbody2D>();
        _col = GetComponent<BoxCollider2D>();

        //_rig.velocity = new Vector2(_rig.velocity.x, 5);
        _rig.AddForce(Vector2.up * 200, ForceMode2D.Force);

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
        if (_target != null)
        {
            Vector2 dir = _target.position - transform.position + Vector3.up * 0.5f;
            //dir.Normalize();

            transform.Translate(dir * 0.1f);
        }
        else
        {
            if (isUp == true)
            {
                _col.size += Vector2.up * 0.004f;

                if(_col.size.y > 0.46f)
                {
                    isUp = false;
                }
            }
            if (isUp == false)
            {
                _col.size -= Vector2.up * 0.003f;

                if (_col.size.y < 0.26f)
                {
                    isUp = true;
                }
            }
        }

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
            if (Input.GetButton("PickUp") && isLoot == false)
            {
                if (InputManager.instance.GetIsPickable())
                {
                    InputManager.instance.SetIsPickable(false);

                    isLoot = true;

                    if (item != null)
                    {
                        Inventory.instance.addItem(Instantiate(item), count);
                    }
                    else
                    {
                        Inventory.instance.SetAddMeso(mesoAmount);
                    }

                    _target = collision.transform;

                    _rig.gravityScale = 0;

                    Destroy(gameObject, 0.5f);
                }
            }
        }
    }

    public int GetCount()
    {
        return count;
    }

    public int GetMesoAmount()
    {
        return mesoAmount;
    }

    public void SetCount(int value)
    {
        count = value;
    }

    public void SetMesoAmount(int value)
    {
        mesoAmount = value;
    }
}
