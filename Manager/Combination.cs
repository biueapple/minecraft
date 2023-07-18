using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum _ITEMCODE
{
    NONE = 0,
    SOIL,
    STONE,
    WOOD,
    PLANK,
    GRASS,
    CRAFTINGBOX,
    LEAF,
    WATER,
    WOODEN_STICK,
    WOODEN_SWORD,

}

//지금 방식이 Combination에서 cutting하고 allRecipes 하나하나 비교하는 방식
//레시피를 넣으면 cutting한거로만 찾으면 1 1 0    11 0 11 차이를 모르기때문에 하나하나 배열을 비교하는데
//                                     1 1 0    0  0  0
//                                     0 0 0    0  0  0
//cutting한거랑 같은걸 찾고 배열로 리턴하고 다시 배열을 하나하나 비교하는걸로 바꾸는게 좋을듯
//cutting한게 0 0 1   0 1 1 의 차이를 모르기에 방법을 다시 생각해야함
//            1 0 1   0 1 1
//            1 0 0   0 0 0

public static class Combination
{
    public static List<Recipe> allRecipes = new List<Recipe>();
    
    public static void init()
    {
        allRecipes.Add(new Soil_Recipe());
        allRecipes.Add(new Plank_Recipe());
        allRecipes.Add(new CraftingBox_Recipe());
        allRecipes.Add(new Wooden_Stick_Recipe());
        allRecipes.Add(new Wooden_Sword_Recipe());
    }


    public static uint Cutting(int[] ints)      //gethashcode로 문자열로 내보내야함 지금 uint로 가능한 이유는 아이템코드를 다시 한자릿수 int로 변환후 저장해서 최대 자릿수가 9자리 unit는 4,294,967,295까지
    {                                           //                                                                                  변환하는 과정은 craftingbox에 Comparison에 있음
        int i1;
        int i2;
        string s = "";

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

        if (s.Equals(""))
        {
            return 0;
        }
        return uint.Parse(s);       //지금은 문자열을 다시 int로 parse하지만 나중에 gethashcode로 바꿔서 하는게 좋을듯
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

    public bool Comparison(uint h, List<_ITEMCODE> list)        //h는 해시코드 나중엔 그냥 uint임 list는 무슨 아이템으로 구성된건지
    {
        //조합법에 들어가는 아이템의 종류의 갯수가 다름
        if(codes.Count != list.Count)
        {
            return false;
        }

        //조합법에 들어가는 아이템의 종류가 다름
        for(int i = 0; i < codes.Count; i++)
        {
            if (codes[i] != list[i])
            {
                return false;
            }
        }

        if (hash == h)
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
            1,1,0,
            1,1,0,
            0,0,0
        };

        recipe = ints;

        hash = Combination.Cutting(recipe); // 1 1 0 1 1로 변환        1 1 0    0 1 1      0 0 0
        codes.Add(_ITEMCODE.SOIL);          // 1이 무슨 아이템인지      1 1 0    0 1 1      1 1 0
                                            //                         0 0 0    0 0 0      1 1 0    다 같은 1 1 0 1 1
        codes.Sort();                       
    }
}

public class Plank_Recipe : Recipe
{
    public Plank_Recipe()
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

public class CraftingBox_Recipe : Recipe
{
    public CraftingBox_Recipe()
    {
        result = _ITEMCODE.CRAFTINGBOX;
        resultCount = 1;

        int[] ints =
        {
            1,1,0,
            1,1,0,
            0,0,0
        };

        recipe = ints;

        hash = Combination.Cutting(recipe);

        codes.Add(_ITEMCODE.PLANK);

        codes.Sort();
    }
}

public class Wooden_Stick_Recipe : Recipe
{
    public Wooden_Stick_Recipe()
    {
        result = _ITEMCODE.WOODEN_STICK;
        resultCount = 4;

        int[] ints =
        {
            1,0,0,
            1,0,0,
            0,0,0
        };

        recipe = ints;

        hash = Combination.Cutting(recipe);

        codes.Add(_ITEMCODE.PLANK);

        codes.Sort();
    }
}

public class Wooden_Sword_Recipe : Recipe
{
    public Wooden_Sword_Recipe()
    {
        result = _ITEMCODE.WOODEN_SWORD;
        resultCount = 1;

        int[] ints =
        {
            0,1,0,
            0,1,0,
            0,2,0
        };

        recipe = ints;

        hash = Combination.Cutting(recipe);

        codes.Add(_ITEMCODE.WOODEN_STICK);
        codes.Add(_ITEMCODE.PLANK);

        codes.Sort();
    }
}