using System;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    protected Stat stat;

    //
    /// <summary>
    /// ������ �ߵ��ϴ� unit = ���� ���ȴ��� float = �����
    /// </summary>
    protected List<Action<Unit, float>> attackDelegate = new List<Action<Unit, float>>();

    public void OriginalAttack(Unit unit)
    {
        
    }


    public void GetDamage(Unit op, float f)
    {
        
    }

    public void GiveDamage(Unit unit, float damage)
    {

    }

    protected virtual void Hit()
    {

    }
}
