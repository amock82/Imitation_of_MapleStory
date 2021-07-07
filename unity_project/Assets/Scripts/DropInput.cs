using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DropInput : MonoBehaviour
{
    GameObject _dropInput;
    GameObject _notDrop;

    InputField _inputField;
    Text _notDropText;

    public GameObject _itemPrefep;

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
        if(_notDrop.active)
        {
            if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
            {
                OKBtn2();
            }
        }
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
        if (SlotDrag.instance.slotDrag.itemCount == 1)
        {
            ItemPickUp iPick = _itemPrefep.GetComponent<ItemPickUp>();      // 떨어뜨릴 아이템의 스크립트           
            iPick.item.ChangeItem(SlotDrag.instance.slotDrag.item);         // 떨어뜨릴 아이템의 정보를 드래그중인 슬릇의 아이템으로 교체

            // 떨어뜨릴 아이템을 플래이어 위치에서 생성 
            GameObject dropItem = Instantiate(_itemPrefep, Player.instance.transform.position + Vector3.up * 0.2f, new Quaternion(0, 0, 0, 0));

            SlotDrag.instance.slotDrag.item.ChangeItem(Inventory.instance.emptyItem);
        }
        else if (SlotDrag.instance.slotDrag.itemCount > 1)
        {
            _dropInput.active = true;
            _notDrop.active = false;

            _dropInput.transform.position = new Vector3(683, 384, 0);

            _inputField.text = SlotDrag.instance.slotDrag.itemCount.ToString();

            _inputField.Select();
        }
    }

    public void CantDrop()
    {
        _notDrop.active = true;

        _notDropText.text = SlotDrag.instance.slotDrag.itemCount + " 이하의 숫자만 가능합니다.";
    }

    public void OKBtn()
    {
        if (int.Parse(_inputField.text) <= SlotDrag.instance.slotDrag.itemCount)
        {
            int count = SlotDrag.instance.slotDrag.itemCount;

            ItemPickUp iPick = _itemPrefep.GetComponent<ItemPickUp>();      // 떨어뜨릴 아이템의 스크립트           
            iPick.item.ChangeItem(SlotDrag.instance.slotDrag.item);         // 떨어뜨릴 아이템의 정보를 드래그중인 슬릇의 아이템으로 교체

            // 떨어뜨릴 아이템을 플래이어 위치에서 생성 
            GameObject dropItem = Instantiate(_itemPrefep, Player.instance.transform.position + Vector3.up * 0.2f, new Quaternion(0, 0, 0, 0));

            dropItem.GetComponent<ItemPickUp>().SetCount(int.Parse(_inputField.text));  // 떨어뜨릴 아이템의 갯수를 입력값으로 변경

            if (int.Parse(_inputField.text) == count)
            {
                SlotDrag.instance.slotDrag.item.ChangeItem(Inventory.instance.emptyItem);
            }
            else if (int.Parse(_inputField.text) < count)
            {
                SlotDrag.instance.slotDrag.item.itemAmount -= int.Parse(_inputField.text);
            }

            _dropInput.active = false;
        }
        else
        {
            CantDrop();
        }

    }

    public void CancelBtn()
    {
        _dropInput.active = false;
    }

    public void OKBtn2()
    {
        _notDrop.active = false;
    }
}
