using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "itemData", menuName = "ScriptableObj/CreateItem", order = int.MaxValue)]
[System.Serializable]
public class ItemScriptble : ScriptableObject
{
    [SerializeField]
    private _ITEMCODE itemCode;
    public _ITEMCODE GetCode()
    {
        return itemCode;
    }

    //

    [SerializeField]
    private string itemName;
    public string GetName()
    {
        return itemName;
    }

    //

    [SerializeField]
    private Sprite itemSprite;
    public Sprite GetSprite()
    {
        return itemSprite;
    }

    //

    
    //[SerializeField]
    //private int craftValue;
    //public int GetCraftValue()
    //{
    //    return craftValue;
    //}
    //public void SetCraftValue(int value)
    //{
    //    craftValue = value;
    //}
    ////

    //[SerializeField]
    //private int _hashcode;
    //public int Get_HashCode()
    //{
    //    return _hashcode;
    //}
    //public void Set_HashCode(int hash)
    //{
    //    _hashcode = hash;
    //}
}
