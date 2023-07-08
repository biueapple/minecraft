using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Stat : MonoBehaviour
{
    //ü��
    public float hp;
    //ü�� �ڿ� ȸ����
    public float naturalHP;
    public float mp;
    public float naturalMP;
    public float defence;
    public float resistance;
    public float ad;
    public float ap;
    //
    public float maxHp;
    public float originalNaturalHP;
    public float maxMp;
    public float originalNaturalMP;
    public float originalDefence;
    public float originalResistance;
    public float originalAd;
    public float originalAp;

    //Natural_Recovery_HP
    private Coroutine nrhp = null;
    //Natural_Recovery_MP
    private Coroutine nrmp = null;
    //Dot_Poison
    private Coroutine dot_poison = null;
    private Coroutine dot_burn = null;
    private Coroutine dot_shock = null;
    private Coroutine dot_bleeding = null;

    /// <summary>
    /// ������ �ߵ��ϴ� unit = ���� ���ȴ��� float = �����
    /// </summary>
    protected List<Action<Unit, float>> hitDelegate = new List<Action<Unit, float>>();


    public void MinusHp(float figure)
    { 
        hp -= figure;
        if(nrhp == null )
        {
            nrhp = StartCoroutine(Natural_Recovery_HP());
        }
    }

    public void MinusMp(float figure) 
    { 
        mp -= figure;
        if (mp < 0)
            mp = 0;
        if (nrmp == null)
        {
            nrmp = StartCoroutine(Natural_Recovery_MP());
        }
    }

    public void RecoveryHP(float figure) { hp += figure; if (hp > maxHp) hp = maxHp; }

    public void RecoveryMP(float figure) { mp += figure; if (mp > maxMp) mp = maxMp; }

    /// <summary>
    /// ad ������� ���¿� �°� ��������
    /// </summary>
    /// <param name="figure">�����</param>
    /// <param name="penetration">�� ����� (����) </param>
    /// <param name="per">�� ����� (�ۼ�Ʈ)</param>
    /// <returns>���°� ������� ���İ� �����</returns>
    public float Halved_AD(float figure, float penetration, float per)
    {
        float defence = this.defence - penetration;
        defence -= defence * per * 0.01f;
        return (100 / (100 + defence) * figure);
    }
    /// <summary>
    /// ap ������� ���¿� �°� ��������
    /// </summary>
    /// <param name="figure">�����</param>
    /// <param name="penetration">���׷� ����� (����) </param>
    /// <param name="per">���׷� ����� (�ۼ�Ʈ)</param>
    /// <returns>���׷°� ������� ���İ� �����</returns>
    public float Halved_AP(float figure, float penetration, float per)
    {
        float resistance = this.resistance - penetration;
        resistance -= resistance * per * 0.01f;
        return (100 / (100 + resistance) * figure);
    }

    /// <summary>
    /// ���������� ���� �޾��� ��
    /// </summary>
    /// <param name="figure">�����</param>
    /// <param name="penetration">�����</param>
    /// <param name="per">����� (�ۼ�Ʈ)</param>
    /// <param name="perpetrator">���� �����Ѱ���</param>
    public void Be_Attacked_AD(float figure, float penetration, float per, Unit perpetrator)
    {
        float damage = Halved_AD(figure, penetration, per); 
        MinusHp(damage);
        HitInvocation(perpetrator, damage);
    }
    /// <summary>
    /// ���������� ���� �޾��� ��
    /// </summary>
    /// <param name="figure">�����</param>
    /// <param name="penetration">�����</param>
    /// <param name="per">����� (�ۼ�Ʈ)</param>
    /// <param name="perpetrator">���� �����Ѱ���</param>
    public void Be_Attacked_AP(float figure, float penetration, float per, Unit perpetrator)
    {
        float damage = Halved_AP(figure, penetration, per);
        MinusHp(damage);
        HitInvocation(perpetrator, damage);
    }
    /// <summary>
    /// ���������� ���� �޾��� ��
    /// </summary>
    /// <param name="figure">�����</param>
    /// <param name="perpetrator">���� �����Ѱ���</param>
    public void Be_Attacked_TRUE(float figure, Unit perpetrator)
    {
        MinusHp(figure);
        HitInvocation(perpetrator, figure);
    }

    //�ڿ� ȸ��, max���� �������� ����
    private IEnumerator Natural_Recovery_HP()
    {
        while(hp < maxHp)
        {
            hp += naturalHP;

            yield return new WaitForSeconds(1);
        }
    }
    private IEnumerator Natural_Recovery_MP()
    {
        while (mp < maxMp)
        {
            mp += naturalMP;

            yield return new WaitForSeconds(1);
        }
    }

    //��Ʈ������� ������ �̹� �ߵ����� ��Ʈ������� ���� �� �ߵ���
    //  �� : �����̳� ���׷� ������ ���� ���� hitDelegate������ ����
    public void Be_Attacked_Poison(int duration, float figure, Unit perpetrator)
    {
        if(dot_poison != null)
        {
            StopCoroutine(dot_poison);
        }
        dot_poison = StartCoroutine(Dot_Poison(duration, figure, perpetrator));
    }
    private IEnumerator Dot_Poison(int duration, float figure, Unit perpetrator)
    {
        int t = 0;
        while (t <= duration)
        {
            Be_Attacked_TRUE(figure, perpetrator);
            yield return new WaitForSeconds(1);
            t++;
        }
        dot_poison = null;
    }
    //ȭ�� : �����̳� ���׷� ������ ���� ����
    public void Be_Attacked_Burn(int duration, float figure)
    {
        if (dot_burn != null)
        {
            StopCoroutine(dot_burn);
        }
        dot_burn = StartCoroutine(Dot_Burn(duration, figure));
    }
    private IEnumerator Dot_Burn(int duration, float figure)
    {
        int t = 0;
        while (t <= duration)
        {
            MinusHp(figure);
            yield return new WaitForSeconds(1);
            t++;
        }
        dot_burn = null;
    }
    //���� : ap�� ���׷� ������ ����
    public void Be_Attacked_Shock(int duration, float figure, float penetration, float per)
    {
        if(dot_shock != null)
        {
            StopCoroutine (dot_shock);
        }
        dot_shock = StartCoroutine(Dot_Shock(duration, figure, penetration, per));
    }
    private IEnumerator Dot_Shock(int duration, float figure, float penetration, float per)
    {
        int t = 0;
        while (t <= duration)
        {
            MinusHp(Halved_AP(figure, penetration, per));
            yield return new WaitForSeconds(1);
            t++;
        }
        dot_shock = null;
    }
    //���� : ad�� ���� ������ ���� hitDelegate������ ����
    public void Be_Attacked_Bleeding(int duration, float figure, float penetration, float per, Unit perpetrator)
    {
        if( dot_bleeding != null)
        {
            StopCoroutine(dot_bleeding);
        }
        dot_bleeding = StartCoroutine(Dot_Bleeding(duration, figure, penetration, per, perpetrator));
    }
    private IEnumerator Dot_Bleeding(int duration, float figure, float penetration, float per, Unit perpetrator)
    {
        int t = 0;
        while (t <= duration)
        {
            Be_Attacked_AD(figure, penetration, per, perpetrator);
            yield return new WaitForSeconds(1);
            t++;
        }
        dot_bleeding = null;
    }

    //
    //
    //

    public void AddHit(Action<Unit, float> action)
    {
        if (hitDelegate.Contains(action)) return;
        hitDelegate.Add(action);
    }
    public void RemoveHit(Action<Unit, float> action)
    {
        hitDelegate.Remove(action);
    }
    /// <summary>
    /// hitDelegate ���� �ߵ�
    /// </summary>
    /// <param name="perpetrator"></param>
    /// <param name="figure"></param>
    public void HitInvocation(Unit perpetrator, float figure)
    {
        for (int i = 0; i < hitDelegate.Count; i++)
        {
            hitDelegate[i](perpetrator, figure);
        }
    }
}
