using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidManager : MonoBehaviour
{
    private Player player;
    private TopographyParent topographyParent;
    private List<FluidBlock> blocks = new List<FluidBlock>();
    private Coroutine fluidList = null;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        topographyParent = FindObjectOfType<TopographyParent>();

        player.AddInstall(FluidCheck);
        player.AddMiningAction(FluidCheck);
    }


    public void FluidCheck(Vector3Int vector3)
    {
        //앞 뒤 좌 우 위 아래 액체인지 체크하고 액체라면 흐르게
        Block block = topographyParent.GetBlock(vector3);
        if (block != null)
        {
            if (block.GetComponent<FluidBlock>() != null && !blocks.Contains(block.GetComponent<FluidBlock>()))
            {
                blocks.Add(block.GetComponent<FluidBlock>());
                if(fluidList == null)
                {
                    fluidList = StartCoroutine(FluidList());
                }
            }
        }
    }

    private IEnumerator FluidList()
    {
        while(blocks.Count > 0)
        {
            yield return new WaitForSeconds(1);

            for (int i = 0; i < blocks.Count; i++)
            {
                //흐르게
                //흐른 블록도 list에 추가
                //더이상 흐르지 않으면 remove
                if(!blocks[i].Fluid(this))
                {
                    blocks.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
