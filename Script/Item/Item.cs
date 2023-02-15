using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : MonoBehaviour
{
    public ItemScriptble scriptble;
    public BlockScriptble blockScriptble;

    private void Start()
    {
        //GetComponent<Renderer>().material.SetTexture()
    }

    public virtual bool RightClick(Player player, Decryption decryption)
    {
        return false;
    }

    public void Installation(Vector3 position)      //설치
    {
        Block block = FindObjectOfType<AddImages>().CreateBlock(scriptble.GetCode());
        if(block != null)
        {
            block.transform.position = position;
            Destroy(this.gameObject);
        }
    }

    public bool BlockCk(Vector3 vector)       //설치가능한지 체크
    {
        Collider[] coll = Physics.OverlapBox(vector, new Vector3(0.3f, 0.3f, 0.3f));
        if(coll.Length > 0)
        {
            return false;
        }
        return true;
    }
}
