using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float  default_Speed = 100;        // 기본 속도
    [SerializeField] float  max_Speed = 140;            // 최대 속도

    [SerializeField] float  default_JumpForce = 100;    // 기본 점프력
    [SerializeField] float  max_JumpForce = 123;        // 최대 점프력

    [SerializeField] public float   horForce = 80;      // 피격시 받는 위치변환정보
    [SerializeField] public float   verForce = 100;     // 피격시 받는 위치변환정보

    Rigidbody2D             _rig;                       // 플레이어 리지드바디
    BoxCollider2D           _col2D;                     // 플레이어의 피격판정 충돌체
    public Animator         _ani;                       // 플레이어 애니매이터

    private bool            isGround = false;           // 플래이어가 땅 위에있는가
    private bool            isTerrain = false;          // 플래이어가 지형 위에있는가
    private bool            isHit = false;              // 플레이어가 피격중인가
    private bool            isClimb = false;            // 플레이어가 사다리와 상호작용중인가
    private bool            isClimbCheck = false;       // 코루틴용 제어 플래그
    private bool            isClimbJumped = false;      // 사다리 이용중 점프시 0.3초간 True가 됨. 조건문용
    private bool            isDownJump = false;         // 플레이어가 하향점프중인가
    private bool            isJumped = false;           // 플레이어가 점프중인가
    private bool            isWall = false;             // 점프시 충돌체가 false되는 문제로 추가한 이동방지 플래그
    private bool            isWallRight = false;        // 인접한 벽이 플레이어의 오른쪽에 있는가

    float                   hitDelay = 1f;              // 피격시 무적시간
    float                   hitTimer = 0;               // 피격시 hitDelay로 설정됨 0이하가 되면 피격 가능

    float                   groundTimer = 0;            // 땅과 떨어져 있는 시간(사다리 이용과 애니매이션 문제로 추가)
    float                   climbTimer = 0;             // 사다리 상호작용 지속시간

    float                   move_Speed;                 // 이동속도. 플레이어 속도에 가공을 하여 완성됨

    int                     lv = 1;                     // 레벨

    int                     exp = 0;                    // 경험치
    int[]                   maxExp = new int[9] { 15, 34, 57, 92, 135, 372, 560, 840, 1242};// 10레벨까지의 경험치

    float                   maxHp = 50;                 // 최대체력
    float                   curHp = 50;                 // 현재체력

    float                   maxMp = 50;                 // 최대마나
    float                   curMp = 50;                 // 현재마나

    public static Player instance;                      // 싱글톤 기법

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
        ChangeAni();

        //타이머 제어
        if (hitTimer > 0)
        {
            hitTimer -= Time.deltaTime;
        }

        if (isGround)
            groundTimer = 0;
        else
            groundTimer += Time.deltaTime;

        if (isClimb == true)
        {
            climbTimer += Time.deltaTime;
        }
        else
        {
            climbTimer = 0;
            _ani.speed = 1;
        }

    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()     // 이동함수
    {
        float moveH = Input.GetAxis("Horizontal");      // 좌우 방향키 입력값
        float moveV = Input.GetAxisRaw("Vertical");     // 상하 방향키 입력값 (-1, 0, 1)

        if (isClimb == true)
        {  
            _rig.velocity = new Vector2(0, moveV * Time.deltaTime * move_Speed);


            if (climbTimer > 0.3f)
            {
                climbTimer = 0;

                if (moveV == 0)
                {
                    _ani.speed = 0;
                }
                else
                    _ani.speed = 1;
            }
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
            else    // 플레이어가 공중에 있는 경우, 방향키 입력으로 아주 미세하게 방향을 틀 수 있음
            {
                _rig.velocity = new Vector2(moveH * Time.deltaTime * move_Speed * 0.01f + _rig.velocity.x * 0.99f, _rig.velocity.y);
            }
        }

        if (isWall == true)     // 벽과 인접한 경우, 이동 제한
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

    private void Jump()     // 점프 함수
    {
        float moveH = Input.GetAxisRaw("Horizontal");       // 좌우 방향키 입력값 (-1, 0, 1)
        float moveV = Input.GetAxisRaw("Vertical");         // 상하 방향키 입력값 (-1, 0, 1)

        // 일반적인 점프/하향점프
        if (Input.GetButton("Jump") && isGround == true && isClimb == false && isClimbJumped == false)
        {
            if (moveV < 0 && isTerrain == true)
            {
                StartCoroutine(DownJump());
            }
            else if (isClimb == false)
            {
                StartCoroutine(JumpDelay());
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

    void ChangeAni()    // 추후 애니매이션 변경 코드를 모두 옮겨놓을 것
    {
        if(isClimb == true)
        {
            _ani.SetBool("IsClimb", true);
        }
        else
        {
            _ani.SetBool("IsClimb", false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)      // 데미지를 주는 작용은 적 객체의 기능으로 옮김
    {
        //if (collision.tag == "DamageZone" && hitTimer <= 0)     // 데미지를 입을 때의 작용
        //{
        //    isGround = false;            

        //    if (collision.transform.position.x < transform.position.x)
        //    {
        //        StartCoroutine(Hitjudgment(1, verForce, horForce));
        //        //_rig.velocity = new Vector2(Time.deltaTime * 100 - _rig.velocity.x * 100, Time.deltaTime * 100 + _rig.velocity.y);
        //    }

        //    if (collision.transform.position.x > transform.position.x)
        //    {
        //        //_rig.AddForce(new Vector2(Time.deltaTime * (-horForce) - _rig.velocity.x * 100, Time.deltaTime * verForce), ForceMode2D.Impulse);
        //        StartCoroutine(Hitjudgment(-1, verForce, horForce));
        //    }

        //    hitTimer = hitDelay;
        //}
    }

    public IEnumerator Hitjudgment(int directionX, float verForce = 120, float horForce = 80)
    {
        //_rig.AddForce(new Vector2(0, verForce), ForceMode2D.Force);
        if (verForce * 0.01f + _rig.velocity.y > 3)
        {
            _rig.velocity = new Vector2(_rig.velocity.x, 3);

        }
        else
        {
            _rig.velocity = new Vector2(_rig.velocity.x, verForce * 0.01f + _rig.velocity.y);
        }

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
            CheckJumped.instance._col2D.enabled = false;          
            isClimb = value;
        }
        else if (isClimbCheck == false)
        {
            isClimbCheck = true;

            yield return new WaitForSeconds(0.02f);
            CheckJumped.instance._col2D.enabled = false;
            isClimb = value;

            isClimbCheck = false;
        }
    }

    IEnumerator FallingDelay()
    {
        yield return new WaitForSeconds(0.3f);

        if (isGround == false && groundTimer > 0.3f)
            _ani.SetBool("IsHitFalling", true);
    }

    public void OnDamage(float damage)
    {
        curHp -= damage;

        hitTimer = hitDelay;

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

    public bool GetIsGround()
    {
        return isGround;
    }

    public float GetGroundTimer()
    {
        return groundTimer;
    }

    public void SetIsGround(bool value)
    {
        isGround = value;

        if (isGround == true)
        {
            _ani.SetBool("IsJump", false);
            _ani.SetBool("IsHitFalling", false);
        }

        if(isGround == false && isJumped == false)
        {
            StartCoroutine(FallingDelay());
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
            CheckJumped.instance._col2D.enabled = false;
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