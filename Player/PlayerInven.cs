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
        //addImages = FindObjectOfType<AddImages>();
        //inventoryItem[0].OneIn(addImages.CreateItem(_ITEMCODE.SOIL));
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
