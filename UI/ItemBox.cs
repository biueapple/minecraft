using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemBox : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] 
    public List<Item> _items = new List<Item>();
    private Text text;
    public ITEM_TYPE type;
    public Action<EquipItem> equip = null;
    public Action<EquipItem> litf = null;
    public bool put;
    private List<Action> popD = new List<Action>();
    private List<Action> putD = new List<Action>();

    private List<Action> pointerDwon = new List<Action>();

    private CharacterInven inventory;
    private InventoryView inventoryView;

    public void Init(CharacterInven inven, InventoryView view)
    {
        inventory = inven;
        inventoryView = view;
        text = transform.GetChild(0).GetComponent<Text>();
    }

    public void SetView(Item item, string count)
    {
        GetComponent<Image>().sprite = item.scriptble.GetSprite();
        text.text = count;
    }
    public void OffView()
    {
        Setting();
    }

    public void Visible()
    {
        if(_items.Count > 0)
        {
            if(inventory != null)
            {
                _items[0].transform.SetParent(inventory.GetUnit().itemPosition, false);
            }
            else
            {
                _items[0].transform.SetParent(null, false);
            }

            _items[0].CollisionRemoval();
            _items[0].transform.localEulerAngles = Vector3.zero;
            _items[0].transform.localPosition = Vector3.zero;
            _items[0].gameObject.SetActive(true);
        }
    }

    public void Invisible()
    {
        if (_items.Count > 0)
        {
            _items[0].gameObject.SetActive(false);
        }
    }

    public bool ItemAdd(Item item)
    {
        if (Condition(item))
        {
            _items.Add(item);

            if (GetEquipItem() != null && equip != null)
                equip(GetEquipItem());

            ExPut();
            return true;
        }

        return false;
    }

    public bool ItemsAdd(Item[] item)
    {
        if (Condition(item))
        { 
            _items.AddRange(item); 

            if (GetEquipItem() != null && equip != null)
                equip(GetEquipItem());

            ExPut();
            return true;
        }
        return false;
    }

    public bool GetItemRemove(out Item item)
    {
        if (_items.Count == 0)
        {
            item = null;
            return false;
        }

        if (GetEquipItem() != null && litf != null)
            litf(GetEquipItem());

        item = _items[_items.Count - 1];
        _items.RemoveAt(_items.Count - 1);

        ExPop();
        return true;
    }

    public bool GetItemsRemove(out Item[] items)
    {
        if (_items.Count == 0)
        {
            items = null;
            return false;
        }

        if (GetEquipItem() != null && litf != null)
        {
            litf(GetEquipItem());
        }
        items = _items.ToArray();
        _items.Clear();


        ExPop();

        return true;
    }

    public void ItemDelete(int count = 0)
    {
        if(count == 0)
        {
            for(int i = 0; i < _items.Count;)
            {
                if (GetEquipItem() != null && litf != null)
                    litf(GetEquipItem());
                Destroy(_items[_items.Count - 1].gameObject);
                _items.RemoveAt(_items.Count - 1);
            }
        }
        else
        {
            if(count > _items.Count)
                count = _items.Count;
            for(int i = 0; i < count; i++)
            {
                if (GetEquipItem() != null && litf != null)
                    litf(GetEquipItem());
                Destroy(_items[_items.Count - 1].gameObject);
                _items.RemoveAt(_items.Count - 1);
            }
        }
        ExPop();
    }

    private bool Condition(Item item)
    {
        if (item == null)
            return false;
        if (type != ITEM_TYPE.NONE && item.scriptble.GetItemType() != type)
            return false;
        if (_items.Count > 0)
        {
            if (_items[0].scriptble.GetBoxType() == ITEM_BOX_TYPE.UNDUPLICATE)
                return false;
            if (_items[0].scriptble.GetCode() != item.scriptble.GetCode())
                return false;

        }
        return true;
    }
    private bool Condition(Item[] item)
    {
        if(!put)
            return false;
        if (item == null)
            return false;
        if (item.Length == 0)
            return true;
        if (type != ITEM_TYPE.NONE && item[0].scriptble.GetItemType() != type)
            return false;
        if (_items.Count > 0)
        {
            if (_items[0].scriptble.GetBoxType() == ITEM_BOX_TYPE.UNDUPLICATE)
                return false;
            if (_items[0].scriptble.GetCode() != item[0].scriptble.GetCode())
                return false;
        }
        return true;
    }

    public void ExPop()
    {
        Setting();
        for (int i = 0; i < popD.Count; i++)
        {
            popD[i]();
        }
    }
    public void ExPut()
    {
        Setting();
        for (int i = 0; i <  putD.Count; i++)
        {
            putD[i]();
        }
    }
    public void SetPopD(Action action)
    {
        popD.Add(action);
    }
    public void SetPutD(Action action)
    {
        putD.Add(action);
    }
    public int GetCount()
    {
        return _items.Count;
    }
    public Item GetItem()
    {
        if (_items.Count > 0) return _items[_items.Count - 1];
        else return null;
    }
    public _ITEMCODE GetCode()
    {
        if (_items.Count > 0) return _items[0].scriptble.GetCode();
        else return  _ITEMCODE.NONE;
    }
    public EquipItem GetEquipItem()
    {
        if (_items.Count == 1)
        {
            if (_items[0].GetComponent<EquipItem>() != null)
            {
                return _items[0].GetComponent<EquipItem>();
            }
        }
        return null;
    }
    public void Setting()
    {
        if(GetCount() > 0)
        {
            GetComponent<Image>().sprite = GetItem().scriptble.GetSprite();
        }
        else
        {
            GetComponent<Image>().sprite = null;
        }
        
        if (GetCount() > 1)
        {
            text.text = GetCount().ToString();
        }
        else
        {
            text.text = "";
        }
    }
    public void HandCheck()
    {
        if(this == inventory.GetHand())
        {
            Visible();
        }
        else
        {
            Invisible();
        }
    }
    public string GetTextString() { return text.text; }

    public void AddPoitnerDown(Action action)
    {
        pointerDwon.Add(action);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        for(int i = 0; i < pointerDwon.Count; i++)
        {
            pointerDwon[i]();
        }
        if (GetItem() != null)
        {
            if (inventoryView != null)
            {
                inventoryView.MouseDownItemBox(this);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}
