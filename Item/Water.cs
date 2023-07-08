using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : InstallItem
{


    /// <summary>
    /// 아이템을 블록으로 설치할때 호출
    /// </summary>
    /// <param name="position">어디에 설치할건지</param>
    public override bool Installation(Vector3Int position)      //설치
    {
        Block block = FindObjectOfType<AddImages>().GetDataBlock(scriptble.GetCode());

        if (block != null)
        {
            if (FindObjectOfType<TopographyParent>().InstallBlock(position, block, true))
            {
                FindObjectOfType<TopographyParent>().GetBlock(position).GetComponent<FluidBlock>().SetVolume(1);
                return true;
            }
        }

        return false;
    }
}
