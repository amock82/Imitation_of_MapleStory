using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public Slot[]           _slot;
    public Item             emptyItem;
    
    Toggle[]                _invenTap;
    Text                    _mesoText;

    public List<List<Item>> item = null;

    public int              tap = 0;
    private int              meso = 1000;
    
    private void Awake()
    {
        instance = this;

        item = new List<List<Item>>();
        _invenTap = GetComponentsInChildren<Toggle>();
        _mesoText = GameObject.Find("MesoText").GetComponent<Text>();

        item.Add(new List<Item>());
        item.Add(new List<Item>());
        item.Add(new List<Item>());
        item.Add(new List<Item>());
        item.Add(new List<Item>());
        item.Add(new List<Item>());

        _slot = GameObject.Find("GridGroup").GetComponentsInChildren<Slot>();

        for(int i = 0; i <6; i++)
        {
            for (int j = 0; j < 32; j++)
                item[i].Add(Instantiate(emptyItem));
        }
    }

    public void addItem(Item itemGet, int count)
    {
        int typeIndex = 0;

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
        //Item it = item[typeIndex].Find(x => x.itemName == itemGet.itemName);

        //Debug.Log(itemGet.itemType + " / " + typeIndex);

        Item comp = item[typeIndex].Find(x =>x.itemName == itemGet.itemName);

        //Debug.Log(comp);

        if (comp == null)
        {
            Item temp = item[typeIndex].Find(x => x.itemName == emptyItem.itemName);

            temp.ChangeItem(itemGet);
            temp.itemAmount = count;
            //temp = itemGet;
        }
        else if (comp.itemName == itemGet.itemName)
        {
            comp.itemAmount += count;

            //Debug.Log(comp.itemAmount);
        }
        
        //Debug.Log(item[typeIndex][0]);
    }

    public int GetMeso()
    {
        return meso;
    }

    public void SetAddMeso(int add)
    {
        meso += add;
    }

    private void Update()
    {
        for (int i = 0; i < 6; i++)
        {
            if(_invenTap[i].isOn == true)
            {
                tap = i;
                break;
            }
        }

        for (int i = 0; i <item[tap].Count; i++)
        {
            _slot[i].PutItem(item[tap][i]);
        }

        _mesoText.text = meso.ToString("N0");
        //for (int i = item[tap].Count; i < _slot.Length; i++)
        //{
        //    _slot[i].PullItem();
        //}

        //Debug.Log(item[2][0].itemAmount);
    }
}
