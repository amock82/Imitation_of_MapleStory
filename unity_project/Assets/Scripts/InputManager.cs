using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    float pickUpDelay = 0.1f;
    float pickUpTimer = 0;

    bool isPickable = true;

    public static InputManager instance;

    public GameObject _skillPrefep;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(pickUpTimer > 0)
        {
            pickUpTimer -= Time.deltaTime;         
        }
        else
        {
            isPickable = false;

            if (Input.GetButton("PickUp"))
            {
                isPickable = true;

                pickUpTimer = pickUpDelay;
            }
        }

        if (Input.GetButton("Fire1") && Player.instance.GetIsAttack() == false)
        {
            Player.instance._ani.SetBool("IsAttack", true);
            Player.instance._ani.SetInteger("AttackRan", Random.Range(1, 101));

            Player.instance.SetIsAttack(true);
            Player.instance.OnAtkZone();

            Player.instance.GetAtkZone().GetComponent<AttackZone>().multiTarget = 1;
        }
        else if (Input.GetButton("Skill") && Player.instance.GetIsUseSkill() == false && Player.instance.GetMp() >= 20)
                                                                                        // 만든 스킬의 mp소모량 = 20
        {
            Player.instance._ani.SetBool("IsUseSkill", true);
            Player.instance._ani.SetTrigger("UseSkill");

            Player.instance.SetIsUseSkill(true);

            GameObject skillObj = Instantiate(_skillPrefep, Player.instance.transform);

            skillObj.transform.position = Player.instance.transform.position + Vector3.up * 0.82f;
            Destroy(skillObj, 1);
        }
        
    }

    public bool GetIsPickable()
    {
        return isPickable;
    }

    public void SetIsPickable(bool value)
    {
        isPickable = value;
    }

    //public void PickUpInput()
    //{
    //    isPickable = false;
    //}
}
