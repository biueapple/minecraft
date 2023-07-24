using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    private Unit user;
    //ü��
    public float hp;
    private float maxHp;
    //ü�� �ڿ� ȸ����
    private float naturalHP;
    private float mp;
    private float maxMp;
    private float naturalMP;
    private float defence;
    private float resistance;
    private float ad;
    public float GetAd() { return ad; }
    private float ap;
    public float GetAp() { return ap; }
    private float speed;
    //
    public float originalHP;
    public float originalNaturalHP;
    public float originalMP;
    public float originalNaturalMP;
    public float originalDefence;
    public float originalResistance;
    public float originalAd;
    public float originalAp;
    public float originalSpeed;

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

    public void SetText(Text text)
    {
        text.text = "hp = " + hp + " / " + maxHp + "\n" +
                    "mp = " + mp + " / " + maxMp + "\n" +
                    "naturalHP = " + naturalHP + "\n" +
                    "naturalMP = " + naturalMP + "\n" +
                    "defence = " + defence + "\n" +
                    "resistance = " + resistance + "\n" +
                    "ad = " + ad + "\n" +
                    "ap = " + ap + "\n" +
                    "speed = " + speed;
    }

    public void ItemEquip(EquipItem item)
    {
        maxHp += item.GetStat().hp;
        hp += item.GetStat().hp;
        maxMp += item.GetStat().mp;
        mp += item.GetStat().mp;
        naturalHP += item.GetStat().naturalHP;
        naturalMP += item.GetStat().naturalMP;
        defence += item.GetStat().defence;
        resistance += item.GetStat().resistance;
        ad += item.GetStat().ad;
        ap += item.GetStat().ap;
        speed += item.GetStat().speed;
        SetSpeed();
    }

    public void ItemLift(EquipItem item)
    {
        maxHp -= item.GetStat().hp;
        hp -= item.GetStat().hp;
        maxMp -= item.GetStat().mp;
        mp -= item.GetStat().mp;
        naturalHP -= item.GetStat().naturalHP;
        naturalMP -= item.GetStat().naturalMP;
        defence -= item.GetStat().defence;
        resistance -= item.GetStat().resistance;
        ad -= item.GetStat().ad;
        ap -= item.GetStat().ap;
        speed -= item.GetStat().speed;
        SetSpeed();
    }

    public void Init()
    {
        user = GetComponent<Unit>();
        hp = maxHp = originalHP;
        naturalHP = originalNaturalHP;
        mp = maxMp = originalMP;
        naturalMP = originalNaturalMP;
        defence = originalDefence;
        resistance = originalResistance;
        ad = originalAd;
        ap = originalAp;
        speed = originalSpeed;
        SetSpeed();
    }

    public void MinusHp(float figure)
    {
        Debug.Log($"����� {figure} ����");
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
    //  �� : �����̳� ���׷� ������ ���� ���� attackDelegate ������ ����
    public void Be_Attacked_Poison(int duration, float figure, Action<Unit, float> attack)
    {
        if(dot_poison != null)
        {
            StopCoroutine(dot_poison);
        }
        dot_poison = StartCoroutine(Dot_Poison(duration, figure, attack));
    }
    private IEnumerator Dot_Poison(int duration, float figure, Action<Unit, float> attack)
    {
        int t = 0;
        while (t <= duration)
        {
            yield return new WaitForSeconds(1);
            MinusHp(figure);
            attack(user, figure);
            t++;
        }
        dot_poison = null;
    }
    //ȭ�� : �����̳� ���׷� ������ ���� ���� hitDelegate������ ����
    public void Be_Attacked_Burn(int duration, float figure, Unit perpetrator)
    {
        if (dot_burn != null)
        {
            StopCoroutine(dot_burn);
        }
        dot_burn = StartCoroutine(Dot_Burn(duration, figure, perpetrator));
    }
    private IEnumerator Dot_Burn(int duration, float figure, Unit perpetrator)
    {
        int t = 0;
        while (t <= duration)
        {
            yield return new WaitForSeconds(1);
            Be_Attacked_TRUE(figure, perpetrator);
            t++;
        }
        dot_burn = null;
    }
    //���� : ap�� ���׷� ������ ���� hitDelegate������ ����
    public void Be_Attacked_Shock(int duration, float figure, float penetration, float per, Unit perpetrator)
    {
        if(dot_shock != null)
        {
            StopCoroutine (dot_shock);
        }
        dot_shock = StartCoroutine(Dot_Shock(duration, figure, penetration, per, perpetrator));
    }
    private IEnumerator Dot_Shock(int duration, float figure, float penetration, float per, Unit perpetrator)
    {
        int t = 0;
        while (t <= duration)
        {
            yield return new WaitForSeconds(1);
            Be_Attacked_AP(figure, penetration, per, perpetrator);
            t++;
        }
        dot_shock = null;
    }
    //���� : ad�� ���� ������ ���� attackDelegate ������ ����
    public void Be_Attacked_Bleeding(int duration, float figure, float penetration, float per, Action<Unit, float> attack)
    {
        if( dot_bleeding != null)
        {
            StopCoroutine(dot_bleeding);
        }
        dot_bleeding = StartCoroutine(Dot_Bleeding(duration, figure, penetration, per, attack));
    }
    private IEnumerator Dot_Bleeding(int duration, float figure, float penetration, float per, Action<Unit, float> attack)
    {
        int t = 0;
        float damage = 0;
        while (t <= duration)
        {
            yield return new WaitForSeconds(1);

            damage = Halved_AD(figure, penetration, per);
            MinusHp(damage);
            attack(user, damage);
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
    public void SetSpeed()
    {
        if(GetComponent<CharacterMove>() != null)
        {
            GetComponent<CharacterMove>().m_Speed = speed;
        }
    }
}
