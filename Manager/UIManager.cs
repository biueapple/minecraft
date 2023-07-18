using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //GraphicRaycaster
    GraphicRaycaster m_gr;
    PointerEventData m_ped;
    List<RaycastResult> results;
    private Canvas canvas;

    public List<GameObject> uiOpens = new List<GameObject>();

    void Start()
    {
        canvas = GetComponent<Canvas>();
        m_ped = new PointerEventData(null);
        m_gr = canvas.GetComponent<GraphicRaycaster>();
        results = new List<RaycastResult>();
    }

    void Update()
    {

    }

    public bool OpenAndClose(GameObject obj)
    {
        if(uiOpens.Contains(obj))
        {
            CloseUI(obj);
            return false;
        }
        else
        {
            OpenUI(obj);
            return true;
        }
            
    }

    public void TouchUI(GameObject obj)
    {
        int index = FindIndex(uiOpens, obj);
        if (index != -1 || index != 0)
            ListSwap(uiOpens, 0, index);
        obj.transform.SetAsLastSibling();
    }

    public void CloseUI()
    {
        if (uiOpens.Count > 0)
        {
            uiOpens[0].SetActive(false);
            uiOpens.RemoveAt(0);
        }
    }

    public void CloseUI(GameObject obj)
    {
        uiOpens.Remove(obj);
        obj.SetActive(false);
    }

    public void OpenUI(GameObject obj)
    {
        if (uiOpens.Contains(obj)) return;
        uiOpens.Add(obj);
        obj.SetActive(true);
        TouchUI(obj);
    }

    public int GetUICount()
    {
        return uiOpens.Count;
    }

    public void AllClose()
    {
        int count = uiOpens.Count;
        for (int i = 0; i < count; i++)
        {
            CloseUI();
        }
    }

    public bool OnlyOne(GameObject obj)
    {
        if(uiOpens.Count == 1 && uiOpens.Contains(obj)) return true;
        return false;
    }

    public bool Contain(GameObject obj)
    {
        if(uiOpens.Contains(obj))
            return true;
        return false;
    }

    public T GetGraphicRay<T>()
    {
        results.Clear();

        m_ped.position = Input.mousePosition;

        m_gr.Raycast(m_ped, results);

        if (results.Count > 0)
        {
            return results[0].gameObject.GetComponent<T>();
        }
        return default(T);
    }

    public void ListSwap<T>(List<T> list, int index1, int index2)
    {
        if (index2 == -1)
            return;

        if (index1 < 0 || index2 >= list.Count)
        {
            return;
        }

        T item = list[index1];
        list[index1] = list[index2];
        list[index2] = item;

    }
    public int FindIndex<T>(List<T> list, T obj)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Equals(obj))
            {
                return i;
            }
        }
        return -1;
    }
}
