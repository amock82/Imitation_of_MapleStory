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

    [SerializeField] float  horForce = 80;
    [SerializeField] float  verForce = 100;

    Rigidbody2D             _rig;
    BoxCollider2D           _col2D;
    public Animator         _ani;

    private bool            isGround = false;
    private bool            isTerrain = false;
    private bool            isHit = false;
    private bool            isClimb = false;
    private bool            isClimbCheck = false;
    private bool            isClimbJumped = false;
    private bool            isDownJump = false;
    private bool            isJumped = false;
    private bool            isWall = false;
    private bool            isWallRight = false;

    float                   hitDelay = 1f;
    float                   hitTimer = 0;

    float                   move_Speed;

    int                     lv = 1;

    int                     exp = 0;
    int[]                   maxExp = new int[9] { 15, 34, 57, 92, 135, 372, 560, 840, 1242};

    float                   maxHp = 50;
    float                   curHp = 50;

    float                   maxMp = 50;
    float                   curMp = 50;

    public static Player instance;

    private void Awake()
    {
        _rig = GetComponent<Rigidbody2D>();
        _col2D = GetComponent<BoxCollider2D>();
        _ani = GetComponent<Animator>();

        instance = this;
    }

    void Start()
    {
        move_Speed = 0.8f * default_Speed;
    }

    void Update()
    {
        Jump();

        if (hitTimer > 0)
        {
            hitTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float moveH = Input.GetAxis("Horizontal");
        float moveV = Input.GetAxisRaw("Vertical");

        if (isClimb == true)
        {
            _rig.velocity = new Vector2(0, moveV * Time.deltaTime * move_Speed);

            CheckJumped.instance._col2D.enabled = false;
        }
        else
        {
            if (moveH > 0)
            {
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            else if (moveH < 0)
            {
                transform.rotation = new Quaternion(0, 180, 0, 0);
            }

            if (isGround == true && isClimbJumped == false)
            {
                _rig.velocity = new Vector2(moveH * Time.deltaTime * move_Speed, _rig.velocity.y);

                if (moveH != 0)
                {
                    _ani.SetBool("IsMove", true);
                }
                else
                {
                    _ani.SetBool("IsMove", false);
                }
            }
            else
            {
                _rig.velocity = new Vector2(moveH * Time.deltaTime * move_Speed * 0.01f + _rig.velocity.x * 0.99f, _rig.velocity.y);
            }
        }

        if (isWall == true)
        {
            if (isWallRight == true)
            {
                if (_rig.velocity.x > 0)
                    _rig.velocity = new Vector2(0, _rig.velocity.y);
            }
            else
            {
                if (_rig.velocity.x < 0)
                    _rig.velocity = new Vector2(0, _rig.velocity.y);
            }
        }

    }

    private void Jump()
    {
        float moveH = Input.GetAxisRaw("Horizontal");
        float moveV = Input.GetAxisRaw("Vertical");

        if (Input.GetButton("Jump") && isGround == true && isClimb == false && isClimbJumped == false)
        {
            if (moveV < 0 && isTerrain == true)
            {
                StartCoroutine(DownJump());
            }
            else if (isClimb == false)
            {
                StartCoroutine(JumpDelay());
                //_rig.velocity = new Vector2(_rig.velocity.x, default_JumpForce * 0.04f);
                
                /*
                 * _rig.AddForce(new Vector2(0, default_JumpForce * 0.1f), ForceMode2D.Impulse);
                _ani.SetBool("IsJump", true);

                isGround = false;
                */
            }
        }

        if (Input.GetButtonDown("Jump") && isClimb == true && moveH != 0)
        {
            SetIsClimb(false);
            StartCoroutine(JumpClimb());

            _rig.velocity = new Vector2(2 * moveH, default_JumpForce * 0.020f);
        }

        // 점프중에 바닥의 콜라이더에 막히는것을 방지
        if ((_rig.velocity.y > 0 && isGround == false) || isClimb == true || isDownJump == true || isClimbJumped == true)
        {
            CheckJumped.instance._col2D.enabled = false;
        }
        else
        {
            CheckJumped.instance._col2D.enabled = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "DamageZone" && hitTimer <= 0)     // 데미지를 입을 때의 작용
        {
            isGround = false;            

            if (collision.transform.position.x < transform.position.x)
            {
                StartCoroutine(Hitjudgment(1));
                //_rig.velocity = new Vector2(Time.deltaTime * 100 - _rig.velocity.x * 100, Time.deltaTime * 100 + _rig.velocity.y);
            }

            if (collision.transform.position.x > transform.position.x)
            {
                //_rig.AddForce(new Vector2(Time.deltaTime * (-horForce) - _rig.velocity.x * 100, Time.deltaTime * verForce), ForceMode2D.Impulse);
                StartCoroutine(Hitjudgment(-1));
            }

            hitTimer = hitDelay;
        }
    }

    IEnumerator Hitjudgment(int directionX, float verForce = 120, float horForce = 80)
    {
        _rig.AddForce(new Vector2(0, verForce), ForceMode2D.Force);

        yield return new WaitForSeconds(0.05f);

        _rig.AddForce(new Vector2(horForce * directionX, 0), ForceMode2D.Force);
    }

    IEnumerator DownJump()
    {
        if (isDownJump == false)
        {
            isDownJump = true;
            _rig.velocity = new Vector2(0, default_JumpForce * 0.02f);

            yield return new WaitForSeconds(0.5f);

            isDownJump = false;
            CheckJumped.instance._col2D.enabled = true;
        }
    }

    IEnumerator JumpDelay()
    {
        if (isJumped == false)
        {
            isJumped = true;

            yield return new WaitForSeconds(0.02f);

            _rig.velocity = new Vector2(_rig.velocity.x, default_JumpForce * 0.04f);
            //_rig.AddForce(new Vector2(0, default_JumpForce * 0.08f), ForceMode2D.Impulse);
            _ani.SetBool("IsJump", true);

            isGround = false;
            isJumped = false;
        }
    }

    IEnumerator JumpClimb()
    {
        if(isClimbJumped == false)
        {
            isClimbJumped = true;

            yield return new WaitForSeconds(0.3f);

            isClimbJumped = false;
        }
    }

    IEnumerator ClimbDelay(bool value)
    {
        if (value == false)
        {
            yield return new WaitForSeconds(0.02f);
            isClimb = value;
        }
        else if (isClimbCheck == false)
        {
            isClimbCheck = true;

            yield return new WaitForSeconds(0.02f);
            isClimb = value;

            isClimbCheck = false;
        }
    }

    public void OnDamage(float damage)
    {
        curHp -= damage;

        if (curHp <= 0)
        {
            curHp = 0;
        }
    }

    public int GetLv()
    {
        return lv;
    }

    public int GetExp()
    {
        return exp;
    }

    public int GetMaxExp(int lv)
    {
        return maxExp[lv];
    }

    public float GetHp()
    {
        return curHp;
    }

    public float GetMaxHp()
    {
        return maxHp;
    }

    public float GetMp()
    {
        return curMp;
    }

    public float GetMaxMp()
    {
        return maxMp;
    }

    public float GetHitTimer()
    {
        return hitTimer;
    }

    public bool GetIsClimb()
    {
        return isClimb;
    }

    public void SetIsGround(bool value)
    {
        isGround = value;

        if (isGround == true)
        {
            _ani.SetBool("IsJump", false);
        }

    }

    public void SetIsTerrain(bool value)
    {
        isTerrain = value;
    }

    public void SetIsClimb(bool value)
    {
        isClimb = value;

        if (value == true)
        {
            _rig.gravityScale = 0;

        }
        else
        {
            _rig.gravityScale = 1;
        }
    }

    public void SetIsClimbCrt(bool value)
    {
        if (value == true)
        {
            _rig.gravityScale = 0;

            StartCoroutine(ClimbDelay(value));
        }
        else
        {
            _rig.gravityScale = 1;

            StartCoroutine(ClimbDelay(value));
        }
    }

    public void SetIsWall(bool value, bool value2 = true)  // 벽에 닿은 상태, 벽이 오른쪽에 있는지
    {
        isWall = value;

        isWallRight = value2;
    }
}