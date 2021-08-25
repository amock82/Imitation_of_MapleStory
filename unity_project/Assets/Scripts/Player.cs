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

    Rigidbody2D                 _rig;                       // 플레이어 리지드바디
    BoxCollider2D               _col2D;                     // 플레이어의 피격판정 충돌체
    public Animator             _ani;                       // 플레이어 애니매이터
    GameObject                  _atkZone;                   // 공격판정 범위 오브젝트
    public GameObject           _grave;                     // 비석 오브젝트 (사망시 만들어냄)

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
    private bool            isAttack = false;           // 공격중인가
    private bool            isDie = false;              // 사망했는가
    private bool            isUseSkill = false;         // 스킬 사용중인가

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

    float                   atk = 10;                   // 공격력
    float                   def;                        // 방어력

    GameObject              grave;                      // 사망시 생성되는 비석 객체

    public static Player instance;                      // 싱글톤 기법

    private void Awake()
    {
        _rig = GetComponent<Rigidbody2D>();
        _col2D = GetComponent<BoxCollider2D>();
        _ani = GetComponent<Animator>();

        _atkZone = GameObject.Find("AttackZone");
        _atkZone.SetActive(false);

        instance = this;
    }

    void Start()
    {
        // 이동속도 설정(100-140의 스피드값에 가중치를 곱함)
        move_Speed = 0.8f * default_Speed;
    }

    void Update()
    {
        // 사망시 실행 X
        if (isDie == false)
        {
            Timer();
            Jump();
            ChangeAni();        
        }
    }

    void FixedUpdate()
    {
        // 사망시 실행 X
        if (isDie == false)
        {
            Move();
        }
    }

    // 타이머 제어
    private void Timer()
    {
        // 피격시 무적시간 조절
        if (hitTimer > 0)
        {
            hitTimer -= Time.deltaTime;
        }

        // 공중에 뜬 상태를 판단하는 타이머
        if (isGround)
            groundTimer = 0;
        else
            groundTimer += Time.deltaTime;

        // 사다리 상호작용 중이면 사다리타이머 값을 갱신
        // 이 코드는 사다리 이용시 애니매이션의 재생을 제어하기 위해 작성됨
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

    private void Move()     // 이동함수
    {
        float moveH = Input.GetAxis("Horizontal");      // 좌우 방향키 입력값
        float moveV = Input.GetAxisRaw("Vertical");     // 상하 방향키 입력값 (-1, 0, 1)

        // 사다리 상호작용 중이면
        if (isClimb == true)
        {
            // Y축이동
            _rig.velocity = new Vector2(0, moveV * Time.deltaTime * move_Speed);

            // 사다리 타는 중 애니매이션 제어
            // 사다리 이용중 멈췄을 때, 애니매이션이 재생되면 안됨
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
            // 공격 중일 때 x축 이동값을 0으로 고정
            if (isAttack == true || isUseSkill == true)
            {
                moveH = 0;
            }

            // 입력받은 x축 이동값에 따라 플래이어의 회전값을 변경 (좌우반전)
            // SpriteRenderer의 FilpX 기능을 이용하지 않은 것은, 사용한 스킬도 함께 방향이 변환되어야 하기 때문
            if (moveH > 0)
            {
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            else if (moveH < 0)
            {
                transform.rotation = new Quaternion(0, 180, 0, 0);
            }

            // 땅에 있고, 사다리 이용중이 아니면
            if (isGround == true && isClimbJumped == false)
            {
                // 이동속도와 입력값에 기반해서 X축으로 움직임
                _rig.velocity = new Vector2(moveH * Time.deltaTime * move_Speed, _rig.velocity.y);         

                // 방향키(좌,우) 입력값이 0이 아니면, 이동 애니매이션 재생
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

        if (isAttack == false)  // 공격중엔 점프 방지
        {
            // 일반적인 점프/하향점프
            if (Input.GetButton("Jump") && isGround == true && isClimb == false && isClimbJumped == false)
            {
                // 아래키가 눌리고, 현재 플래이어가 있는 곳이 '지형(Terrain)'일 경우 (땅(Ground)과는 다름)
                if (moveV < 0 && isTerrain == true)
                {
                    // 하향 점프 코루틴 사용 (하향점프중 일시적으로 지형과 플래이어 발판의 충돌을 무시)
                    StartCoroutine(DownJump());
                }
                else if (isClimb == false)
                {
                    // 점프 딜레이 코루틴 사용 (연속으로 점프키가 입력되는 경우, 약간의 딜레이를 줌)
                    StartCoroutine(JumpDelay());
                }
            }

            // 사다리 이용중 점프키가 눌릴 경우
            if (Input.GetButtonDown("Jump") && isClimb == true && moveH != 0)
            {
                SetIsClimb(false);
                // 연속으로 점프가 실행되는 것을 방지
                StartCoroutine(JumpClimb());

                _rig.velocity = new Vector2(2 * moveH, default_JumpForce * 0.020f);
            }
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

    void ChangeAni()    // 애니매이션 변경 코드
    {
        if(isClimb == true)
        {
            _ani.SetBool("IsClimb", true);
        }
        else
        {
            _ani.SetBool("IsClimb", false);
        }

        if (isGround == true)
        {
            _ani.SetBool("IsHitFalling", false);
        }
    }

    // 공격 애니매이션이 끝날 때 호출 됨
    public void EndAttack()
    {
        _ani.SetBool("IsAttack", false);

        isAttack = false;

        _atkZone.SetActive(false);
    }

    // 스킬사용이 끝날 때 호출 됨
    public void EndSkill()
    {
        _ani.SetBool("IsUseSkill", false);

        isUseSkill = false;
    }

    // 피격 시 넉백 판정 코루틴 (바닥에서 x, y축의 힘을 한꺼번에 처리할 경우 제대로 처리되지 않아, 시간차를 두고 따로 처리)
    public IEnumerator Hitjudgment(int directionX, float verForce = 120, float horForce = 80)
    {
        // 점프 중에 피격시, 너무 큰 힘을 받는 것을 방지
        // Y축 넉백
        if (verForce * 0.01f + _rig.velocity.y > 3)
        {
            _rig.velocity = new Vector2(_rig.velocity.x, 3);

        }
        else
        {
            _rig.velocity = new Vector2(_rig.velocity.x, verForce * 0.01f + _rig.velocity.y);
        }

        // 0.05초 딜레이
        yield return new WaitForSeconds(0.05f);

        // X축 넉백
        _rig.AddForce(new Vector2(horForce * directionX, 0), ForceMode2D.Force);
    }

    // 하향점프 코루틴
    IEnumerator DownJump()
    {
        if (isDownJump == false)
        {
            isDownJump = true;
            _rig.velocity = new Vector2(0, default_JumpForce * 0.02f);

            // 0.5초 뒤에 플레이어 발판 콜라이더를 활성화
            yield return new WaitForSeconds(0.5f);

            isDownJump = false;
            CheckJumped.instance._col2D.enabled = true;
        }
    }

    // 점프처리 코루틴
    // 연속으로 점프값이 들어왔을 때, 이동속도에 문제가 생기는 걸 해결하기위해
    // 시간차를 두고 점프가 되도록 처리
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

    // 사다리 이용중 점프시 다시 사다리가 타지는 것을 방지
    IEnumerator JumpClimb()
    {
        if(isClimbJumped == false)
        {
            isClimbJumped = true;

            yield return new WaitForSeconds(0.3f);

            isClimbJumped = false;
        }
    }

    // 사다리 이용 또는 이용중지시 중력값 조절 이후 실행
    // 플레이어 발판 비활성화
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

    // 플레이어가 공중에 0.3초 이상 있었다면, 애니매이션 변경
    IEnumerator FallingDelay()
    {
        yield return new WaitForSeconds(0.3f);

        if (isGround == false && groundTimer > 0.3f)
            _ani.SetBool("IsHitFalling", true);
    }

    // 플레이어에게 데미지를 줌 (데미지를 주는 객체에서 호출)
    public void OnDamage(float damage)
    {
        curHp -= damage;

        // 피격시 무적시간 설정
        hitTimer = hitDelay;

        if (curHp <= 0)
        {
            curHp = 0;

            Die();
        }
    }

    // 공격 범위 활성화
    public void OnAtkZone()
    {
        _atkZone.SetActive(true);
    }

    // 경험치 추가 함수
    public void AddExp(int getExp)
    {
        exp += getExp;

        // 레벨업 경험치 충족시 경험치 차감후 레벨업
        // 위 과정을 반복
        for (; exp > maxExp[lv - 1]; )
        {
            exp -= maxExp[lv - 1];

            LevelUP();
            // 레벨업 UI 활성화
            UIManager.instance.OnLevelUpUI();
        }
    }

    // 레벨업 함수 (각종 스테이터스 향상)
    public void LevelUP()
    {
        lv++;

        maxHp += 10;        // 추후 수정
        maxMp += 10;        // 추후 수정

        curHp = maxHp;
        curMp = maxMp;

        atk += 5;
    }

    // 플레이어 체력이 0이하가 되면 실행됨
    public void Die()
    {
        // 비석 객체
        grave = Instantiate(_grave);

        // 비석위치 초기화
        grave.transform.position = transform.position + Vector3.up * 5;
        grave.GetComponent<Grave>().startPos = grave.transform.position;

        // 플레이어 사망 애니매이션 재생
        isDie = true;
        _ani.SetBool("IsDie", true);

        // 충돌체 비활성화
        _col2D.enabled = false;

        // 만약 점프했을 때, 바닥과 충돌 불가능한 상태가 되는 것을 방지
        CheckJumped.instance._col2D.enabled = true;

        // 점프중에 죽을 경우, X축 이동을 막음
        _rig.velocity = new Vector2(0, _rig.velocity.y);

        UIManager.instance.OnDeathUI();
    }

    public void Revive()
    {
        UIManager.instance.OffDeathUI();

        Destroy(grave);

        curHp = maxHp;
        curMp = maxMp;

        _ani.SetBool("IsDie", false);
        isDie = false;

        _col2D.enabled = true;
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

    public bool GetIsAttack()
    {
        return isAttack;
    }

    public bool GetIsUseSkill()
    {
        return isUseSkill;
    }

    public GameObject GetAtkZone()
    {
        return _atkZone;
    }

    public float GetAtk()
    {
        return atk;
    }

    public void SetIsGround(bool value)
    {
        isGround = value;

        // 관련 애니매이션 설정
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

    public void SetIsAttack(bool value)
    {
        isAttack = value;
    }

    public void SetIsUseSkill(bool value)
    {
        isUseSkill = value;
    }

    public void AddCurHp(int Hp, bool isRatio = false)
    {
        curHp += Hp;

        if(curHp > maxHp)
        {
            curHp = maxHp;
        }
    }

    public void AddCurMp(int Mp, bool isRatio = false)
    {
        curMp += Mp;

        if (curMp > maxMp)
        {
            curMp = maxMp;
        }
    }
}