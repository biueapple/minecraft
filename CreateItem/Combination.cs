using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public enum _ITEMCODE
{
    NONE = 0,
    SOIL,
    STONE,
    WOOD,
    PLANK,

}

public static class Combination
{
    public static List<Recipe> allRecipes = new List<Recipe>();
    
    public static void init()
    {
        allRecipes.Add(new Soil_Recipe());
        allRecipes.Add(new Wood_Recipe());
    }


    public static uint Cutting(int[] ints)
    {
        int i1;
        int i2;
        string s = "";

        for(int i = 0; i < ints.Length; i++)
        {
            //Debug.Log(ints[i]);
        }

        for (i1 = 0; i1 < ints.Length; i1++)
        {
            if (ints[i1] != 0)
            {
                break;
            }
        }

        for (i2 = ints.Length - 1; i2 >= 0; i2--)
        {
            if (ints[i2] != 0)
            {
                break;
            }
        }

        for (int i = i1; i <= i2; i++)
        {
            s += ints[i].ToString();
        }
        
        if(s.Equals(""))
        {
            return 0;
        }
        return uint.Parse(s);
    }
}

public class Recipe
{
    public uint hash;
    public _ITEMCODE result;
    public int resultCount;

    public List<_ITEMCODE> codes = new List<_ITEMCODE>();

    public int[] recipe;

    public bool Including(_ITEMCODE code)     //포함하는지
    {
        for(int i = 0; i < codes.Count; i++)
        {
            if(codes[i] == code)
            {
                return true;
            }
        }
        return false;
    }

    public bool Comparison(uint h, List<_ITEMCODE> list)
    {
        if(codes.Count != list.Count)
        {
            return false;
        }

        for(int i = 0; i < codes.Count; i++)
        {
            if (codes[i] != list[i])
            {
                return false;
            }
        }

        if(hash == h)
        {
            return true;
        }

        return false;
    }
}

public class Soil_Recipe : Recipe
{
    public Soil_Recipe()
    {
        result = _ITEMCODE.SOIL;
        resultCount = 4;

        int[] ints =
        {
            0,0,0,
            1,1,0,
            1,1,0
        };

        recipe = ints;

        hash = Combination.Cutting(recipe);
        codes.Add(_ITEMCODE.SOIL);

        codes.Sort();
    }
}

public class Wood_Recipe : Recipe
{
    public Wood_Recipe()
    {
        result = _ITEMCODE.PLANK;
        resultCount = 4;

        int[] ints =
        {
            1,0,0,
            0,0,0,
            0,0,0
        };

        recipe = ints;

        hash = Combination.Cutting(recipe);

        codes.Add(_ITEMCODE.WOOD);

        codes.Sort();
    }
}