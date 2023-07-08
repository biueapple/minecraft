using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public Canvas canvas;

    public SkillTree tree;
    public SkillTree treePrefab;

    public SkillView skillViewPrefab;   
    private SkillView skillView;

    private ScrollRect rect;
    public ScrollRect rectPrefab;

    public Line linePrefab;


    public void CreateSkillTree()
    {
        if(tree == null)
        {
            tree = Instantiate(treePrefab, canvas.transform);
            tree.manager = this;
            tree.line = linePrefab;
        }
    }

    public void CreateSkillView()
    {
        if(tree != null)
        {
            skillView = Instantiate(skillViewPrefab, tree.transform);
            skillView.Lock();
            tree.AddSkillView(skillView);
        }
    }

    public void CreateScrollRect()
    {
        rect = Instantiate(rectPrefab, canvas.transform);
    }

    public void PlateSizeToTree()
    {
        if (tree != null)
        {
            float width = rect.GetComponent<RectTransform>().sizeDelta.x;
            float size = tree.GetComponent<RectTransform>().sizeDelta.x - width;
            rect.content.sizeDelta = new Vector2(-size, tree.GetComponent<RectTransform>().sizeDelta.y);

            tree.transform.SetParent(rect.GetComponent<ScrollRect>().content);
        }
    }
}
