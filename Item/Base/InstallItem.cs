using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallItem : Item
{

    /// <summary>
    /// �������� ������� ��ġ�Ҷ� ȣ��
    /// </summary>
    /// <param name="position">��� ��ġ�Ұ���</param>
    public virtual bool Installation(Vector3Int position)      //��ġ
    {
        Block block = FindObjectOfType<AddImages>().GetDataBlock(scriptble.GetCode());

        if (block != null)
        {
            if (FindObjectOfType<TopographyParent>().InstallBlock(position, block, true))
            {
                return true;
            }
        }

        return false;
    }

    public Vector3Int CalculatePosition(Player player, Decryption decryption)
    {
        Vector3Int nomal = new Vector3Int(Mathf.RoundToInt(decryption.nomal.x), Mathf.RoundToInt(decryption.nomal.y), Mathf.RoundToInt(decryption.nomal.z));
        Vector3Int posi = new Vector3Int(Mathf.RoundToInt(decryption.Obj.transform.position.x), Mathf.RoundToInt(decryption.Obj.transform.position.y), Mathf.RoundToInt(decryption.Obj.transform.position.z));

        if (decryption.Obj != null)
        {
            if (decryption.HitDistance <= player.nomal_Distance_Item)
            {
                if (BlockCk(nomal + posi))
                {
                    return nomal + posi;
                }
            }
        }

        return default(Vector3Int);
    }
}
