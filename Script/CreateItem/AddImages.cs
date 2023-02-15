using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddImages : MonoBehaviour
{
    public Item[] allItems;
    
    public Block[] allBlocks;

    private void Awake()
    {
        Init();
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void Init()
    {
        allItems = Resources.LoadAll<Item>("AllItems");
        allBlocks = Resources.LoadAll<Block>("AllBlocks");
    }

    public Item CreateItem(_ITEMCODE code)
    {
        for(int i = 0; i < allItems.Length; i++)
        {
            if (allItems[i].scriptble.GetCode() == code)
            {
                return Instantiate(allItems[i]);
            }
        }
        return null;
    }

    public Block CreateBlock(_ITEMCODE code)
    {
        for (int i = 0; i < allBlocks.Length; i++)
        {
            if (allBlocks[i].itemScriptble.GetCode() == code)
            {
                return Instantiate(allBlocks[i]);
            }
        }
        return null;
    }
}
