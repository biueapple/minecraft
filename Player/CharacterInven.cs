using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR;

public class CharacterInven : MonoBehaviour
{
    public GameObject inventoryWindow;
    public GameObject craftingWindow;
    public GameObject equipWindow;

    public ItemBox[] itemBoxes;
    private InventoryView inventoryView;
    private CharacterEquip equip;
    
    protected ItemBox invenHand;
    protected Unit user;


    public void Init()
    {
        inventoryView = GetComponent<InventoryView>();
        inventoryView.Init();
        for (int i = 0;i < itemBoxes.Length; i++)
        {
            itemBoxes[i].put = true;
            itemBoxes[i].Init(this, inventoryView);
            itemBoxes[i].SetPutD(itemBoxes[i].HandCheck);
        }
        equip = GetComponent<CharacterEquip>();
        equip.Init();
        user = GetComponent<Unit>();
        invenHand = itemBoxes[0];
    }

    public bool ItemAdd(Item item)
    {
        for (int i = 0; i < itemBoxes.Length; i++)
        {
            if (itemBoxes[i].GetCount() != 0)
            {
                if (itemBoxes[i].ItemAdd(item))
                {
                    item.gameObject.SetActive(false);
                    item.CollisionRemoval();

                    return true;
                }
            }
        }

        for (int i = 0; i < itemBoxes.Length; i++)
        {
            if (itemBoxes[i].ItemAdd(item))
            {
                item.gameObject.SetActive(false);
                item.CollisionRemoval();

                return true;
            }
        }
        return false;
    }

    public bool ItemAdd(Item item, int index)
    {
        if (index < 0 || index >= itemBoxes.Length) { return false; }
        if (itemBoxes[index].ItemAdd(item))
        {
            item.gameObject.SetActive(false);
            item.CollisionRemoval();

            return true;
        }
        return false;
    }

    public bool ItemsAdds(Item[] item)
    {
        for (int i = 0; i < itemBoxes.Length; i++)
        {
            if (itemBoxes[i].GetCount() != 0)
            {
                if (itemBoxes[i].ItemsAdd(item))
                {
                    for (int j = 0; j < item.Length; j++)
                    {
                        item[j].gameObject.SetActive(false);
                        item[j].CollisionRemoval();
                    }
                    return true;
                }
            }
        }

        for (int i = 0; i < itemBoxes.Length; i++)
        {
            if (itemBoxes[i].ItemsAdd(item))
            {
                for (int j = 0; j < item.Length; j++)
                {
                    item[j].gameObject.SetActive(false);
                    item[j].CollisionRemoval();
                }
                return true;
            }
        }
        return false;
    }

    public bool ItemsAdds(Item[] item, int index)
    {
        if (index < 0 || index >= itemBoxes.Length) { return false; }
        if (itemBoxes[index].ItemsAdd(item))
        {
            for (int j = 0; j < item.Length; j++)
            {
                item[j].gameObject.SetActive(false);
                item[j].CollisionRemoval();
            }
            return true;
        }
        return false;
    }

    public bool Give(ItemBox box1, ItemBox box2)
    {
        Item item;
        if (box2.GetItemRemove(out item))
        {
            if (box1.ItemAdd(item))
            {
                return true;
            }
        }
        return false;
    }

    public bool Duplicate(ItemBox box1, ItemBox box2)
    {
        Item[] items;
        if (box2.GetItemsRemove(out items))
        {
            if (box1.ItemsAdd(items))
            {
                return true;
            }
            else
            {
                box2.ItemsAdd(items);
            }
        }
        return false;
    }

    public bool Swap(ItemBox box1, ItemBox box2)
    {
        Item[] items1;
        Item[] items2;
        box1.GetItemsRemove(out items1);
        box2.GetItemsRemove(out items2);

        if (items2 != null)
        {
            if (!box1.ItemsAdd(items2))
            {
                box1.ItemsAdd(items1);
                box2.ItemsAdd(items2);
                return false;
            }
        }
        if (items1 != null)
        {
            if (!box2.ItemsAdd(items1))
            {
                box1.GetItemsRemove(out items2);
                box1.ItemsAdd(items1);
                box2.ItemsAdd(items2);
                return false;
            }
        }
        return true;
    }

    public void ItemDelete(int index, int count)
    {
        for (int i = 0; i < count; i++)
        {
            itemBoxes[index].ItemDelete();
        }
    }

    public void ItemDelete(int count = 1)
    {
        if(invenHand != null)
        {
            invenHand.ItemDelete(count);
        }
    }

    public void ItemsThrow(ItemBox box)
    {
        if (itemBoxes.Contains(box))
        {
            for (int i = 0; i < itemBoxes.Length; i++)
            {
                if (itemBoxes[i].Equals(box))
                {
                    for (int j = 0; j < itemBoxes[i].GetCount();)
                    {
                        ItemThrow(i);
                    }
                }
            }
        }
    }

    public void ItemThrow(int index)
    {
        Item item;
        if (itemBoxes[index].GetItemRemove(out item))
        {
            item.gameObject.SetActive(true);
            FindObjectOfType<DropItemManager>()?.Throw(gameObject, item);
        }
    }

    public int GetIndex(ItemBox box)
    {
        if (itemBoxes.Contains(box))
        {
            for (int i = 0; i < itemBoxes.Length; i++)
            {
                if (itemBoxes[i].Equals(box))
                    return i;
            }
        }

        return -1;
    }

    public void HotkeyOff()
    {
        if(inventoryView != null)
        {
            inventoryView.HotkeyState(false);
        }
    }
    public void HotkeyOn()
    {
        if (inventoryView != null)
        {
            inventoryView.HotkeyState(true);
        }
    }
    public void HandChange(int index)
    {
        if (invenHand != null)
            invenHand.Invisible();
        invenHand = itemBoxes[index];
        invenHand.Visible();
    }

    public InventoryView GetInventoryView() { return inventoryView; }
    public ItemBox GetHand() { return invenHand; }
    public Unit GetUnit() { return user; }
}
