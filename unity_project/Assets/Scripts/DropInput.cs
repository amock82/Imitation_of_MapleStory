using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DropInput : MonoBehaviour
{
    GameObject _dropInput;      // 아이템 드롭 UI
    GameObject _notDrop;        // 아이템 드롭 불가 안내 UI

    InputField _inputField;     // 아이템 드롭시 갯수 입력 필드
    Text _notDropText;          // 아이템 드롭 불가 안내 UI 의 Text

    public GameObject _itemPrefep;      // 아이템 객체

    static public DropInput instance;

    private void Awake()
    {
        _dropInput = GameObject.Find("DropInputUI");
        _notDrop = GameObject.Find("NotDropUIFrame");

        _inputField = GameObject.Find("InputField").GetComponent<InputField>();
        _notDropText = GameObject.Find("NotDropText").GetComponent<Text>();

        instance = this;

        _dropInput.active = false;
    }

    private void Update()
    {
        // 드롭불가 UI 활성화 시, ESC나 엔터키로 끌 수 있음
        if(_notDrop.active)
        {
            if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
            {
                OKBtn2();
            }
        }
        // 아이템 드롭 UI 활성화 시, ESC와 엔터키에 따른 각각의 작용 가능
        else if(_dropInput.active)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                CancelBtn();
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                OKBtn();
            }
        }
    }

    public void Call()
    {
        // 떨어뜨릴 아이템의 갯수가 1개인 경우
        if (SlotDrag.instance.slotDrag.itemCount == 1)
        {
            ItemPickUp iPick = _itemPrefep.GetComponent<ItemPickUp>();      // 떨어뜨릴 아이템의 스크립트           
            iPick.item.ChangeItem(SlotDrag.instance.slotDrag.item);         // 떨어뜨릴 아이템의 정보를 드래그중인 슬릇의 아이템으로 교체

            // 떨어뜨릴 아이템을 플래이어 위치에서 생성 
            GameObject dropItem = Instantiate(_itemPrefep, Player.instance.transform.position + Vector3.up * 0.2f, new Quaternion(0, 0, 0, 0));

            // 슬릇의 아이템을 빈 아이템 객체로 변경
            SlotDrag.instance.slotDrag.item.ChangeItem(Inventory.instance.emptyItem);
        }
        // 떨어뜨리려는 아이템의 보유 갯수가 1개보다 많은경우
        else if (SlotDrag.instance.slotDrag.itemCount > 1)
        {
            // 아이템 드랍 UI 활성화
            _dropInput.active = true;
            _notDrop.active = false;


            // 아이템 드랍  UI 위치 초기화
            _dropInput.transform.position = new Vector3(683, 384, 0);

            // 떨어뜨릴 아이템 갯수 입력란의 숫자는 아이템의 총 보유량으로 초기화
            _inputField.text = SlotDrag.instance.slotDrag.itemCount.ToString();

            _inputField.Select();
        }
    }

    // 드랍불가 UI 설정
    public void CantDrop()
    {
        _notDrop.active = true;

        _notDropText.text = SlotDrag.instance.slotDrag.itemCount + " 이하의 숫자만 가능합니다.";
    }

    // 아이템 드랍 UI에서 확인 버튼을 누른 경우
    public void OKBtn()
    {
        // 입력값이 아이템 보유 갯수보다 적거나 같은경우
        if (int.Parse(_inputField.text) <= SlotDrag.instance.slotDrag.itemCount)
        {
            int count = SlotDrag.instance.slotDrag.itemCount;

            ItemPickUp iPick = _itemPrefep.GetComponent<ItemPickUp>();      // 떨어뜨릴 아이템의 스크립트           
            iPick.item.ChangeItem(SlotDrag.instance.slotDrag.item);         // 떨어뜨릴 아이템의 정보를 드래그중인 슬릇의 아이템으로 교체

            // 떨어뜨릴 아이템을 플래이어 위치에서 생성 
            GameObject dropItem = Instantiate(_itemPrefep, Player.instance.transform.position + Vector3.up * 0.2f, new Quaternion(0, 0, 0, 0));

            dropItem.GetComponent<ItemPickUp>().SetCount(int.Parse(_inputField.text));  // 떨어뜨릴 아이템의 갯수를 입력값으로 변경

            // 아이템을 버리는 갯수가 보유한 아이템의 갯수랑 같은경우
            if (int.Parse(_inputField.text) == count)
            {
                // 슬릇의 아이템을 빈 아이템 객체로 바꿈
                SlotDrag.instance.slotDrag.item.ChangeItem(Inventory.instance.emptyItem);
            }
            // 아이템을 버리는 갯수가 보유한 아이템의 갯수보다 적은 경우
            else if (int.Parse(_inputField.text) < count)
            {
                // 보유한 아이템 갯수 차감
                SlotDrag.instance.slotDrag.item.itemAmount -= int.Parse(_inputField.text);
            }

            // 아이템 드랍 UI 비활성화
            _dropInput.active = false;
        }
        // 입력값이 보유한 아이템의 갯수보다 많은경우
        else
        {
            // 드랍불가 함수
            CantDrop();
        }

    }

    // 아이템 드랍 UI 비활성화
    public void CancelBtn()
    {
        _dropInput.active = false;
    }

    // 아이템 드랍 불가 UI 비활성화
    public void OKBtn2()
    {
        _notDrop.active = false;
    }
}
