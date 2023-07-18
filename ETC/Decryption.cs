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
    public InstallItem installItem;
    public Block block;
    public SolidBlock solidBlock;
    public FluidBlock fluidBlock;
    public InterBlock interBlock;
    public Vector3 nomal;
    //public float Figure;             //수치(대미지나 다른)

    public Vector3 HitVec;

    private bool same;

    public bool DecryptionSetting(RaycastHit hit, Transform tf = null)
    {
        if (Obj == hit.transform.gameObject)
        {
            same = true;
        }
        else
        {
            same = false;
        }

        Init();

        Obj = hit.transform.gameObject;

        if (tf != null)
        {
            Distance = Vector3.Distance(tf.position, hit.transform.position);
            HitDistance = Vector3.Distance(tf.position, hit.point);
        }
        else
        {
            Distance = Vector3.Distance(transform.position, hit.transform.position);
            HitDistance = Vector3.Distance(transform.position, hit.point);
        }
        
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
            if(item.GetComponent<InstallItem>() != null)
            {
                installItem = item.GetComponent<InstallItem>();
            }
        }
        if(hit.transform.GetComponent<Block>() != null)
        {
            block = hit.transform.GetComponent<Block>();
            large_Frame = _LARGE_FRAME.BLOCK;
            if(block.GetComponent<SolidBlock>() != null)
            {
                solidBlock = block.GetComponent<SolidBlock>();
            }
            if(block.GetComponent<FluidBlock>() != null)
            {
                fluidBlock = block.GetComponent<FluidBlock>();
            }
            if (block.GetComponent<InterBlock>() != null)
            {
                interBlock = block.GetComponent<InterBlock>();
            }
        }

        return same;
    }

    public void Init()
    {
        if(block != null && !same)
        {
            block.Init();
        }

        same = false;
        Obj = null;
        large_Frame = _LARGE_FRAME.NONE;
        small_Frame = _SMALL_FRAME.NONE;
        striking_Area = _STRIKING_AREA.NONE;
        Distance = 0;
        HitDistance = 0;
        nomal = Vector3.zero;
        HitVec = Vector3.zero;
        unit = null;
        item = null;
        installItem = null;
        block = null;
        solidBlock = null;
        fluidBlock = null;
        interBlock = null;
    }
}
