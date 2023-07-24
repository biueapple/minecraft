using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillBox : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Skill skill;
    Image timer;

    private void Start()
    {
        timer = transform.GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        if(skill != null)
        { 
            timer.fillAmount = skill.GetTimer();
        }
    }

    public void SetSkill(Skill s)
    {
        skill = s;
        Setting();
    }

    public Skill GetSkill()
    {
        return skill;
    }

    public void Setting()
    {
        if (skill != null)
        {
            GetComponent<Image>().sprite = skill.sp;
        }
        else
        {
            GetComponent<Image>().sprite = null;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        FindObjectOfType<SkillUI>().MouseDownSkill(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerEnter != null && eventData.pointerEnter.GetComponent<SkillBox>() != null)
            FindObjectOfType<SkillUI>().MouseUpSkill(eventData.pointerEnter.GetComponent<SkillBox>());
        else
            FindObjectOfType<SkillUI>().MouseUpSkill(null);
    }
}
