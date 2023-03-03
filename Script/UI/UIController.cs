using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class UIController : MonoBehaviour
{
    bool inGame;

    //GraphicRaycaster
    GraphicRaycaster m_gr;
    PointerEventData m_ped;
    List<RaycastResult> results;

    public CraftingBox craftingBox_1x1;
    public CraftingBox craftingBox_9x9;
    private CraftingBox craftingBox;
    public Canvas canvas;

    private PlayerInven playerInven;
    private PlayerMouse playerMouse;

    public List<GameObject> uiOpens;

    public GameObject inventory;
    public GameObject crafting;
    public GameObject option;
    void Start()
    {
        m_ped = new PointerEventData(null);
        m_gr = canvas.GetComponent<GraphicRaycaster>();
        results = new List<RaycastResult>();
        playerInven = FindObjectOfType<PlayerInven>();
        playerMouse = FindObjectOfType<PlayerMouse>();
    }



    void Update()
    {
        GraphicRay();
        InputKeys();
    }

    public void InputKeys()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            InventoryKey();
            CraftingKey(1);
            if(uiOpens.Count <= 0)
            {
                playerMouse.isMove = true;
            }
            else
            {
                playerMouse.isMove = false;
            }
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(uiOpens.Count > 0)
            {
                uiOpens[0].SetActive(false);
                uiOpens.RemoveAt(0);
            }
            else
            {
                option.SetActive(true);
                uiOpens.Add(option);
            }
            if (uiOpens.Count <= 0)
            {
                playerMouse.isMove = true;
            }
            else
            {
                playerMouse.isMove = false;
            }
        }
    }

    public void GraphicRay()                                                //player에서 호출
    {
        if(inGame)
        {

        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_ped.position = Input.mousePosition;

                m_gr.Raycast(m_ped, results);

                if (results.Count > 0)
                {
                    if (results[0].gameObject.transform.GetComponent<ItemBox>() != null)        //아이템박스를 클릭했다
                    {
                        if (results[0].gameObject.transform.parent.GetComponent<CraftingBox>() != null)      //조합대 안에 있는 아이템 박스였다
                        {
                            playerInven.BoxInputLeftDown(results[0].gameObject.transform.GetComponent<ItemBox>());

                            craftingBox.Comparison();

                            ListSwap(uiOpens, 0, FindIndex(uiOpens, crafting));
                        }
                        else if (results[0].gameObject.transform.parent.name.Equals("ResultBack"))           //조합대 결과창
                        {
                            if(playerInven.BoxInputLeftDown(results[0].gameObject.transform.GetComponent<ItemBox>()))
                                craftingBox.FinishComparison();
                        }
                        else if (results[0].gameObject.transform.parent.name.Equals("InventoryBack"))        //인벤토리
                        {
                            playerInven.BoxInputLeftDown(results[0].gameObject.transform.GetComponent<ItemBox>());

                            ListSwap(uiOpens, 0, FindIndex(uiOpens, inventory));
                        }
                    }
                }
            }   //get mouse down
            else if (Input.GetMouseButtonUp(0))
            {
                m_ped.position = Input.mousePosition;

                m_gr.Raycast(m_ped, results);


                if (results.Count > 0)
                {
                    if (results[0].gameObject.transform.GetComponent<ItemBox>() != null)
                    {
                        if (results[0].gameObject.transform.parent.GetComponent<CraftingBox>() != null)      //조합대 안에 있는 아이템 박스였다
                        {
                            playerInven.BoxInputLeftUp(results[0].gameObject.transform.GetComponent<ItemBox>());
                            craftingBox.Comparison();
                        }
                        else if (results[0].gameObject.transform.parent.name.Equals("ResultBack"))           //조합대 결과창
                        {
                            playerInven.BoxInputLeftUp(null);
                        }
                        else if (results[0].gameObject.transform.parent.name.Equals("InventoryBack"))        //인벤토리
                        {
                            playerInven.BoxInputLeftUp(results[0].gameObject.transform.GetComponent<ItemBox>());
                        }
                    }
                }
                else
                {
                    playerInven.BoxInputLeftUp(null);
                }
            }   //get mouse up
            else if(Input.GetMouseButtonDown(1))
            {
                m_ped.position = Input.mousePosition;

                m_gr.Raycast(m_ped, results);


                if (results.Count > 0)
                {
                    if (results[0].gameObject.transform.GetComponent<ItemBox>() != null)
                    {
                        if (results[0].gameObject.transform.parent.GetComponent<CraftingBox>() != null)      //조합대 안에 있는 아이템 박스였다
                        {
                            playerInven.BoxInputRightDown(results[0].gameObject.transform.GetComponent<ItemBox>());
                            craftingBox.Comparison();

                            ListSwap(uiOpens, 0, FindIndex(uiOpens, crafting));
                        }
                        else if (results[0].gameObject.transform.parent.name.Equals("ResultBack"))           //조합대 결과창
                        {

                        }
                        else if (results[0].gameObject.transform.parent.name.Equals("InventoryBack"))        //인벤토리
                        {
                            playerInven.BoxInputRightDown(results[0].gameObject.transform.GetComponent<ItemBox>());

                            ListSwap(uiOpens, 0, FindIndex(uiOpens, inventory));
                        }
                    }
                }
                else
                {
                    
                }
            }   //get mouse down
        }
        results.Clear();
    }

    public void InventoryKey()
    {
        if(uiOpens.Contains(inventory))
        {
            inventory.SetActive(false);
            uiOpens.Remove(inventory);
        }
        else
        {
            inventory.SetActive(true);
            uiOpens.Add(inventory);
        }
    }
    public void CraftingKey(int index)
    {
        if (uiOpens.Contains(crafting))
        {
            crafting.SetActive(false);
            uiOpens.Remove(crafting);

            craftingBox = null;
            crafting = null;
        }
        else
        {
            if(index == 1)
            {
                craftingBox = craftingBox_1x1;
            }
            else if(index == 2)
            {
                craftingBox = craftingBox_9x9;
            }
            crafting = craftingBox.gameObject;

            crafting.SetActive(true);
            uiOpens.Add(crafting);
        }
    }


    public T GetGraphicRay<T>()
    {
        results.Clear();

        m_ped.position = Input.mousePosition;

        m_gr.Raycast(m_ped, results);

        if(results.Count > 0)
        {
            return results[0].gameObject.GetComponent<T>();
        }
        return default(T);
    }

    public void ListSwap<T>(List<T> list, int index1, int index2)
    {
        if (index2 == -1)
            return;

        if(index1 < 0 || index2 >= list.Count)
        {
            return;
        }

        T item = list[index1];
        list[index1] = list[index2];
        list[index2] = item;

    }
    public int FindIndex<T>(List<T> list, T obj)
    {
        for(int i = 0; i < list.Count; i++)
        {
            if (list[i].Equals(obj))
            {
                return i;
            }
        }
        return -1;
    }
}
