using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FindingManger : MonoBehaviour
{
    public float minimumDistance;
    public static List<Monster> list = new List<Monster>();
    private static Coroutine coroutine = null;
    public List<_NODE> OpenList = new List<_NODE>();
    public List<_NODE> CloseList = new List<_NODE>();
    public List<_NODE> FinalList = new List<_NODE>();
    private Vector3Int[] vec = new Vector3Int[26];
    private TopographyParent topographyParent;
    public int test;

    void Start()
    {
        vec[0] = new Vector3Int(1, 1, 1);
        vec[1] = new Vector3Int(1, 0, 1);
        vec[2] = new Vector3Int(1, -1, 1);

        vec[3] = new Vector3Int(0, 1, 1);
        vec[4] = new Vector3Int(0, 0, 1);
        vec[5] = new Vector3Int(0, -1, 1);

        vec[6] = new Vector3Int(-1, 1, 1);
        vec[7] = new Vector3Int(-1, 0, 1);
        vec[8] = new Vector3Int(-1, -1, 1);

        vec[9] = new Vector3Int(1, 1, 0);
        vec[10] = new Vector3Int(0, 1, 0);
        vec[11] = new Vector3Int(-1, 1, 0);

        vec[12] = new Vector3Int(1, 1, -1);
        vec[13] = new Vector3Int(0, 1, -1);
        vec[14] = new Vector3Int(-1, 1, -1);

        vec[15] = new Vector3Int(1, 0, 0);
        vec[16] = new Vector3Int(1, 0, -1);

        vec[17] = new Vector3Int(1, -1, 0);
        vec[18] = new Vector3Int(1, -1, -1);

        vec[19] = new Vector3Int(-1, 0, 0);
        vec[20] = new Vector3Int(-1, 0, -1);

        vec[21] = new Vector3Int(-1, -1, 0);
        vec[22] = new Vector3Int(-1, -1, -1);

        vec[23] = new Vector3Int(0, -1, 0);

        vec[24] = new Vector3Int(0, -1, -1);

        vec[25] = new Vector3Int(0, 0, -1);

        topographyParent = FindObjectOfType<TopographyParent>();    
    }

    public void AddMonster(Monster monster)
    {
        if (list.Contains(monster))
            return;

        list.Add(monster);
        if(coroutine == null)
        {
            coroutine = StartCoroutine(Finding());
        }
    }

    public void RemoveMonster(Monster monster)
    {
        if(coroutine != null)
            StopCoroutine(coroutine);
        list.Remove(monster);
        coroutine = StartCoroutine(Finding());
    }

    private  IEnumerator Finding()
    {
        while (list.Count > 0)
        {

            OpenList.Add(new _NODE(ToVector3(list[0].transform.position), Vector3.Distance(list[0].transform.position, list[0].target.transform.position)));
            OpenList[OpenList.Count - 1].previous = -1;

            while (true)
            {

                if (OpenList.Count <= 0)
                {
                    Debug.Log("길이 없음");
                    yield break;
                }
                int index = FindMinDis(OpenList);

                CloseList.Add(OpenList[index]);
                OpenList.RemoveAt(index);

                if (Vector3.Distance(CloseList[CloseList.Count - 1].position, list[0].target.transform.position) < minimumDistance)
                {
                    break;
                }

                for (int i = 0; i < vec.Length; i++)
                {
                    if (RayShot(CloseList[CloseList.Count - 1].position, vec[i]))
                    {
                        if (PosiSame(CloseList[CloseList.Count - 1].position + vec[i]))
                        {
                            OpenList.Add(new _NODE(CloseList[CloseList.Count - 1].position + vec[i]));
                            OpenList[OpenList.Count - 1].SetDistance(ToVector3(list[0].target.transform.position));
                            OpenList[OpenList.Count - 1].previous = CloseList.Count - 1;

                        }
                    }
                }

                test++;
                if (test > 100)
                {
                    break;
                }

                yield return null;
            }

            int temp = CloseList.Count - 1;

            while (true)
            {

                FinalList.Add(CloseList[temp]);
                temp = CloseList[temp].previous;

                if (FinalList[FinalList.Count - 1].previous == -1)
                {
                    break;
                }
                yield return null;
            }

            FinalList.Reverse();

            list[0].SetWay(FinalList);
            list.RemoveAt(0);

            test = 0;
            OpenList.Clear();
            CloseList.Clear();
            FinalList.Clear();

            yield return null;
        }
        coroutine = null;
    }



    public int FindMinDis(List<_NODE> list)            //가장 작은 값 찾기
    {
        float min = list[0].distance;
        int index = 0;

        for (int i = 1; i < list.Count; i++)
        {
            if (min > list[i].distance)
            {
                min = list[i].distance;
                index = i;

            }
        }

        return index;
    }
    public bool RayShot(Vector3Int posi, Vector3Int direction)    //ray쏴 충돌 찾기 Physics로 교체
    {
        if (topographyParent.GetBlock(posi + direction) == null)
        {
            return true;
        }
        else
            return false;
    }

    public bool PosiSame(Vector3 posi)
    {
        for (int i = 0; i < OpenList.Count; i++)
        {
            if (OpenList[i].position == posi)
            {
                return false;
            }
        }

        for (int i = 0; i < CloseList.Count; i++)
        {
            if (CloseList[i].position == posi)
            {
                return false;
            }
        }

        return true;
    }

    public Vector3Int ToVector3(Vector3 vector)
    {
        return new Vector3Int((int)vector.x, (int)vector.y, (int)vector.z);
    }
}

[System.Serializable]
public class _NODE     //부모노드 gfh
{
    public Vector3Int position;    //위치
    public float distance;      //거리
    public int previous;
    public _NODE(Vector3Int vec, float d)
    {
        position = vec;
        distance = d;
    }
    public _NODE(Vector3Int vec)
    {
        position = vec;
    }
    public void SetDistance(Vector3Int target)
    {
        distance = Vector3.Distance(position, target);
    }
}