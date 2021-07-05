using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    EdgeCollider2D          _terrain;
    Animator                _ani;
    Rigidbody2D             _rig;

    enum State 
    {
        Idle = 0,
        Move,
        Jump,
        Hit,
        Die
    };

    int                     direction;
    float                   stateTimer = 2;
    float                   stateDelay = 2;
    float                   changeDirTimer = 0;
    float                   changeDirDelay = 0.5f;

    bool                    isTerrain;
    bool                    isJump;

    float                   hp;
    float                   exp;

    float                   atk;
    float                   def;

    public GameObject       lootItem;

    [SerializeField]State   state = State.Idle;

    private void Awake()
    {
        _ani = GetComponent<Animator>();
        _rig = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isTerrain == true)
        {
            //Debug.Log(_terrain.bounds.min.x);   // 밟고있는 지형의 왼쪽 경계
            //Debug.Log(_terrain.bounds.max.x);   // 밟고있는 지형의 오른쪽 경계
        }

        Timer();
        StateChange();
        AniChange(state);


    }

    private void FixedUpdate()
    {
        Move();
    }

    void Timer()        // 타이머 값 줄이기
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

    void Move()         // 이동함수
    {
        _rig.velocity = new Vector2(1 * direction, _rig.velocity.y);

        if(direction == -1)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else if (direction == 1)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
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

    void StateChange()      // 몬스터 상태 변경 함수
    {
        if(stateTimer < 0)
        {
            direction = Random.Range(-1, 2);

            int ran = Random.Range(0, 100);

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
                _ani.SetBool("IsHit", true);
                break;

            case State.Jump:
                _ani.SetBool("IsJump", true);
                break;

            case State.Die:
                _ani.SetBool("IsDie", true);
                break;
        }
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
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player instance = Player.instance;

        if(collision.tag == "Player")   // 플레이어와 접촉했을 경우 데미지를 주고 넉백
        {
            if (Player.instance.GetHitTimer() <= 0)
            {
                //instance.SetIsGround(false);

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

                //hitTimer = hitDelay;

                instance.OnDamage(10);
            }
        }
    }
}
