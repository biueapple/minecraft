using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidBlock : Block
{
    protected float durability;               //강도보다 낮으면 상승하지 않고 강도보다 높아지면 파괴
    public float GetDurability()
    {
        return durability;
    }
    public void SetDurabillity(float f)
    {
        durability = f;
    }


    /// <summary>
    /// 초기화 durability를 0으로
    /// </summary>
    public override void Init()
    {
        durability = 0;
        GetComponent<Renderer>().material.SetFloat("_Float", -0.6f);
    }


    /// <summary>
    /// 블록을 파괴할때 계속 호출
    /// </summary>
    /// <param name="item"></param>
    public void Mining(Item item)
    {
        if (item == null)
        {
            if (1 >= blockScriptble.GetStrength())
            {
                durability = (durability + (1 + 1) * Time.deltaTime);
                GetComponent<Renderer>().material.SetFloat("_Float", durability * 1.2f - 0.6f);
            }
        }
        else
        {
            //손에 뭘 들고있다면 장비인지 장비의 강도가 얼마인지 파악하고 해야하는데 아직 장비를 안만들었으니 
            durability = (durability + (1 + 1) * Time.deltaTime);
            GetComponent<Renderer>().material.SetFloat("_Float", durability * 1.2f - 0.6f);
        }
        //else if (item.scriptble.GetStrength() >= itemScriptble.GetStrength())
        //{
        //    durability = (durability + (1 + item.scriptble.GetStrength()) * Time.deltaTime);
        //    GetComponent<Renderer>().material.SetFloat("_Float", durability * 1.2f - 0.6f);
        //}
    }


    /// <summary>
    /// 아이템이 파괴됬을때 호출
    /// </summary>
    /// <param name="equip">무슨 장비로 파괴했는지</param>
    public virtual void Destruction(Item equip)       //파괴
    {
        if (valueScriptble != null)
        {
            Item item = FindObjectOfType<AddImages>().CreateItem(valueScriptble.GetCode());
            FindObjectOfType<TopographyParent>().BrokenBlock((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
            item.Drop(transform.position);
        }

        Destroy(this.gameObject);
    }

    
}
