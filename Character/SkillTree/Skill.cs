using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public string skillName; //�̸�
    public int id;      //
    public int level;       //��ų ����
    public int maxLevel;
    public Sprite sp = null;

    
    
    public void LevelUp()
    {
        level++;
    }

    public void LevelDown()
    {
        level--;
    }

    public void Projective_Skill(Unit target, Action<Unit> action, float speed, Projective projective)
    {
        projective.MoveToUnit(target, action, speed);
    }

    public void Single_Effect(Unit target, float figure)
    {
        
    }
}
