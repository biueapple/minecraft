using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public BlockScriptble blockScriptble;       //���� ���
    public ItemScriptble itemScriptble;         //���� ����� �����۹���

    public ItemScriptble valueScriptble;        //����� �ı��ϸ� ������ ������

    public bool isVisible;

    public virtual void RightClick(Player player)
    {
        
    }

    public Vector3Int GetPosition() { return new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z); }

    public void ApplyTexture()
    {
        GetComponent<Renderer>().material.SetTexture("_Texture2D", blockScriptble.GetTexture());
    }


    /// <summary>
    /// �Ⱥ��̰�
    /// </summary>
    public void SetInvisible()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// ���̰�
    /// </summary>
    public void SetVisible()
    {
        if (isVisible)
        {
            gameObject.SetActive(true);
        }
    }

    public virtual void Init() { }
}
