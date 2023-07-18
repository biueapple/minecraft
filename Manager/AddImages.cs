using UnityEngine;

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
                return Instantiate(allItems[i]).Init();
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
                return Instantiate(allBlocks[i]).Init();
            }
        }
        return null;
    }

    public Block CreateBlock(Block block)
    {
        for (int i = 0; i < allBlocks.Length; i++)
        {
            if (allBlocks[i].itemScriptble.GetCode() == block.itemScriptble.GetCode())
            {
                return Instantiate(allBlocks[i]).Init();
            }
        }
        return null;
    }

    public Block GetDataBlock(_ITEMCODE code)
    {
        for (int i = 0; i < allBlocks.Length; i++)
        {
            if (allBlocks[i].itemScriptble.GetCode() == code)
            {
                return allBlocks[i];
            }
        }
        return null;
    }

    public Item GetDataItem(_ITEMCODE code)
    {
        for (int i = 0; i < allItems.Length; i++)
        {
            if (allItems[i].scriptble.GetCode() == code)
            {
                return allItems[i];
            }
        }
        return null;
    }
}
