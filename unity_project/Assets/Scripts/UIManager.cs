using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour, IPointerClickHandler
{
    Slider                  _expBar;
    Text                    _expText;

    Slider                  _hpBar;
    Text                    _hpText;

    Slider                  _mpBar;
    Text                    _mpText;

    Text                    _lvText;
    Text                    _nameText;

    GameObject              _lvUpUI;

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

        instance = this;

        _lvUpUI.SetActive(false);
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
        float expRatio = (float)Player.instance.GetExp() / Player.instance.GetMaxExp(Player.instance.GetLv() - 1);
        float hpRatio = (float)Player.instance.GetHp() / Player.instance.GetMaxHp();
        float mpRatio = (float)Player.instance.GetMp() / Player.instance.GetMaxMp();

        _expBar.value = Mathf.Lerp(_expBar.value, expRatio, Time.deltaTime * 2);

        _expText.text = Player.instance.GetExp() + " [" + (expRatio * 100).ToString("N3") + "%]";

        _hpBar.value = Mathf.Lerp(_hpBar.value, hpRatio, Time.deltaTime * 3);
        _hpText.text = (float)Player.instance.GetHp() + "/" + Player.instance.GetMaxHp();

        _mpBar.value = Mathf.Lerp(_mpBar.value, mpRatio, Time.deltaTime * 3);
        _mpText.text = (float)Player.instance.GetMp() + "/" + Player.instance.GetMaxMp();

        _lvText.text = "lv. " + Player.instance.GetLv();
    }

    public void OnLevelUpUI()
    {
        _lvUpUI.SetActive(true);
        _lvUpUI.GetComponentInChildren<Text>().text = "레벨 " + Player.instance.GetLv();

        _expBar.value = (float)Player.instance.GetExp() / Player.instance.GetMaxExp(Player.instance.GetLv() - 1);
        _hpBar.value = (float)Player.instance.GetHp() / Player.instance.GetMaxHp();
        _mpBar.value = (float)Player.instance.GetMp() / Player.instance.GetMaxMp();

        Invoke("OffLevelUpUi", 2);
    }

    public void OffLevelUpUi()
    {
        _lvUpUI.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerEnter.tag == "DropZone" && SlotDrag.instance.GetIsTrackingMouse() == true)
        {
            if (SlotDrag.instance.slotDrag != null)
            {
                DropInput.instance.Call();

                SlotDrag.instance.SetIsTrackingMouse(false);
            }
        }
    }
}
