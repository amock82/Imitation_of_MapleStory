using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    Slider                  _expBar;        // 경험치 바
    Text                    _expText;       // 현재 경험치 표기 (+전체 비율)

    Slider                  _hpBar;         // 체력 바
    Text                    _hpText;        // 체력 표기

    Slider                  _mpBar;         // 마나 바
    Text                    _mpText;        // 마나 표기

    Text                    _lvText;        // 레벨 표기
    Text                    _nameText;      // 닉네임 표기

    Text                    _mapNameText;   // 맵 이름 표기
    Image                   _mapIcon;       // 맵 아이콘 표기

    GameObject              _lvUpUI;        // 레벨업 시 표기되는 UI
    GameObject              _deathUI;       // 사망시 표기되는 UI

    GameObject              _map;           // 맵 객체

    public static UIManager instance;
    Player                  _player = Player.instance;

    private void Awake()
    {
        _expBar = GameObject.Find("ExpUI").GetComponent<Slider>();
        _expText = GameObject.Find("CurExpText").GetComponent<Text>();

        _hpBar = GameObject.Find("HpBar").GetComponent<Slider>();
        _hpText = GameObject.Find("HpText").GetComponent<Text>();

        _mpBar = GameObject.Find("MpBar").GetComponent<Slider>();
        _mpText = GameObject.Find("MpText").GetComponent<Text>();

        _lvText = GameObject.Find("LvText").GetComponent<Text>();
        _nameText = GameObject.Find("NameText").GetComponent<Text>();

        _lvUpUI = GameObject.Find("LevelUpUI");
        _deathUI = GameObject.Find("DeathUI");

        _map = GameObject.Find("Map");

        _mapNameText = GameObject.Find("MapNameUI").GetComponentInChildren<Text>();
        _mapIcon = GameObject.Find("MapIcon").GetComponent<Image>();

        instance = this;

        _lvUpUI.SetActive(false);
        _deathUI.SetActive(false);
    }

    void Start()
    {

    }

    void Update()
    {
        UIUpdate();
    }

    void UIUpdate()
    {
        // 경험치, 체력, 마나 비율
        float expRatio = (float)Player.instance.GetExp() / Player.instance.GetMaxExp(Player.instance.GetLv() - 1);
        float hpRatio = (float)Player.instance.GetHp() / Player.instance.GetMaxHp();
        float mpRatio = (float)Player.instance.GetMp() / Player.instance.GetMaxMp();

        // 경험치 UI 갱신
        _expBar.value = Mathf.Lerp(_expBar.value, expRatio, Time.deltaTime * 2);
        _expText.text = Player.instance.GetExp() + " [" + (expRatio * 100).ToString("N3") + "%]";

        // 체력 UI 갱신
        _hpBar.value = Mathf.Lerp(_hpBar.value, hpRatio, Time.deltaTime * 3);
        _hpText.text = (float)Player.instance.GetHp() + "/" + Player.instance.GetMaxHp();

        // 마나 UI 갱신
        _mpBar.value = Mathf.Lerp(_mpBar.value, mpRatio, Time.deltaTime * 3);
        _mpText.text = (float)Player.instance.GetMp() + "/" + Player.instance.GetMaxMp();

        // 레벨 Text 갱신
        _lvText.text = "lv. " + Player.instance.GetLv();

        _mapNameText.text = _map.GetComponent<MapManager>().GetMapName() + "\n" + _map.GetComponent<MapManager>().GetRegionName();
        _mapIcon.sprite = _map.GetComponent<MapManager>().mapIcon;
    }

    // 레벨업 시에 실행
    public void OnLevelUpUI()
    {
        _lvUpUI.SetActive(true);
        // 레벨업 UI 표기 설정
        _lvUpUI.GetComponentInChildren<Text>().text = "레벨 " + Player.instance.GetLv();

        _expBar.value = (float)Player.instance.GetExp() / Player.instance.GetMaxExp(Player.instance.GetLv() - 1);
        _hpBar.value = (float)Player.instance.GetHp() / Player.instance.GetMaxHp();
        _mpBar.value = (float)Player.instance.GetMp() / Player.instance.GetMaxMp();

        // 2초 후에 레벨업 UI를 비활성화 
        Invoke("OffLevelUpUi", 2);
    }

    // 레벨업 UI 비활성화 함수
    public void OffLevelUpUi()
    {
        _lvUpUI.SetActive(false);
    }

    // 플레이어 사망시 실행
    public void OnDeathUI()
    {
        _deathUI.SetActive(true);

        _deathUI.transform.position = new Vector2(683, 384);
        _deathUI.GetComponent<DragUI>().InitOriginalVec();
    }

    // 사망 UI 비활성화 함수
    public void OffDeathUI()
    {
        _deathUI.SetActive(false);
    }
}
