using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemRatingManger
{
    public static void Rating(EquipItem equip, ITEM_RATING min, ITEM_RATING max)
    {
        ITEM_RATING rating = (ITEM_RATING)Random.Range((int)min, (int)max + 1);
        equip.rating = rating;
    }

    public static void SetStat(EquipItem equip)
    {
        switch (equip.rating)
        {
            case ITEM_RATING.COMMON:
                Common(equip);
                break;
        }
    }

    public static void Common(EquipItem equip)
    {
        equip.damage = Random.Range(equip.level * 10, equip.level * 10 + 10);
    }
}
