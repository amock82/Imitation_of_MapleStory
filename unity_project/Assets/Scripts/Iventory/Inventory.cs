using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public Slot[]           _slot;          // 인벤토리 UI의 인벤토리 슬릇 객체들
    public Item             emptyItem;      // 슬릇이 비어있을 때, 슬릇에 넣어둘 아이템객체
    
    Toggle[]                _invenTap;      // 인벤토리 아이템 분류탭 (ex.장비/소비/기타 등)
    Text                    _mesoText;      // 소유한 메소의 양을 표기하는 텍스트

    public List<List<Item>> item = null;    // 인벤토리에 들어있는 아이템의 리스트

    public int              tap = 0;        // 현재 인벤토리 아이템 분류 탭
    private int             meso = 1000;    // 메소    (초기값 1000 - 테스트용)
    
    private void Awake()
    {
        instance = this;

        item = new List<List<Item>>();
        _invenTap = GetComponentsInChildren<Toggle>();
        _mesoText = GameObject.Find("MesoText").GetComponent<Text>();

        item.Add(new List<Item>());     // 장비 아이템 리스트
        item.Add(new List<Item>());     // 소비 아이템 리스트
        item.Add(new List<Item>());     // 기타 아이템 리스트
        item.Add(new List<Item>());     // 설치 아이템 리스트
        item.Add(new List<Item>());     // 캐시 아이템 리스트
        item.Add(new List<Item>());     // 치장 아이템 리스트

        _slot = GameObject.Find("GridGroup").GetComponentsInChildren<Slot>();


        // 초기 인벤토리에는 아무것도 들은게 없기에, 빈 아이템 객체로 채워둠
        // 이는 리스트가 비어있으면 일어나는 오류와 인벤토리 빈공간에 아이템을 옮기는 기능을 위해 추가됨
        for(int i = 0; i <6; i++)
        {
            for (int j = 0; j < 32; j++)
                item[i].Add(Instantiate(emptyItem));
        }
    }

    // 아이템을 주웠을 때, 인벤토리에 추가하는 함수
    public void addItem(Item itemGet, int count)
    {
        int typeIndex = 0;

        // 주운 아이템의 유형검사
        switch (itemGet.itemType)
        {
            case Item.ItemType.Equipment:
                typeIndex = 0;
                break;

            case Item.ItemType.Consumable:
                typeIndex = 1;
                break;

            case Item.ItemType.Loot:
                typeIndex = 2;
                break;

            case Item.ItemType.SetUp:
                typeIndex = 3;
                break;

            case Item.ItemType.Cash:
                typeIndex = 4;
                break;

            case Item.ItemType.Cosmetic:
                typeIndex = 5;
                break;
        }

        // 획득한 아이템이 이미 인벤토리에 있는지 찾기
        Item comp = item[typeIndex].Find(x =>x.itemName == itemGet.itemName);

        // 인벤토리에 같은 아이템이 존재하지 않음 -> 빈 아이템객체를 획득한 아이템으로 바꿈 (인벤토리 추가)
        if (comp == null)
        {
            Item temp = item[typeIndex].Find(x => x.itemName == emptyItem.itemName);

            temp.ChangeItem(itemGet);
            temp.itemAmount = count;
        }
        // 인벤토리에 같은 아이템이 존재함 -> 획득한 아이템의 갯수를 기존 아이템에 추가함
        else if (comp.itemName == itemGet.itemName)
        {
            comp.itemAmount += count;
        }
    }

    // 메소 반환 함수
    public int GetMeso()
    {
        return meso;
    }

    // 메소 가감 함수
    public void SetAddMeso(int add)
    {
        meso += add;
    }

    private void Update()
    {
        // 현재 인벤토리 아이템 유형 탭 검사
        for (int i = 0; i < 6; i++)
        {
            if(_invenTap[i].isOn == true)
            {
                tap = i;
                break;
            }
        }

        // 아이템 리스트 중, 현재 탭의 아이템들을 슬릇UI에 갱신
        for (int i = 0; i <item[tap].Count; i++)
        {
            _slot[i].PutItem(item[tap][i]);
            if(item[tap][i].itemAmount == 0)
            {
                item[tap][i].ChangeItem(Inventory.instance.emptyItem);
            }
        }

        // 보유한 메소 표기 (표기법은 1,000,000)
        _mesoText.text = meso.ToString("N0");
    }
}
