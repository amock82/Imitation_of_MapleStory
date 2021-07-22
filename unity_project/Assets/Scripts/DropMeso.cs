using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropMeso : MonoBehaviour
{
    GameObject _dropMeso;       // 메소 드롭 UI
    GameObject _notDropMeso;    // 메소 드롭 불가 UI

    InputField _inputField;     // 메소 드롭값 입력 필드
    Text _notDropText;          // 메소 드롭 불가 UI의 Text

    public GameObject _mesoPrefep;      // 메소 객체

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
        // 메소 드롭 불가 UI가 활성화중이면
        if (_notDropMeso.active)
        {
            // ESC, Enter 키 입력을 받으면 실행
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
            {
                OKBtn2();
            }
        }
        // 메소 드롭 UI가 활성화중이면
        else if (_dropMeso.active)
        {
            // ESC키가 눌리면 Cancel 작용
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CancelBtn();
            }
            // Enter키가 눌리면 OK 작용
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                OKBtn();
            }
        }
    }

    public void Call()
    {
        // 메소 드롭 UI 활성화
        _dropMeso.active = true;
        _notDropMeso.active = false;

        // 객체 위치 초기화
        _dropMeso.transform.position = new Vector3(683, 384, 0);

        // 드롭 값 입력 필드 초기값은 10
        _inputField.text = 10.ToString();

        _inputField.Select();
    }

    public void CantDrop()
    {
        // 입력값이 50000 보다 크면 드랍불가 UI 활성화
        if (int.Parse(_inputField.text) > 50000)
        {
            _notDropMeso.active = true;

            _notDropText.text = "50000 이하의 숫자만 가능합니다.";
        }
        // 입력값이 10보다 작으면, 드랍불가 UI 활성화
        else if (int.Parse(_inputField.text) < 10)
        {
            _notDropMeso.active = true;

            _notDropText.text = "10 이상의 숫자만 가능합니다.";
        }
        // 입력값보다 적은 메소를 보유한 경우, 드랍불가 UI 활성화
        else
        {
            _notDropMeso.active = true;

            _notDropText.text = Inventory.instance.GetMeso() + " 이하의 숫자만 가능합니다.";
        }
    }

    public void OKBtn()
    {
        int meso = Inventory.instance.GetMeso();        // 메소 보유량
        int input = int.Parse(_inputField.text);        // 드롭 값 입력 수치

        // 드롭이 불가능한 경우
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

            Inventory.instance.SetAddMeso(-input);  // 보유한 메소 설정

            _dropMeso.active = false;               // 메소 드랍 UI 비활성화
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
