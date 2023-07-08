using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public BlockScriptble blockScriptble;       //지금 블록
    public ItemScriptble itemScriptble;         //지금 블록의 아이템버전

    public ItemScriptble valueScriptble;        //블록을 파괴하면 배출할 아이템

    public bool isVisible;

    public virtual void RightClick(Player player)
    {
        
    }

    public Vector3Int GetPosition() { return new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z); }

    public void ApplyTexture()
    {
        GetComponent<Renderer>().material.SetTexture("_Texture2D", blockScriptble.GetTexture());
    }


    /// <summary>
    /// 안보이게
    /// </summary>
    public void SetInvisible()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 보이게
    /// </summary>
    public void SetVisible()
    {
        if (isVisible)
        {
            gameObject.SetActive(true);
        }
    }

    public virtual void Init() { }
}
