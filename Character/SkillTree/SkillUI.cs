using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    SkillBox startB = null;
    SkillView startV = null;
    Coroutine coroutine = null;
    public Image i;
    public SkillBox[] hotkeys;

    private void Start()
    {
    }


    public void MouseDownSkillV(SkillView box)
    {
        startV = box;
        i.transform.position = Input.mousePosition;
        i.gameObject.SetActive(true);
        i.sprite = startV.skill.sp;
        i.transform.SetAsLastSibling();

        if(coroutine != null )
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(MouseFollow());
    }
   
    public void MouseUpSkillV(SkillBox box)
    {
        if(box != null)
        {
            box.SetSkill(startV.skill);
        }

        i.gameObject.SetActive(false);
        if (coroutine != null)
            StopCoroutine(coroutine);
    }

    public void MouseDownSkill(SkillBox box)
    {
        startB = box;
        i.transform.position = Input.mousePosition;
        i.gameObject.SetActive(true);
        i.sprite = startB.GetSkill().sp;
        i.transform.SetAsLastSibling();

        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(MouseFollow());
    }

    public void MouseUpSkill(SkillBox box)
    {
        if (box != null)
        {
            box.SetSkill(startB.GetSkill());
            startB.SetSkill(null);
        }
        else
        {
            startB.SetSkill(null);
        }

        i.gameObject.SetActive(false);
        if (coroutine != null)
            StopCoroutine(coroutine);
    }

    private IEnumerator MouseFollow()
    {
        while (true)
        {
            i.transform.position = Input.mousePosition;

            yield return null;
        }
    }

    public void HotkeyState(bool b)
    {
        hotkeys[0].transform.parent.gameObject.SetActive(b);
    }
}
