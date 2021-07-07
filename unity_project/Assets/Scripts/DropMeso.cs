using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropMeso : MonoBehaviour
{
    GameObject _dropMeso;
    GameObject _notDropMeso;

    InputField _inputField;
    Text _notDropText;

    public GameObject _mesoPrefep;

    static public DropMeso instance;

    private void Awake()
    {
        _dropMeso = GameObject.Find("DropMesoUI");
        _notDropMeso = GameObject.Find("NotDropMesoUIFrame");

        _inputField = GameObject.Find("InputFieldMeso").GetComponent<InputField>();
        _notDropText = GameObject.Find("NotDropTextMeso").GetComponent<Text>();

        instance = this;

        _dropMeso.active = false;
    }

    private void Update()
    {
        if (_notDropMeso.active)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
            {
                OKBtn2();
            }
        }
        else if (_dropMeso.active)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
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
        _dropMeso.active = true;
        _notDropMeso.active = false;

        _dropMeso.transform.position = new Vector3(683, 384, 0);

        _inputField.text = 10.ToString();

        _inputField.Select();
    }

    public void CantDrop()
    {
        if (int.Parse(_inputField.text) > 50000)
        {
            _notDropMeso.active = true;

            _notDropText.text = "50000 이하의 숫자만 가능합니다.";
        }
        else if (int.Parse(_inputField.text) < 10)
        {
            _notDropMeso.active = true;

            _notDropText.text = "10 이상의 숫자만 가능합니다.";
        }
        else
        {
            _notDropMeso.active = true;

            _notDropText.text = Inventory.instance.GetMeso() + " 이하의 숫자만 가능합니다.";
        }
    }

    public void OKBtn()
    {
        int meso = Inventory.instance.GetMeso();
        int input = int.Parse(_inputField.text);

        if (input < 10 || 50000 < input || meso < input)
        {
            CantDrop();
        }
        else
        {
            ItemPickUp iPick = _mesoPrefep.GetComponent<ItemPickUp>();      // 떨어뜨릴 메소의 스크립트           

            // 떨어뜨릴 아이템을 플래이어 위치에서 생성 
            GameObject dropItem = Instantiate(_mesoPrefep, Player.instance.transform.position + Vector3.up * 0.2f, new Quaternion(0, 0, 0, 0));

            dropItem.GetComponent<ItemPickUp>().SetMesoAmount(input);   // 떨어뜨릴 메소의 갯수를 입력값으로 변경

            Inventory.instance.SetAddMeso(-input);

            _dropMeso.active = false;
        }
    }

    public void CancelBtn()
    {
        _dropMeso.active = false;
    }

    public void OKBtn2()
    {
        _notDropMeso.active = false;
    }
}
