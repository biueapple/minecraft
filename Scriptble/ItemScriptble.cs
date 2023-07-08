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
}
