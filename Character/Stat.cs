using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Stat : MonoBehaviour
{
    //체력
    public float hp;
    //체력 자연 회복량
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
    /// 맞을때 발동하는 unit = 누가 때렸는지 float = 대미지
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
    //  독 : 방어력이나 저항력 판정을 받지 않음 hitDelegate판정을 받음
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
    //화상 : 방어력이나 저항력 판정을 받지 않음
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
    //감전 : ap딜 저항력 판정을 받음
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
    //출혈 : ad딜 방어력 판정을 받음 hitDelegate판정을 받음
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
}
