using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class PlayerInven : MonoBehaviour
{
    public ItemBox[] inventoryItem;
    public ItemBox clickBox;
    private int hotkey;
    //

    private AddImages addImages;
    private ItemBox lastClickBox;

    private ItemBox hand;


    void Start()
    {
        addImages = FindObjectOfType<AddImages>();
        inventoryItem[0].OneIn(addImages.CreateItem(_ITEMCODE.WOOD));
        //inventoryItem[0].OneIn(addImages.CreateItem(_ITEMCODE.SOIL));
        //inventoryItem[0].OneIn(addImages.CreateItem(_ITEMCODE.SOIL));
        //inventoryItem[0].OneIn(addImages.CreateItem(_ITEMCODE.SOIL));
    }


    void Update()
    {
        if(clickBox.gameObject.activeSelf)
        {
            clickBox.transform.position = Input.mousePosition;
        }
        hand = inventoryItem[hotkey];
    }



    public bool ItemInput(Item item)
    {
        for(int i = 0; i < inventoryItem.Length; i++)       //인벤토리에 같은 아이템이 있으면 거기에 넣기
        {
            if (inventoryItem[i].GetItem() != null)
            {
                if(inventoryItem[i].GetItem().scriptble.GetCode() == item.scriptble.GetCode())
                {
                    inventoryItem[i].OneIn(item);
                    return true;
                }
            }
        }

        for (int i = 0; i < inventoryItem.Length; i++)      //인벤토리에 빈곳에 아이템 넣기
        {
            if (inventoryItem[i].GetItem() == null)
            {
                inventoryItem[i].OneIn(item);
                return true;
            }
        }

        return false;
    }



    public void RightClickHotkey(Player player, Decryption decryption)
    {
        if(inventoryItem[hotkey].GetItem() != null)
        {
            if (inventoryItem[hotkey].GetItem().RightClick(player, decryption))
            {
                inventoryItem[hotkey].GetItemList().RemoveAt(0);
            }
        }
    }

    public void BoxInputRightDown(ItemBox box)
    {
        if(clickBox.GetItem() != null)
        {
            box.OneIn(clickBox.OneOut());
        }
        
        if(clickBox.GetItem() == null)
        {
            clickBox.gameObject.SetActive(false);
        }
    }
    public void BoxInputRightUp()
    {

    }

    public bool BoxInputLeftDown(ItemBox box)
    {
        if (box.GetItem() == null)
            return false;

        lastClickBox = box;
        clickBox.gameObject.SetActive(true);
        clickBox.SetItemList(box.GetItemList());
        box.ClearItems();
        return true;
    }
    public void BoxInputLeftUp(ItemBox box)
    {
        if (clickBox.GetItem() == null)
            return;

        if(box != null)
        {
            box.SetItemList(clickBox.GetItemList());
        }
        else
        {
            if(lastClickBox != null)
                lastClickBox.SetItemList(clickBox.GetItemList());
        }

        clickBox.Setting();
        clickBox.gameObject.SetActive(false);
    }


    public ItemBox GetHotkeyBox()
    {
        return hand;
    }

    public void SetHotkey(int key)
    {
        hotkey = key;
    }
}
