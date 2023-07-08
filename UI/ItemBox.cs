using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ItemBox : MonoBehaviour
{
    [SerializeField]
    private List<Item> _item = new List<Item>();
    public Text _text;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void Visible(Transform par)
    {
        if (_item.Count > 0)
        {
            _item[_item.Count - 1].gameObject.SetActive(true);
            _item[_item.Count - 1].transform.SetParent(par, false);
            _item[_item.Count - 1].transform.localPosition = Vector3.zero;
            _item[_item.Count - 1].CollisionRemoval();
        }
    }

    public void Invisible()
    {
        if (_item.Count > 0)
            _item[_item.Count - 1].gameObject.SetActive(false);
    }

    public void DeleteItems()
    {
        for(int i = 0; i < _item.Count; i++)
        {
            Destroy(_item[i].gameObject);
        }

        _item.Clear();

        Setting();
    }
    public void ClearItems()
    {
        _item.Clear();
        Setting();
    }

    public Item OneOut()
    {
        Item item = null;

        if (_item.Count > 0)
        {
            item = _item[_item.Count - 1];
            _item.RemoveAt(_item.Count - 1);
        }

        Setting();
        item.gameObject.SetActive(true);
        return item;
    }

    public void OneIn(Item item)
    {
        _item.Add(item);
        item.gameObject.SetActive(false);
        Setting();
    }

    public void SetItemList(List<Item> list)
    {
        if(list == null)
        {
            DeleteItems();
        }
        else if(list.Count <= 0)         //리스트로 아무것도 안들어오면 사제 후 비우기
        {
            DeleteItems();
        }
        else if(_item.Count <= 0)   //들어온 리스트가 있으면서 내가 아무것도 없을때 리스트 받고 상대껀 클리어
        {
            _item = list.ToList();
            list.Clear();
        }
        else if (list.Count > 0 && _item.Count > 0)     //나도 있고 상대도 있을때
        {
            if (list[0].scriptble.GetCode() == _item[0].scriptble.GetCode())    //근데 서로 같은 아이템임
            {
                _item.AddRange(list);
                list.Clear();
            }
            else //같은 아이템이 아님
            {
                List<Item> items = list.ToList();
                list = _item.ToList();
                _item = items.ToList();
            }
        }
        
        Setting();
    }

    public List<Item> GetItemList()
    {
        return _item;
    }

    public Item GetItem()
    {
        if (_item.Count > 0)
        {
            return _item[0];
        }
        else
        {
            return null;
        }
    }

    public void Setting()
    {
        if(_item.Count > 0)
        {
            SetSprite(_item[0].scriptble.GetSprite());
        }
        else
        {
            SetSprite(null);
        }
        SetText();
    }
    private void SetSprite(Sprite sprite)
    {
        GetComponent<Image>().sprite = sprite;
    }
    private void SetText()
    {
        if(_item.Count <= 1)
        {
            _text.text = "";
        }
        else
        {
            _text.text = _item.Count.ToString();
        }
    }
}
