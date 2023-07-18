using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private CharacterSkills characterSkills;
    public Skill skill;

    public SkillView[] Next;
    public SkillView[] Previous;

    private Button plus;
    private Button minus;
    private Text text;
    public int condition;

    public void Init(CharacterSkills skills)
    {
        characterSkills = skills;
        text = transform.GetChild(2).GetComponent<Text>();
    }

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
        if(skill != null && skill.level != 0)
        {
            for(int i = 0; i < skill.level;)
            {
                LevelDownButton();
            }
        }

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

        //plus.onClick.RemoveAllListeners();
        //minus.onClick.RemoveAllListeners();
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

        //plus.onClick.RemoveAllListeners();
        //minus.onClick.RemoveAllListeners();

        //plus.onClick.AddListener(skill.LevelUp);
        //minus.onClick.AddListener(skill.LevelDown);

        //for(int i = 0; i < Next.Length; i++)
        //{
        //    plus.onClick.AddListener(Next[i].ConditionCheck);
        //    minus.onClick.AddListener(Next[i].ConditionCheck);
        //}
    }


    public void LevelUpButton()
    {
        if (characterSkills != null)
        {
            if (characterSkills.point == 0)
                return;
            characterSkills.point--;
        }
            
        skill.LevelUp();
        for (int i = 0; i < Next.Length; i++)
        {
            Next[i].ConditionCheck();
        }
        text.text = skill.level.ToString();
    }

    public void LevelDownButton()
    {
        if (characterSkills != null)
            characterSkills.point++;

        skill.LevelDown();
        for (int i = 0; i < Next.Length; i++)
        {
            Next[i].ConditionCheck();
        }
        text.text = skill.level.ToString();
    }

    public void SetSprite()
    {
        GetComponent<Image>().sprite = skill.sp;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(skill.level > 0)
            FindObjectOfType<SkillUI>().MouseDownSkillV(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerEnter != null && eventData.pointerEnter.GetComponent<SkillBox>() != null)
            FindObjectOfType<SkillUI>().MouseUpSkillV(eventData.pointerEnter.GetComponent<SkillBox>());
        else
            FindObjectOfType<SkillUI>().MouseUpSkillV(null);
    }
}
