using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CratfingBoxBlock : Block
{

    void Start()
    {
        
    }


    void Update()
    {

    }

    public override void RightClick(Player player)
    {
        player.OpenCrafting_9x9();
    }
}
