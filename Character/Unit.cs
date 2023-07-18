using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Transform itemPosition;
    protected Stat stat;
    protected CharacterInven inven;
    protected CharacterEquip equip;
    protected CharacterSkills skill;
    protected SkillUI skillUI;

    //
    /// <summary>
    /// 때릴때 발동하는, unit = 누굴 때렸는지 float = 대미지
    /// </summary>
    protected List<Action<Unit, float>> attackDelegate = new List<Action<Unit, float>>();


    public void Init()
    {
        stat = GetComponent<Stat>();
        stat.Init();
        inven = GetComponent<CharacterInven>();
        inven.Init();
        equip = GetComponent<CharacterEquip>();
        skill = GetComponent<CharacterSkills>();
        skillUI = GetComponent<SkillUI>();
    }

    public void Attack()
    {
        if(equip.GetWeapon() != null)
        {
            Collider[] colliders = equip.GetWeapon().Damage_Coll(transform.position);
            for(int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].GetComponent<Unit>() != null && colliders[i].transform != transform)
                {

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

    public Stat GetStat() { return stat; }
    public void AttackInvocation(Unit victim, float figure)
    {
        for(int i = 0; i < attackDelegate.Count; i++)
        {
            attackDelegate[i](victim, figure);
        }
    }
    public CharacterInven GetInven() {  return inven; }
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
}
