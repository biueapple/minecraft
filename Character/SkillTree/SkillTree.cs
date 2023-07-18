using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public List<SkillView> SkillViews = new List<SkillView>();
    public List<Line> lines = new List<Line>();
    public Line line;
    private CharacterSkills skill;

    public void Init(CharacterSkills skills)
    {
        skill = skills;
        for(int i = 0; i < SkillViews.Count; i++)
        {
            SkillViews[i].Init(skill);
        }
    }






    public void AddSkillView(SkillView view)
    {
        SkillViews.Add(view);
    }

    public void LineDraw(SkillView from, SkillView to)
    {
        if(from == null || to == null) return;

        for(int i = 0; i < lines.Count; i++)
        {
            if (lines[i].Contain(from,to))
            {
                //이미 있는경우
                return;
            }
        }
        
        lines.Add(Instantiate(line, transform));
        lines[lines.Count - 1].SetLine(from, to);
        lines[lines.Count - 1].transform.SetSiblingIndex(0);
    }

    public void AllLineDraw()
    {
        for(int i = 0; i < SkillViews.Count; i++)
        {
            for(int j = 0; j < SkillViews[i].Next.Length; j++)
            {
                LineDraw(SkillViews[i], SkillViews[i].Next[j]);
            }

            for (int j = 0; j < SkillViews[i].Previous.Length; j++)
            {
                LineDraw(SkillViews[i].Previous[j], SkillViews[i]);
            }
        }
    }
}
