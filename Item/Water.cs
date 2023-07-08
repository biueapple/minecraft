using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : InstallItem
{


    /// <summary>
    /// �������� ������� ��ġ�Ҷ� ȣ��
    /// </summary>
    /// <param name="position">��� ��ġ�Ұ���</param>
    public override bool Installation(Vector3Int position)      //��ġ
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
