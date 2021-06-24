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
    Animator                _ani;

    private bool            isGround = false;
    private bool            isHit = false;
    private bool            isclimb = false;

    float                   hitDelay = 1f;
    float                   hitTimer = 0;

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

    }

    void Update()
    {
        if (hitTimer > 0)
        {
            hitTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        Move();
        Jump();

    }

    private void Move()
    {
        float moveH = Input.GetAxis("Horizontal");
        float moveV = Input.GetAxis("Vertical");

        if (isGround == true && isclimb == false)
        {
            _rig.velocity = new Vector2(moveH * Time.deltaTime * default_Speed, _rig.velocity.y);

            if (moveH != 0)
            {
                _ani.SetBool("IsMove", true);
            }
            else
            {
                _ani.SetBool("IsMove", false);
            }

            if(moveH > 0)
            {
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            else if (moveH < 0)
            {
                transform.rotation = new Quaternion(0, 180, 0, 0);
            }
        }
        else if (isGround == false && isclimb == false)
        {
            _rig.velocity = new Vector2(moveH * Time.deltaTime * default_Speed * 0.005f + _rig.velocity.x, _rig.velocity.y);
        }
        else if (isclimb == true)
        {
            _rig.velocity = new Vector2(0, moveV * Time.deltaTime * default_Speed);

            CheckJumped.instance._col2D.enabled = false;
        }
    }

    private void Jump()
    {
        float moveH = Input.GetAxis("Horizontal");

        if (moveH > 0)
        {
            moveH = 1;
        }
        else if(moveH < 0)
        {
            moveH = -1;
        }
        

        if (Input.GetButton("Jump") && isGround == true && isclimb == false)
        {
            _rig.velocity = new Vector2(_rig.velocity.x, default_JumpForce * 0.05f);
            _ani.SetBool("IsJump", true);

            isGround = false;       
        }

        if (Input.GetButton("Jump") && isclimb == true && moveH != 0)
        {
            isclimb = false;

            _rig.velocity = new Vector2(2 * moveH, default_JumpForce * 0.025f);
        }

        // 점프중에 바닥의 콜라이더에 막히는것을 방지
        if ((_rig.velocity.y > 0 && isGround == false) || isclimb == true)
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
        if (collision.tag == "DamageZone" && hitTimer <= 0)
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

                Debug.Log(Time.deltaTime * verForce);
            }

            hitTimer = hitDelay;
        }

        if (collision.tag == "Ladder")
        {
            float moveV = Input.GetAxis("Vertical");

            if (moveV > 0 || moveV < 0)
            {
                isclimb = true;

                transform.position = new Vector2(collision.GetComponent<BoxCollider2D>().offset.x, transform.position.y);

                CheckJumped.instance._col2D.enabled = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ladder" && isclimb == true)
        {
            isclimb = false;

            _rig.velocity = new Vector2(_rig.velocity.x, 0);
        }
    }

    IEnumerator Hitjudgment(int directionX, float verForce = 120, float horForce = 80)
    {
        _rig.AddForce(new Vector2(0, verForce), ForceMode2D.Force);

        yield return new WaitForSeconds(0.05f);

        _rig.AddForce(new Vector2(horForce * directionX, 0), ForceMode2D.Force);
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
        return isclimb;
    }

    public void setIsGround(bool value)
    {
        isGround = value;

        if (isGround == true)
        {
            _ani.SetBool("IsJump", false);
        }
    }
}
