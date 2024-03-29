using UnityEngine;

public enum ITEM_BOX_TYPE
{
    DUPLICATE,
    UNDUPLICATE,
}
public enum ITEM_TYPE
{
    NONE,
    CONSUM,
    HEAD,
    UPPER,
    LOWER,
    FOOT,
    WEAPON,
    ACCESSORIES,

}

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
    private int strength;       //����
    public int GetStrength()
    {
        return strength;
    }

    [SerializeField]
    private Sprite itemSprite;
    public Sprite GetSprite()
    {
        return itemSprite;
    }

    [SerializeField]
    private ITEM_TYPE type;
    public ITEM_TYPE GetItemType() { return type; }

    [SerializeField]
    private ITEM_BOX_TYPE boxType;
    public ITEM_BOX_TYPE GetBoxType() { return boxType; }
}
