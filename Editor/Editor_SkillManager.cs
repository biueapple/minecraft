using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(SkillManager), true)]
public class Editor_SkillManager : Editor
{
    SkillManager s;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        s = (SkillManager)target;

        if (GUILayout.Button("Create SkillTree"))
        {
            s.CreateSkillTree();
        }

        if (GUILayout.Button("Create SkillView"))
        {
            s.CreateSkillView();
        }

        if (GUILayout.Button("초기화"))
        {
            s.tree = null;
        }

        if(GUILayout.Button("테스트"))
        {
            
        }
    }
}
