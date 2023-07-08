using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SkillTree), true)]
public class Editor_SkillTree : Editor
{
    SkillTree tree;

    public override void OnInspectorGUI()
    {
        tree = (SkillTree)target;

        if (GUILayout.Button("Draw Line"))
        {
            tree.AllLineDraw();
        }

    }
}
