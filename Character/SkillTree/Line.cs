using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    private SkillView from;
    private SkillView to;

    public void SetLine(SkillView f, SkillView t)
    {
        transform.SetSiblingIndex(0);
        from = f;
        to = t;

        Vector2 position = from.transform.localPosition + (to.transform.localPosition - from.transform.localPosition) * 0.5f;
        float size = Vector2.Distance(to.transform.localPosition, from.transform.localPosition);
        Vector2 direction = to.GetComponent<RectTransform>().anchoredPosition - from.GetComponent<RectTransform>().anchoredPosition;
        float angle = Vector2.Angle(Vector2.right, direction) - 90;

        transform.GetComponent<RectTransform>().anchoredPosition = position;
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(30, size);
        transform.localEulerAngles = new Vector3(0, 0, angle);
    }

    public bool Contain(SkillView f, SkillView t)
    {
        if(f == from && t == to) return true;
        return false;
    }
}
