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
        //�� �� �� �� �� �Ʒ� ��ü���� üũ�ϰ� ��ü��� �帣��
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
                //�帣��
                //�帥 ��ϵ� list�� �߰�
                //���̻� �帣�� ������ remove
                if(!blocks[i].Fluid(this))
                {
                    blocks.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
