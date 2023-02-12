using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockData", menuName = "ScriptableObj/CreateBlock", order = int.MaxValue)]
public class BlockScriptble : ScriptableObject
{
    [SerializeField]
    private Texture texture;
    public Texture GetTexture()
    {
        return texture; 
    }

    //
    [SerializeField]
    private int strength;       //강도
    public int GetStrength()
    {
        return strength;
    }

    private float durability;               //강도보다 낮으면 상승하지 않고 강도보다 높아지면 파괴
    public float GetDurability()
    {
        return durability;
    }
    public void SetDurabillity(float f)
    {
        durability = f;
    }
}
