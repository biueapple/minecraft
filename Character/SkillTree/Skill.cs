using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public string skillName; //이름
    public int id;      //
    public int level;       //스킬 레벨
    public int maxLevel;
    public Sprite sp = null;
    protected Character user;
    public float interval;
    protected float timer;
    
    public void LevelUp()
    {
        level++;
    }

    public void LevelDown()
    {
        level--;
    }

    public virtual bool Use(Character user, Unit target)
    {
        this.user = user;
        if (timer <= 0)
            return true;
        return false;
    }

    public virtual void Effect(Unit unit, Transform tf)
    {

    }

    public void Projective_Skill(Unit user,float range, Vector3 angle, Action<Unit, Transform> action, float speed, Projective projective)
    {
        projective.MoveToUnit(user, range, angle, action, speed);
    }

    public void Single_Effect(Unit target, float figure)
    {
        
    }

    protected IEnumerator CoolTimeLate()
    {
        while(timer > 0)
        {
            timer -= Time.deltaTime / interval;

            yield return null;
        }
    }

    public float GetTimer()
    {
        return timer;
    }
}
