using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkills : MonoBehaviour
{
    public GameObject skillTreeSelectWindow;
    private UIManager uIManager;
    private List<SkillTree> skillTrees = new List<SkillTree>();
    public int point;

    private void Start()
    {
        uIManager = FindObjectOfType<UIManager>();
        SetSkillTrees(FindObjectOfType<SkillManager>().trees);

    }

    public void SetSkillTrees(SkillTree[] tree)
    {
        for(int i = 0; i < tree.Length; i++)
        {
            skillTrees.Add(Instantiate(tree[i], FindObjectOfType<Canvas>().transform));
            skillTrees[i].Init(this);
        }
    }

    public void OpenSkillTree(int index)
    {
        if (index >= 0 && index < skillTrees.Count)
        {
            uIManager.OpenUI(skillTrees[index].gameObject);
        }
    }
}
