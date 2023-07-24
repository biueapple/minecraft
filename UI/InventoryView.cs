using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    UIManager uiManager;
    private CharacterInven inventory;
     ItemBox startbox = null;
     ItemBox endbox = null;
    public Image i;
    public Image[] hotkeys;
    Coroutine coroutine = null;

    public void Init()
    {
        uiManager = FindObjectOfType<UIManager>();
        inventory = GetComponent<CharacterInven>();
        for(int i = 0; i < hotkeys.Length; i++)
        {
            inventory.itemBoxes[i].SetPopD(Change);
            inventory.itemBoxes[i].SetPutD(Change);
        }
    }

    public void MouseDownItemBox(ItemBox box)
    {
        startbox = box;
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(TEST());
        i.transform.position = Input.mousePosition;
        i.gameObject.SetActive(true);
        i.sprite = startbox.GetItem().scriptble.GetSprite();
        i.transform.SetAsLastSibling();
    }

    private IEnumerator TEST()
    {
         while(true)
        {
            yield return null;

            if(Input.GetMouseButtonUp(0))
            {
                LeftClick();

                break;
            }
            else if(Input.GetMouseButtonDown(1))
            {
                RightClick();
                if(startbox.GetItem() == null)
                {
                    break;
                }
            }
            i.transform.position = Input.mousePosition;
        }
        i.gameObject.SetActive(false);
        coroutine = null;
    }

    public void LeftClick()
    {
        endbox = uiManager.GetGraphicRay<ItemBox>();
        if (endbox != null)
        {
            if (inventory.Duplicate(endbox, startbox))
            {

            }
            else
            {
                if (inventory.Swap(startbox, endbox))
                {

                }
                else
                {
                    //실패가 아니라 아이템박스는 맞는데 인벤창이 아닌 아이템박스 (조합창 아이템 박스)인거임

                }
            }
        }
        else
        {
            inventory.ItemsThrow(startbox);
        }
    }

    public void RightClick()
    {
        endbox = uiManager.GetGraphicRay<ItemBox>();
        if (endbox != null)
        {
            if (inventory.Give(endbox, startbox))
            {

            }
            else
            {
                if (inventory.Swap(startbox, endbox))
                {

                }
                else
                {

                }
            }
        }
        else
        {
            inventory.ItemThrow(inventory.GetIndex(startbox));
        }
    }

    public void Change()
    {
        for (int i = 0; i < hotkeys.Length; i++)
        {
            if (inventory.itemBoxes[i].GetItem() != null)
            {
                hotkeys[i].sprite = inventory.itemBoxes[i].GetItem().scriptble.GetSprite();
                hotkeys[i].transform.GetChild(0).GetComponent<Text>().text = inventory.itemBoxes[i].GetTextString();
            }
            else
            {
                hotkeys[i].sprite = null;
                hotkeys[i].transform.GetChild(0).GetComponent<Text>().text = "";
            }
        }
    }

    public void HotkeyState(bool b)
    {
        hotkeys[0].transform.parent.gameObject.SetActive(b);
    }
}
