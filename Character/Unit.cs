using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{
    protected Stat stat;
    protected Rigidbody rigid; // Rigidbody ������Ʈ�� ������ ����

    public void Hit(Transform target)
    {
        rigid.AddForce((target.forward + Vector3.up) * 4, ForceMode.Impulse);
    }
    public Stat GetStat() { return stat; }
}
