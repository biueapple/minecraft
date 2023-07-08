using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidBlock : Block
{
    protected float durability;               //�������� ������ ������� �ʰ� �������� �������� �ı�
    public float GetDurability()
    {
        return durability;
    }
    public void SetDurabillity(float f)
    {
        durability = f;
    }


    /// <summary>
    /// �ʱ�ȭ durability�� 0����
    /// </summary>
    public override void Init()
    {
        durability = 0;
        GetComponent<Renderer>().material.SetFloat("_Float", -0.6f);
    }


    /// <summary>
    /// ����� �ı��Ҷ� ��� ȣ��
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
            //�տ� �� ����ִٸ� ������� ����� ������ ������ �ľ��ϰ� �ؾ��ϴµ� ���� ��� �ȸ�������� 
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
    /// �������� �ı������� ȣ��
    /// </summary>
    /// <param name="equip">���� ���� �ı��ߴ���</param>
    public virtual void Destruction(Item equip)       //�ı�
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
