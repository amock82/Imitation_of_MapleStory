using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    EdgeCollider2D          _terrain;       // 적이 서있는 지형
    Animator                _ani;           // 적 애니매이터
    public Rigidbody2D      _rig;           // 적 리지드바디
    public SpriteRenderer   _spRen;         // 적 이미지 렌더러
    BoxCollider2D           _col2D;         // 적 피격범위
    public BoxCollider2D    _damageZone;    // 적 공격범위

    Transform               _target = null; // 적의 타겟 (타겟쪽으로 이동)
    public Transform        _damagePos;     // 받은 데미지 표기할 장소 
    public GameObject       _damageText;    // 받은 데미지 텍스트

    GameObject              _enemyUI;       // 적이 받은 데미지/체력바 등을 표기하는 UICanvas가 담긴 객체
    Slider                  _hpBar;         // 적의 체력바 (공격받으면 보여짐)

    // 적의 행동 패턴
    enum State 
    {
        Idle = 0,
        Move,
        Jump,
        Hit,
        Die
    };

    public int              direction;              // 이동방향
    float                   stateTimer = 2;         // 행동 패턴 타이머 (0이 되면 행동패턴을 랜덤으로 바꿈)
    float                   stateDelay = 2;         // 행동 패턴 딜레이 (행동 패턴 타이머를 이 값으로 초기화)
    float                   changeDirTimer = 0;     // 적이 지형위에 있을 경우, 지형 끝에서 방향을 바꿔서 떨어지지 않게 방향을 바꾸는 타이머
    float                   changeDirDelay = 0.5f;  // 방향전환 타이머의 초기화 값

    bool                    isTerrain;              // 지형위에 있는지
    bool                    isJump;                 // 점프중인지
    bool                    isHit = false;          // 피격중인지
    bool                    isDie = false;          // 사망했는지

    float                   maxHp = 100;            // 최대체력
    float                   curHp = 100;            // 현재체력
    int                     exp = 30;               // 경험치양

    float                   atk;                    // 공격력 (미구현 - 임시값 사용)
    float                   def;                    // 방어력 (미구현 - 임시값 사용)

    public GameObject       lootItem;               // 드랍 아이템
    public GameObject       dropMeso;               // 드랍 메소

    [SerializeField]State   state = State.Idle;

    private void Awake()
    {
        _ani = GetComponent<Animator>();
        _rig = GetComponent<Rigidbody2D>();
        _spRen = GetComponent<SpriteRenderer>();
        _col2D = GetComponentInChildren<BoxCollider2D>();

        _enemyUI = GetComponentInChildren<Canvas>().gameObject;
        _hpBar = GetComponentInChildren<Slider>();

        _enemyUI.SetActive(false);
    }

    private void Update()
    {
        // 적이 살아있으면 실행
        if (isDie == false)
        {
            Timer();        // 각종 타이머 컨트롤
            StateChange();  // 행동 패턴 변환 (랜덤하게)

            UIView();       // 적 UI정보 갱신 (피격 데미지, 피격시 체력바)
        }
        AniChange(state);   // 행동 패턴에 따라 애니매이션 변화
    }

    private void FixedUpdate()
    {
        Move();         // 이동함수
    }

    void Timer()        // 타이머 값 줄이기
    {
        if (isHit == false)
        {
            if (stateTimer >= 0)
            {
                stateTimer -= Time.deltaTime;
            }

            if (changeDirTimer >= 0)
            {
                changeDirTimer -= Time.deltaTime;
            }
        }
    }

    void Move()         // 이동함수
    {
        // 피격중이 아닐때만
        if (isHit == false)
        {
            // 이동
            _rig.velocity = new Vector2(1 * direction, _rig.velocity.y);

            // 이동방향에 따라 이미지를 반전시킴
            if (direction == -1)
            {
                _spRen.flipX = false;
            }
            else if (direction == 1)
            {
                _spRen.flipX = true;
            }

            // 지형위에 있고 지형 끝에 있으면 방향 전환
            if (_terrain != null)
            {
                if (_terrain.bounds.min.x >= transform.position.x && changeDirTimer < 0 && isJump == false)
                {
                    direction = 1;

                    changeDirTimer = changeDirDelay;
                }
                else if (_terrain.bounds.max.x <= transform.position.x && changeDirTimer < 0 && isJump == false)
                {
                    direction = -1;

                    changeDirTimer = changeDirDelay;
                }
            }
        }
    }

    void StateChange()      // 몬스터 상태 변경 함수
    {
        if(isHit == true)
        {
            state = State.Hit;
        }
        else
        {
            if (stateTimer < 0)
            {
                int ran = 0;    // 랜덤값 저장공간

                // 타겟(목표물)이 없으면
                if (_target == null)
                {
                    // 방향 랜덤지정
                    direction = Random.Range(-1, 2);

                    // 랜덤값 받기
                    ran = Random.Range(0, 100);
                }
                // 타겟이 있으면 타겟 방향으로 이동
                else
                {
                    if (_target.position.x < transform.position.x)
                        direction = -1;
                    else
                        direction = 1;
                }

                // 이동방향이 양/음수이면 Move상태, 0이면 Idle 상태
                if (direction == -1 || direction == 1)
                {
                    state = State.Move;
                }
                else
                {
                    state = State.Idle;
                }

                // 랜덤값이(1-100) 70 이상이면 점프
                if (ran >= 70)
                {
                    state = State.Jump;

                    _rig.velocity = new Vector2(_rig.velocity.x, 4);
                    isJump = true;
                }

                // 상태 변환 타이머 초기화
                stateTimer = stateDelay;
            }
        }
    }

    void AniChange(State state)     // 상태에 때른 애니매이션 변경
    {
        switch (state)
        {
            case State.Idle:
                _ani.SetBool("IsMove", false);
                break;

            case State.Move:
                _ani.SetBool("IsMove", true);
                break;

            case State.Hit:
                if (isHit == true)
                {
                    _ani.SetBool("IsHit", true);
                }
                break;

            case State.Jump:
                _ani.SetBool("IsJump", true);
                break;

            case State.Die:
                _ani.SetBool("IsDie", true);
                break;
        }
    }

    void UIView()
    {
        if (_target != null)
        {
            _enemyUI.SetActive(true);

            // 적 체력바 수치 설정
            _hpBar.value = curHp / maxHp;
        }

    }

    public void OnDamage(float damage)
    {
        curHp -= damage;    // 체력 차감

        // TextMp - TextMeshProUGUI
        // 데미지를 입으면 TextMP를 지정된 부모의 자식객체로 생성
        GameObject textMP = Instantiate(_damageText, _damagePos);

        // TextMP의 텍스트 값을 받은 데미지로 변경
        textMP.GetComponent<TextMeshProUGUI>().text = damage.ToString();

        isHit = true;
        
        // 남은 체력이 0보다 같거나 작아지면
        if (curHp <= 0)
        {
            curHp = 0;

            // 아이템 생성 (드랍템)
            GameObject item = Instantiate(lootItem, transform.position + Vector3.up * -0.3f + Vector3.left * 0.1f, new Quaternion(0, 0, 0, 0));
            GameObject meso = Instantiate(dropMeso, transform.position + Vector3.up * -0.3f + Vector3.right * 0.1f, new Quaternion(0, 0, 0, 0));

            // 아이템 갯수, 메소량 설정
            item.GetComponent<ItemPickUp>().SetCount(1);
            meso.GetComponent<ItemPickUp>().SetMesoAmount(50);

            // 타겟(플레이어)의 경험치 추가
            _target.gameObject.GetComponent<Player>().AddExp(exp);

            // 콜라이더 기능 끄기
            _col2D.enabled = false;
            _damageZone.enabled = false;

            // 상태 변환 
            state = State.Die;
            isDie = true;
        }
    }

    // 애니매이션 이벤트 (Hit 애니매이션이 끝나면 호출)
    public void EndHit()
    {
        isHit = false;

        _ani.SetBool("IsHit", false);

        stateTimer = 0.5f;
    }

    // 사망처리
    public void Die()
    {
        Destroy(gameObject);

        // 리스폰을 위해 맵의 총 몬스터 수 감소 처리
        MapManager.instance.MonDie();
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "Terrain")  // 착지시 충돌 객체가 지형이면
        {
            isTerrain = true;
            _terrain = collision.gameObject.GetComponent<EdgeCollider2D>();

            // 점프 애니매이션 해제
            _ani.SetBool("IsJump", false);

            // 이동방향에 따라 애니매이션 조절
            if (direction == -1 || direction == 1)
            {
                state = State.Move;
            }
            else
                state = State.Idle;

            isJump = false;
        }
        // 기본적인 동작은 위 if문과 동일
        else if (collision.gameObject.tag == "Ground")
        {
            isTerrain = false;
            _terrain = null;

            _ani.SetBool("IsJump", false);

            if (direction == -1 || direction == 1)
            {
                state = State.Move;
            }
            else
                state = State.Idle;

            isJump = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player instance = Player.instance;

        if(collision.tag == "Player" && collision.tag != "AttackZone" )   // 플레이어와 접촉했을 경우 데미지를 주고 넉백
        {
            // 플레이어의 피격시 무적시간 타이머가 0보다 같거나 작으면
            if (Player.instance.GetHitTimer() <= 0)
            {
                // 플레이어의 체력이 임시데미지(20)보다 크면 넉백
                if (Player.instance.GetHp() > 20)
                {
                    // 몬스터의 위치에 따라 플레이어의 넉백방향을 정함
                    if (collision.transform.position.x < transform.position.x)
                    {
                        instance.StartCoroutine(instance.Hitjudgment(-1, instance.verForce, instance.horForce));
                    }

                    if (collision.transform.position.x > transform.position.x)
                    {
                        instance.StartCoroutine(instance.Hitjudgment(1, instance.verForce, instance.horForce));
                    }
                }

                // 플레이어에게 임시데미지(20)를 줌
                instance.OnDamage(20);
            }
        }
    }

    // 적의 체력 반환
    public float GetCurHp()
    {
        return curHp;
    }

    // 피격중인지를 세팅
    public void SetIsHit(bool value)
    {
        isHit = value;
    }

    // 타겟을 세팅(선공몹 또는 피격시)
    public void SetTarget(Transform target)
    {
        _target = target;
    }
}
