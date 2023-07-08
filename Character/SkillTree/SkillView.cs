using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillView : MonoBehaviour
{
    public Skill skill;

    public SkillView[] Next;
    public SkillView[] Previous;

    private Button plus;
    private Button minus;
    public int condition;

    public void ConditionCheck()
    {
        int a = 0;
        for(int i = 0; i < Previous.Length; i++)
        {
            a += Previous[i].skill.level;
        }
        if(a >= condition)
        {
            Open();
            return;
        }
        Lock();
    }

    public void Lock()
    {
        GetComponent<Image>().color = Color.black;
        if(plus == null)
        {
            plus = transform.GetChild(0).GetComponent<Button>();
        }
        if (minus == null)
        {
            minus = transform.GetChild(1).GetComponent<Button>();
        }
        plus.gameObject.SetActive(false);
        minus.gameObject.SetActive(false);

        plus.onClick.RemoveAllListeners();
        minus.onClick.RemoveAllListeners();
    }

    public void Open()
    {
        if (skill == null)
            return;

        GetComponent<Image>().color = Color.white;
        if (plus == null)
        {
            plus = transform.GetChild(0).GetComponent<Button>();
        }
        if (minus == null)
        {
            minus = transform.GetChild(1).GetComponent<Button>();
        }
        plus.gameObject.SetActive(true);
        minus.gameObject.SetActive(true);

        plus.onClick.RemoveAllListeners();
        minus.onClick.RemoveAllListeners();

        plus.onClick.AddListener(skill.LevelUp);
        minus.onClick.AddListener(skill.LevelDown);

        for(int i = 0; i < Next.Length; i++)
        {
            plus.onClick.AddListener(Next[i].ConditionCheck);
            minus.onClick.AddListener(Next[i].ConditionCheck);
        }
    }


    public void LevelUpButton()
    {
        skill.LevelUp();
        for (int i = 0; i < Next.Length; i++)
        {
            Next[i].ConditionCheck();
        }
    }

    public void LevelDownButton()
    {
        skill.LevelDown();
        for (int i = 0; i < Next.Length; i++)
        {
            Next[i].ConditionCheck();
        }
    }

    public void SetSprite()
    {
        GetComponent<Image>().sprite = skill.sp;
    }
}
