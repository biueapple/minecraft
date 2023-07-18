using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterBlock : SolidBlock
{


    public virtual bool RightClick(Player player)
    {
        return false;
    }
}
