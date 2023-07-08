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
    private int strength;       //°­µµ
    public int GetStrength()
    {
        return strength;
    }
}
