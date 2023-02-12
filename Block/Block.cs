using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public BlockScriptble blockScriptble;       //지금 블록
    public ItemScriptble itemScriptble;         //지금 블록의 아이템버전

    public ItemScriptble valueScriptble;        //블록을 파괴하면 배출할 아이템

    public bool isVisible;

    void Start()
    {
        
    }


    void Update()
    {
           
    }

    public void ApplyTexture()
    {
        GetComponent<Renderer>().material.SetTexture("_Texture2D", blockScriptble.GetTexture());
    }

    public void Destruction()       //파괴
    {
        Item item = FindObjectOfType<AddImages>().CreateItem(valueScriptble.GetCode());
        item.transform.position = transform.position;
        Destroy(this.gameObject);
    }

    public void SetInvisible()
    {
        gameObject.SetActive(false);
    }
    public void SetVisible()
    {
        if (isVisible)
        {
            gameObject.SetActive(true);
        }
    }
}
