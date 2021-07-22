using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    float pickUpDelay = 0.1f;   // 아이템 획득 딜레이
    float pickUpTimer = 0;      // 아이템 획득 타이머

    bool isPickable = true;     // 아이템 획득 가능한 상태인지

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
        // 아이템 획득 버튼 작용
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

        // 공격 버튼 작용 (Left Ctrl)
        if (Input.GetButton("Fire1") && Player.instance.GetIsAttack() == false)
        {
            Player.instance._ani.SetBool("IsAttack", true);
            Player.instance._ani.SetInteger("AttackRan", Random.Range(1, 101));

            Player.instance.SetIsAttack(true);
            Player.instance.OnAtkZone();

            Player.instance.GetAtkZone().GetComponent<AttackZone>().multiTarget = 1;
        }
        // 스킬 버튼 작용 (Left Shift)
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

        // 인벤토리 버튼 작용 (I key)
        if (Input.GetButtonDown("Inventory"))
        {
            if (InventoryDrag.instance.GetIsOpened() == false)
                InventoryDrag.instance.OpenInventory();
            else if (InventoryDrag.instance.GetIsOpened() == true)
                InventoryDrag.instance.ExitInventory();
        }
    }

    // 아이템 획득 가능상태 반환
    public bool GetIsPickable()
    {
        return isPickable;
    }

    // 아이템 획득 가능상태 설정
    public void SetIsPickable(bool value)
    {
        isPickable = value;
    }
}
