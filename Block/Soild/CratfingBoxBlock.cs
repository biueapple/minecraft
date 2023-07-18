using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CratfingBoxBlock : InterBlock
{
    public GameObject crafting3x3;
    private GameObject crafting;
    void Start()
    {
        
    }


    void Update()
    {

    }

    public override bool RightClick(Player player)
    {
        FindObjectOfType<UIManager>().OpenUI(player.character.GetInven().inventoryWindow);
        FindObjectOfType<UIManager>().OpenUI(crafting);
        player.GetPlayerMouse().isMove = false;

        return true;
    }

    public override Block Init()
    {
        if (crafting == null)
        {
            crafting = Instantiate(crafting3x3, FindObjectOfType<Canvas>().transform);
            crafting.SetActive(false);
        }
        return base.Init();
    }
}
