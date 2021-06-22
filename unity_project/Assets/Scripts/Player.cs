using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float  default_Speed = 100;
    [SerializeField] float  max_Speed = 140;

    [SerializeField] float  default_JumpForce = 100;
    [SerializeField] float  max_JumpForce = 123;

    Rigidbody2D             _rig;
    BoxCollider2D           _col2D;

    private bool            isGround = false;

    float                   hitDelay = 0.7f;
    float                   hitTimer = 0;

    public static Player instance;

    private void Awake()
    {
        _rig = GetComponent<Rigidbody2D>();
        _col2D = GetComponent<BoxCollider2D>();

        instance = this;
    }

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Move();
        Jump();

        if( hitTimer > 0)
        {
            hitTimer -= Time.deltaTime;
        }
    }

    private void Move()
    {
        if (isGround == true)
        {
            float moveH = Input.GetAxis("Horizontal");

            _rig.velocity = new Vector2(moveH * Time.deltaTime * default_Speed, _rig.velocity.y);
        }
    }

    private void Jump()
    {
        if (Input.GetButton("Jump") && isGround == true)
        {
            _rig.velocity = new Vector2(_rig.velocity.x, Time.deltaTime * default_JumpForce * 3.2f);

            isGround = false;       
        }

        if (_rig.velocity.y > 0)
        {
            CheckJumped.instance._col2D.enabled = false;
        }
        else
        {
            CheckJumped.instance._col2D.enabled = true;
        }

        //Debug.Log(_rig.velocity.y);
    }

    public void setIsJumped(bool value)
    {
        isGround = value;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "DamageZone" && hitTimer <= 0)
        {
            if (collision.transform.position.x < transform.position.x)
            {
                _rig.velocity = new Vector2(Time.deltaTime * 100, Time.deltaTime * 100);
            }

            if (collision.transform.position.x > transform.position.x)
            {
                _rig.velocity = new Vector2(Time.deltaTime * (-100), Time.deltaTime * 100);
            }
            isGround = false;
            hitTimer = hitDelay;
        }
    }
}
