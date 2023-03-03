using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : Item
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool RightClick(Player player, Decryption decryption)
    {
        Vector3Int nomal = new Vector3Int(Mathf.RoundToInt(decryption.nomal.x), Mathf.RoundToInt(decryption.nomal.y), Mathf.RoundToInt(decryption.nomal.z));
        Vector3Int posi = new Vector3Int(Mathf.RoundToInt(decryption.Obj.transform.position.x), Mathf.RoundToInt(decryption.Obj.transform.position.y), Mathf.RoundToInt(decryption.Obj.transform.position.z));

        if (decryption.Obj != null)
        {
            if (decryption.HitDistance <= player.nomal_Distance_Item)
            {
                if (BlockCk(nomal + posi))
                {
                    Installation(nomal + posi);        //dec의 nomal의 반올림
                    return true;
                }
            }
        }

        return false;
    }
}
