using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    EdgeCollider2D          _terrain;
    Animator                _ani;
    public Rigidbody2D      _rig;
    public SpriteRenderer   _spRen;
    BoxCollider2D           _col2D;

    Transform               _target = null;
    public Transform        _damagePos;
    public GameObject       _damageText;

    GameObject              _enemyUI;
    Slider                  _hpBar;

    enum State 
    {
        Idle = 0,
        Move,
        Jump,
        Hit,
        Die
    };

    public int              direction;
    float                   stateTimer = 2;
    float                   stateDelay = 2;
    float                   changeDirTimer = 0;
    float                   changeDirDelay = 0.5f;

    bool                    isTerrain;
    bool                    isJump;
    bool                    isHit = false;
    bool                    isDie = false;

    float                   maxHp = 100;
    float                   curHp = 100;
    int                     exp = 30;

    float                   atk;
    float                   def;

    public GameObject       lootItem;
    public GameObject       dropMeso;

    [SerializeField]State   state = State.Idle;

    private void Awake()
    {
        _ani = GetComponent<Animator>();
        _rig = GetComponent<Rigidbody2D>();
        _spRen = GetComponent<SpriteRenderer>();
        _col2D = GetComponent<BoxCollider2D>();

        _enemyUI = GetComponentInChildren<Canvas>().gameObject;
        _hpBar = GetComponentInChildren<Slider>();

        _enemyUI.SetActive(false);
    }

    private void Update()
    {
        if (isTerrain == true)
        {
            //Debug.Log(_terrain.bounds.min.x);   // 밟고있는 지형의 왼쪽 경계
            //Debug.Log(_terrain.bounds.max.x);   // 밟고있는 지형의 오른쪽 경계
        }

        if (isDie == false)
        {
            Timer();
            StateChange();

            UIView();
        }
        AniChange(state);
    }

    private void FixedUpdate()
    {
        Move();
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
        if (isHit == false)
        {
            _rig.velocity = new Vector2(1 * direction, _rig.velocity.y);

            if (direction == -1)
            {
                //transform.rotation = new Quaternion(0, 0, 0, 0);
                _spRen.flipX = false;
            }
            else if (direction == 1)
            {
                //transform.rotation = new Quaternion(0, 180, 0, 0);
                _spRen.flipX = true;
            }

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
                int ran = 0;

                if (_target == null)
                {
                    direction = Random.Range(-1, 2);

                    ran = Random.Range(0, 100);
                }
                else
                {
                    if (_target.position.x < transform.position.x)
                        direction = -1;
                    else
                        direction = 1;
                }

                //Debug.Log(direction);

                if (direction == -1 || direction == 1)
                {
                    state = State.Move;
                }
                else
                {
                    state = State.Idle;
                }

                if (ran >= 70)
                {
                    state = State.Jump;

                    _rig.velocity = new Vector2(_rig.velocity.x, 4);
                    isJump = true;
                }

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

            _hpBar.value = curHp / maxHp;
        }

    }

    public void OnDamage(float damage)
    {
        curHp -= damage;
        GameObject textMP = Instantiate(_damageText, _damagePos);

        //textMP.transform.position = _damagePos.position;
        textMP.GetComponent<TextMeshProUGUI>().text = damage.ToString();

        isHit = true;

        if (curHp <= 0)
        {
            curHp = 0;

            GameObject item = Instantiate(lootItem, transform.position + Vector3.up * -0.3f + Vector3.left * 0.1f, new Quaternion(0, 0, 0, 0));
            GameObject meso = Instantiate(dropMeso, transform.position + Vector3.up * -0.3f + Vector3.right * 0.1f, new Quaternion(0, 0, 0, 0));

            item.GetComponent<ItemPickUp>().SetCount(1);
            meso.GetComponent<ItemPickUp>().SetMesoAmount(50);

            _target.gameObject.GetComponent<Player>().AddExp(exp);

            _col2D.enabled = false;

            state = State.Die;
            isDie = true;

        }
    }

    public void EndHit()
    {
        isHit = false;

        _ani.SetBool("IsHit", false);

        stateTimer = 0.5f;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "Terrain")  // 충돌 객체가 지형이면
        {
            isTerrain = true;
            _terrain = collision.gameObject.GetComponent<EdgeCollider2D>();

            _ani.SetBool("IsJump", false);

            if (direction == -1 || direction == 1)
            {
                state = State.Move;
            }
            else
                state = State.Idle;

            isJump = false;
        }
        else if (collision.gameObject.tag == "Ground")
        {
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
            if (Player.instance.GetHitTimer() <= 0)
            {
                //instance.SetIsGround(false);

                if (Player.instance.GetHp() > 20)
                {
                    if (collision.transform.position.x < transform.position.x)
                    {
                        instance.StartCoroutine(instance.Hitjudgment(-1, instance.verForce, instance.horForce));
                        //_rig.velocity = new Vector2(Time.deltaTime * 100 - _rig.velocity.x * 100, Time.deltaTime * 100 + _rig.velocity.y);
                    }

                    if (collision.transform.position.x > transform.position.x)
                    {
                        //_rig.AddForce(new Vector2(Time.deltaTime * (-horForce) - _rig.velocity.x * 100, Time.deltaTime * verForce), ForceMode2D.Impulse);
                        instance.StartCoroutine(instance.Hitjudgment(1, instance.verForce, instance.horForce));
                    }
                }

                //hitTimer = hitDelay;

                instance.OnDamage(20);
            }
        }
    }

    public float GetCurHp()
    {
        return curHp;
    }

    public void SetIsHit(bool value)
    {
        isHit = value;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}
