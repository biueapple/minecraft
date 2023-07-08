using UnityEngine;
using UnityEditor;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.Plastic.Antlr3.Runtime.Tree;

[CustomEditor(typeof(SkillView), true)]
public class Editor_SkillView : Editor
{
    SkillView view;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        view = (SkillView)target;

        if (GUILayout.Button("Lock"))
        {
            view.Lock();
        }

        if (GUILayout.Button("Open"))
        {
            view.Open();
        }

        if (GUILayout.Button("LevelUp"))
        {
            view.LevelUpButton();
        }

        if (GUILayout.Button("LevelDown"))
        {
            view.LevelDownButton();
        }

        //EditorGUI.BeginChangeCheck();
        //if(EditorGUI.EndChangeCheck() )
        //{

        //}

        if (GUI.changed)
        {
            if (view.skill == null)
                view.Lock();
            else
                view.SetSprite();
            Debug.Log("123");
        }
    }
}
