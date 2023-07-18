using UnityEngine;
using UnityEngine.UI;
using System;
using static UnityEditor.Progress;

public class CharacterEquip : MonoBehaviour
{
    private Unit user;
    public ItemBox[] itemBoxes;
    public Text statText;
    private Stat stat;

    public void Init()
    {
        user = GetComponent<Unit>();
        stat = GetComponent<Stat>();
        for (int i = 0; i < itemBoxes.Length; i++)
        {
            itemBoxes[i].equip = StatEquip;
            itemBoxes[i].litf = StatLift;
            itemBoxes[i].SetPopD(StatText);
            itemBoxes[i].SetPutD(StatText);
        }
    }

    public EquipItem GetWeapon()
    {
        for (int i = 0; i < itemBoxes.Length; i++)
        {
            if (itemBoxes[i].type == ITEM_TYPE.WEAPON)
            {
                if (itemBoxes[i]._items.Count > 0)
                {
                    return itemBoxes[i].GetComponent<EquipItem>();
                }
                break;
            }
        }
        return null;
    }

    public void Visible()
    {
        if (user != null && GetWeapon() != null)
        {
            GetWeapon().transform.SetParent(user.itemPosition, false);
            GetWeapon().transform.localEulerAngles = Vector3.zero;
            GetWeapon().transform.localPosition = Vector3.zero;
            GetWeapon().gameObject.SetActive(true);
        }
    }

    public void Invisible()
    {
        if (GetWeapon() != null)
        {
            GetWeapon().gameObject.SetActive(false);
        }
    }

    public void StatText()
    {
        if(statText != null)
            stat.SetText(statText);
    }

    public void StatEquip(EquipItem item)
    {
        stat.ItemEquip(item);
    }

    public void StatLift(EquipItem item)
    {
        stat.ItemLift(item);
    }
}
