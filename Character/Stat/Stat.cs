using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    private Unit user;
    //체력
    public float hp;
    private float maxHp;
    //체력 자연 회복량
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
    /// 맞을때 발동하는 unit = 누가 때렸는지 float = 대미지
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
        Debug.Log($"대미지 {figure} 받음");
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
    /// ad 대미지를 방어력에 맞게 조정해줌
    /// </summary>
    /// <param name="figure">대미지</param>
    /// <param name="penetration">방어구 관통력 (고정) </param>
    /// <param name="per">방어구 관통력 (퍼센트)</param>
    /// <returns>방어력과 관통력을 거쳐간 대미지</returns>
    public float Halved_AD(float figure, float penetration, float per)
    {
        float defence = this.defence - penetration;
        defence -= defence * per * 0.01f;
        return (100 / (100 + defence) * figure);
    }
    /// <summary>
    /// ap 대미지를 방어력에 맞게 조정해줌
    /// </summary>
    /// <param name="figure">대미지</param>
    /// <param name="penetration">저항력 관통력 (고정) </param>
    /// <param name="per">저항력 관통력 (퍼센트)</param>
    /// <returns>저항력과 관통력을 거쳐간 대미지</returns>
    public float Halved_AP(float figure, float penetration, float per)
    {
        float resistance = this.resistance - penetration;
        resistance -= resistance * per * 0.01f;
        return (100 / (100 + resistance) * figure);
    }

    /// <summary>
    /// 누군가에게 공격 받았을 때
    /// </summary>
    /// <param name="figure">대매지</param>
    /// <param name="penetration">관통력</param>
    /// <param name="per">관통력 (퍼센트)</param>
    /// <param name="perpetrator">누가 공격한건지</param>
    public void Be_Attacked_AD(float figure, float penetration, float per, Unit perpetrator)
    {
        float damage = Halved_AD(figure, penetration, per); 
        MinusHp(damage);
        HitInvocation(perpetrator, damage);
    }
    /// <summary>
    /// 누군가에게 공격 받았을 때
    /// </summary>
    /// <param name="figure">대매지</param>
    /// <param name="penetration">관통력</param>
    /// <param name="per">관통력 (퍼센트)</param>
    /// <param name="perpetrator">누가 공격한건지</param>
    public void Be_Attacked_AP(float figure, float penetration, float per, Unit perpetrator)
    {
        float damage = Halved_AP(figure, penetration, per);
        MinusHp(damage);
        HitInvocation(perpetrator, damage);
    }
    /// <summary>
    /// 누군가에게 공격 받았을 때
    /// </summary>
    /// <param name="figure">대매지</param>
    /// <param name="perpetrator">누가 공격한건지</param>
    public void Be_Attacked_TRUE(float figure, Unit perpetrator)
    {
        MinusHp(figure);
        HitInvocation(perpetrator, figure);
    }

    //자연 회복, max보다 많아지면 끝남
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

    //도트대미지는 받을때 이미 발동중인 도트대미지를 없앤 후 발동함
    //  독 : 방어력이나 저항력 판정을 받지 않음 attackDelegate 판정을 받음
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
    //화상 : 방어력이나 저항력 판정을 받지 않음 hitDelegate판정을 받음
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
    //감전 : ap딜 저항력 판정을 받음 hitDelegate판정을 받음
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
    //출혈 : ad딜 방어력 판정을 받음 attackDelegate 판정을 받음
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
    /// hitDelegate 전부 발동
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
