using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public BlockScriptble blockScriptble;       //지금 블록
    public ItemScriptble itemScriptble;         //지금 블록의 아이템버전

    public ItemScriptble valueScriptble;        //블록을 파괴하면 배출할 아이템

    public bool isVisible;



    private float durability;               //강도보다 낮으면 상승하지 않고 강도보다 높아지면 파괴
    public float GetDurability()
    {
        return durability;
    }
    public void SetDurabillity(float f)
    {
        durability = f;
    }

    //

    public void Flow()
    {
        if (durability > 0f)
        {
            durability -= Time.deltaTime;
        }
    }

    public void Repetition()
    {
        Flow();
        if(durability >= itemScriptble.GetStrength())
        {
            FindObjectOfType<TopographyParent>().BrokenBlock((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
            Destruction();
        }
    }

    public void Mining(Item item)
    {
        if (item == null)
        {
            if (1 >= itemScriptble.GetStrength())
            {
                durability = (durability + (1 + 1) * Time.deltaTime);
            }
        }
        else if(item.scriptble.GetStrength() >= itemScriptble.GetStrength())
        {
            durability = (durability + (1 + item.scriptble.GetStrength()) * Time.deltaTime);
        }
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
