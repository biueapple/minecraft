using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum _LARGE_FRAME
{
    NONE = 0,       //그냥 오브젝트 아무것도아닌 벽같은거
    UNIT,           //유닛(플레이어나 적)
    ITEM,           //아이템
    BLOCK
}
public enum _SMALL_FRAME
{
    NONE = 0,
    PLAYER,         //플레이어UNIT
    FRIENDAI,       //아군UNIT
    ENEMYAI,        //적UNIT
    NONEINTERACTION,    //상호작용 불가능한 오브젝트나 아이템
    INTERACTION,        //상호작용 가능한 오브젝트나 아이템
    ETC,            //

}
public enum _STRIKING_AREA  //UNIT인 경우에만
{ 
    NONE = 0,       //부위가 없음
    HEAD,
    BODY,
    ARM,
    LEG,

}

public class Decryption : MonoBehaviour
{
    public GameObject Obj;
    public _LARGE_FRAME large_Frame;        
    public _SMALL_FRAME small_Frame;
    public _STRIKING_AREA striking_Area;
    public float Distance;            //거리
    public float HitDistance;
    public Unit unit;
    public Item item;
    public Block block;
    public Vector3 nomal;
    //public float Figure;             //수치(대미지나 다른)

    public Vector3 HitVec;

    //private bool same;

    public void DecryptionSetting(RaycastHit hit)
    {
        //if(Obj == hit.transform.gameObject)
        //{
        //    same = true;
        //}
        //else
        //{
        //    same = false;
        //}

        Init();

        Obj = hit.transform.gameObject;

        Distance = Vector3.Distance(transform.position, hit.transform.position);
        HitDistance = Vector3.Distance(transform.position, hit.point);
        nomal = hit.normal;
        HitVec = hit.point;

        if (hit.transform.GetComponent<Unit>() != null)
        {
            unit = hit.transform.GetComponent<Unit>();
            large_Frame = _LARGE_FRAME.UNIT;
        }
        if (hit.transform.GetComponent<Item>() != null)
        {
            item = hit.transform.GetComponent<Item>();
            large_Frame = _LARGE_FRAME.ITEM;
        }
        if(hit.transform.GetComponent<Block>() != null)
        {
            block = hit.transform.GetComponent<Block>();
            large_Frame = _LARGE_FRAME.BLOCK;
        }

        //else if (hit.transform.parent != null)
        //{
        //    Debug.Log("2");
        //    Distance = Vector3.Distance(transform.position, hit.transform.position);
        //    HitDistance = Vector3.Distance(transform.position, hit.point);
        //    HitVec = hit.point;

        //    if (hit.transform.parent.GetComponent<Unit>() != null)
        //    {
        //        unit = hit.transform.parent.GetComponent<Unit>();
        //        large_Frame = _LARGE_FRAME.UNIT;
        //    }
        //    if (hit.transform.parent.GetComponent<BaseItem>() != null)
        //    {
        //        item = hit.transform.parent.GetComponent<BaseItem>();
        //        if (hit.transform.parent.GetComponent<InteractionItem>() != null)
        //        {
        //            small_Frame = _SMALL_FRAME.INTERACTION;
        //        }
        //        else
        //        {
        //            small_Frame = _SMALL_FRAME.NONEINTERACTION;
        //        }
        //        large_Frame = _LARGE_FRAME.ITEM;
        //    }
        //}

        //return same;
    }

    public void Init()
    {

        Obj = null;
        large_Frame = _LARGE_FRAME.NONE;
        small_Frame = _SMALL_FRAME.NONE;
        striking_Area = _STRIKING_AREA.NONE;
        Distance = 0;
        HitDistance = 0;
        //Figure = 0;
        nomal = Vector3.zero;
        HitVec = Vector3.zero;
        unit = null;
        item = null;
        block = null;
    }
}
