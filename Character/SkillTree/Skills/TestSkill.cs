using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkill : Skill
{
    public Projective proPre;
    public float range;
    public float damage;
    protected List<Projective> projectives = new List<Projective>();

    public override bool Use(Character user, Unit target)
    {
        if(base.Use(user, target))
        {
            projectives.Add(Instantiate(proPre));
            Projective_Skill(user, range, new Vector3(user.GetPlayer().GetComponent<PlayerMouse>().GetVertical(), user.GetPlayer().GetComponent<PlayerMouse>().GetHorizontal(), 0)
                , Effect, 5, projectives[projectives.Count - 1]);
            timer = 1;
            StartCoroutine(CoolTimeLate());
            return true;
        }
        return false;
    }

    public override void Effect(Unit unit, Transform tf)
    {
        unit.Hit(tf);
        unit.GetStat().Be_Attacked_AP(damage + (user.GetStat().GetAp() * 0.6f) + level * 10, 0, 0, user);
    }
}
