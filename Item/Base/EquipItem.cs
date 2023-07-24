using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class EquipItem : Item
{
    //공격이 마법인지 물리인지 정하고
    //
    protected Stat stat;
    public float damage;
    public float range;
    public Transform user;

    public Collider[] Damage_Coll(Transform use)
    {
        user = use;
        return Physics.OverlapBox((use.position + use.forward)/* * (range * 0.5f)*/, new Vector3(1, 1, range));
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
        if(user != null)
        {
            Gizmos.DrawWireCube((user.position + user.forward)/* * (0.5f * range)*/, new Vector3(0.5f, 0.5f, range * 0.5f));
        }
    }
}

