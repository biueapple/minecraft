using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidBlock : Block
{
    protected float volume = 0;

    public void SetVolume(float f)
    {
        volume = f;
        Init();
    }

    public float GetVolume()
    {
        return volume;
    }

    public override Block Init()
    {
        GetComponent<Renderer>().material.SetFloat("_Float", volume);
        return base.Init();
    }

    /// <summary>
    /// 주변에 흐를 수 있다면 흐르고 true리턴 없다면 false
    /// </summary>
    /// <returns></returns>
    public bool Fluid(FluidManager manager)
    {
        TopographyParent topographyParent = FindObjectOfType<TopographyParent>();
        Vector3Int down = new Vector3Int((int)transform.position.x, (int)transform.position.y - 1, (int)transform.position.z);

        //가장 먼저 아래를 체크해서 먼저 아래로 내려가는지 확인해야할듯
        if (topographyParent.InstallBlock(down, _ITEMCODE.WATER, true) || topographyParent.GetBlock(down).GetComponent<FluidBlock>() != null)
        {
            //밑으로 흐르게

            topographyParent.GetBlock(down).GetComponent<FluidBlock>().SetVolume(volume);
            SetVolume(0);
            manager.FluidCheck(down);
            topographyParent.BrokenBlock(down);
            Destroy(gameObject, 1.5f);

            return false;
        }

        if (volume <= 0.15f)
            return false;

        Vector3Int front = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z + 1);
        Vector3Int back = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z - 1);
        Vector3Int right = new Vector3Int((int)transform.position.x + 1, (int)transform.position.y, (int)transform.position.z);
        Vector3Int left = new Vector3Int((int)transform.position.x - 1, (int)transform.position.y, (int)transform.position.z);
        int ck = 0;

        

        //앞뒤좌우만 체크
        if (topographyParent.InstallBlock(right, _ITEMCODE.WATER, true) || topographyParent.GetBlock(right).GetComponent<FluidBlock>() != null)
        {
            if (topographyParent.GetBlock(right).GetComponent<FluidBlock>().GetVolume() >= volume - 0.15f)
                return false;

            topographyParent.GetBlock(right).GetComponent<FluidBlock>().SetVolume(topographyParent.GetBlock(right).GetComponent<FluidBlock>().GetVolume() + 0.1f);

            SetVolume(volume - 0.1f);
            manager.FluidCheck(right);
            if (volume <= 0.15f)
                return false;
        }
        else ck++;

        if (topographyParent.InstallBlock(left, _ITEMCODE.WATER, true) || topographyParent.GetBlock(left).GetComponent<FluidBlock>() != null)
        {
            if (topographyParent.GetBlock(left).GetComponent<FluidBlock>().GetVolume() >= volume - 0.15f)
                return false;

            topographyParent.GetBlock(left).GetComponent<FluidBlock>().SetVolume(topographyParent.GetBlock(left).GetComponent<FluidBlock>().GetVolume() + 0.1f);

            SetVolume(volume - 0.1f);
            manager.FluidCheck(left);
            if (volume <= 0.15f)
                return false;
        }
        else ck++;

        if (topographyParent.InstallBlock(front, _ITEMCODE.WATER, true) || topographyParent.GetBlock(front).GetComponent<FluidBlock>() != null)
        {
            if (topographyParent.GetBlock(front).GetComponent<FluidBlock>().GetVolume() >= volume - 0.15f)
                return false;

            topographyParent.GetBlock(front).GetComponent<FluidBlock>().SetVolume(topographyParent.GetBlock(front).GetComponent<FluidBlock>().GetVolume() + 0.1f);

            SetVolume(volume - 0.1f);
            manager.FluidCheck(front);
            if (volume <= 0.15f)
                return false;
        }
        else ck++;

        if (topographyParent.InstallBlock(back, _ITEMCODE.WATER, true) || topographyParent.GetBlock(back).GetComponent<FluidBlock>() != null)
        {
            if (topographyParent.GetBlock(back).GetComponent<FluidBlock>().GetVolume() >= volume - 0.15f)
                return false;

            topographyParent.GetBlock(back).GetComponent<FluidBlock>().SetVolume(topographyParent.GetBlock(back).GetComponent<FluidBlock>().GetVolume() + 0.1f);

            SetVolume(volume - 0.1f);
            manager.FluidCheck(back);
            if (volume <= 0.15f)
                return false;
        }
        else ck++;

        if (ck >= 4)
            return false;

        return true;
    }
}
