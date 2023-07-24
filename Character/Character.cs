using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Unit
{
    public Transform itemPosition;
    protected Player player;
    protected CharacterInven inven;
    protected CharacterEquip equip;
    protected CharacterSkills skill;
    protected SkillUI skillUI;

    //
    /// <summary>
    /// 때릴때 발동하는, unit = 누굴 때렸는지 float = 대미지
    /// </summary>
    protected List<Action<Unit, float>> attackDelegate = new List<Action<Unit, float>>();


    public void Init(Player player)
    {
        this.player = player;
        stat = GetComponent<Stat>();
        stat.Init();
        inven = GetComponent<CharacterInven>();
        if (inven != null)
        {
            inven.Init();

            inven.ItemAdd(FindObjectOfType<AddImages>().CreateItem(_ITEMCODE.WOOD));
            inven.ItemAdd(FindObjectOfType<AddImages>().CreateItem(_ITEMCODE.WOODEN_SWORD));
        }

        equip = GetComponent<CharacterEquip>();
        skill = GetComponent<CharacterSkills>();
        skillUI = GetComponent<SkillUI>();
        rigid = GetComponent<Rigidbody>();
    }

    public void SkillUse(int index)
    {
        if(skillUI.hotkeys[index].GetSkill() != null)
        {
            skillUI.hotkeys[index].GetSkill().Use(this, null);
        }
    }

    public void Attack()
    {
        if (equip.GetWeapon() != null)
        {
            Collider[] colliders = equip.GetWeapon().Damage_Coll(transform);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].GetComponent<Unit>() != null && colliders[i].transform != transform)
                {
                    colliders[i].GetComponent<Unit>().GetStat().Be_Attacked_AD(equip.GetWeapon().damage + stat.GetAd(), 0, 0, this);
                    colliders[i].GetComponent<Unit>().Hit(transform);
                    for (int j = 0; j < attackDelegate.Count; j++)
                    {
                        attackDelegate[i](colliders[i].GetComponent<Unit>(), equip.GetWeapon().damage + stat.GetAd());
                    }
                }
            }
        }
    }

    public void BattleMode()
    {
        inven.HotkeyOff();
        inven.GetHand().Invisible();

        equip.Visible();
        skillUI.HotkeyState(true);

    }
    public void PeaceMode()
    {
        inven.HotkeyOn();
        inven.GetHand().Visible();

        equip.Invisible();
        skillUI.HotkeyState(false);

    }

    public void AttackInvocation(Unit victim, float figure)
    {
        for (int i = 0; i < attackDelegate.Count; i++)
        {
            attackDelegate[i](victim, figure);
        }
    }
    public CharacterInven GetInven() { return inven; }
    public CharacterEquip GetEquip() { return equip; }
    public CharacterSkills GetSkillManager() { return skill; }
    public SkillUI GetSkillUI() { return skillUI; }
    public void ItemEquip(EquipItem item)
    {
        stat.ItemEquip(item);
    }
    public void ItemLift(EquipItem item)
    {
        stat.ItemLift(item);
    }
    public Player GetPlayer() { return player; }
}
