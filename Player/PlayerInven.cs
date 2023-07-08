using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInven : MonoBehaviour
{
    public ItemBox[] inventoryItem;
    public ItemBox clickBox;
    public Transform itemPosition;
    //

    private AddImages addImages;
    private ItemBox lastClickBox;

    public ItemBox hand;


    void Start()
    {
        addImages = FindObjectOfType<AddImages>();
        inventoryItem[0].OneIn(addImages.CreateItem(_ITEMCODE.WOOD));
        ItemInput(addImages.CreateItem(_ITEMCODE.WATER));
        //inventoryItem[0].OneIn(addImages.CreateItem(_ITEMCODE.SOIL));
        //inventoryItem[0].OneIn(addImages.CreateItem(_ITEMCODE.SOIL));
        //inventoryItem[0].OneIn(addImages.CreateItem(_ITEMCODE.SOIL));
        hand = inventoryItem[0];
        HandInit();
    }


    void Update()
    {
        if(clickBox.gameObject.activeSelf)
        {
            clickBox.transform.position = Input.mousePosition;
        }
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
                    HandInit();
                    BoxSetting();
                    return true;
                }
            }
        }

        for (int i = 0; i < inventoryItem.Length; i++)      //인벤토리에 빈곳에 아이템 넣기
        {
            if (inventoryItem[i].GetItem() == null)
            {
                inventoryItem[i].OneIn(item);
                HandInit();
                BoxSetting();
                return true;
            }
        }

        return false;
    }



    public void HandConsume()
    {
        if(hand.GetItem() != null)
        {
            Destroy(hand.GetItemList()[0].gameObject);
            hand.GetItemList().RemoveAt(0);
            BoxSetting();
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

        if (box == hand)
            hand.Invisible();

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
        HandInit();
    }


    public ItemBox GetHotkeyBox()
    {
        return hand;
    }

    public void SetHotkey(int key)
    {
        if(hand != null)
            hand.Invisible();
        hand = inventoryItem[key];
        HandInit();
    }

    public void HandInit()
    {
        if(hand != null)
        {
            hand.Visible(itemPosition);
        }
    }

    public void BoxSetting()
    {
        for(int i = 0; i < inventoryItem.Length; i++)
        {
            inventoryItem[i].Setting();
        }
    }
}
