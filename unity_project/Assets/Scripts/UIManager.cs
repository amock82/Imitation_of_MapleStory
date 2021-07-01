using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour, IDropHandler
{
    Slider _expBar;
    Text _expText;

    Slider _hpBar;
    Text _hpText;

    Slider _mpBar;
    Text _mpText;

    Text _lvText;
    Text _nameText;

    Player _player = Player.instance;

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
    }

    void Start()
    {

    }

    void Update()
    {
        UIUpdate();
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Drop");
        // throw new System.NotImplementedException();
    }

    void UIUpdate()
    {
        _expBar.value = (float)Player.instance.GetExp() / Player.instance.GetMaxExp(Player.instance.GetLv() - 1);
        _expText.text = Player.instance.GetExp() + " [" + (_expBar.value * 100).ToString("N3") + "%]";

        _hpBar.value = (float)Player.instance.GetHp() / Player.instance.GetMaxHp();
        _hpText.text = (float)Player.instance.GetHp() + "/" + Player.instance.GetMaxHp();

        _mpBar.value = (float)Player.instance.GetMp() / Player.instance.GetMaxMp();
        _mpText.text = (float)Player.instance.GetMp() + "/" + Player.instance.GetMaxMp();

        _lvText.text = "lv. " + Player.instance.GetLv();
    }
}
