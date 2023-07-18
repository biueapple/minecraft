using UnityEngine;

public class EquipItem : Item
{
    //공격이 마법인지 물리인지 정하고
    //
    protected Stat stat;
    public int level;
    public float damage;
    public ITEM_RATING rating;
    public DAMAGE_TYPE d_Type;
    public float range;

    public Collider[] Damage_Coll(Vector3 posi)
    {
        return Physics.OverlapBox(posi + new Vector3(0, 0, range * 0.5f), new Vector3(1, 1, range));
    }

    public override Item Init()
    {
        stat = GetComponent<Stat>();
        stat.Init();
        return base.Init();
    }

    public Stat GetStat() { return  stat; }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + new Vector3(0, 0, 1 * 0.5f), new Vector3(0.5f, 0.5f, 0.5f));
    }
}

