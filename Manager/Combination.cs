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

//���� ����� Combination���� cutting�ϰ� allRecipes �ϳ��ϳ� ���ϴ� ���
//�����Ǹ� ������ cutting�Ѱŷθ� ã���� 1 1 0    11 0 11 ���̸� �𸣱⶧���� �ϳ��ϳ� �迭�� ���ϴµ�
//                                     1 1 0    0  0  0
//                                     0 0 0    0  0  0
//cutting�ѰŶ� ������ ã�� �迭�� �����ϰ� �ٽ� �迭�� �ϳ��ϳ� ���ϴ°ɷ� �ٲٴ°� ������
//cutting�Ѱ� 0 0 1   0 1 1 �� ���̸� �𸣱⿡ ����� �ٽ� �����ؾ���
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


    public static uint Cutting(int[] ints)      //gethashcode�� ���ڿ��� ���������� ���� uint�� ������ ������ �������ڵ带 �ٽ� ���ڸ��� int�� ��ȯ�� �����ؼ� �ִ� �ڸ����� 9�ڸ� unit�� 4,294,967,295����
    {                                           //                                                                                  ��ȯ�ϴ� ������ craftingbox�� Comparison�� ����
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
        return uint.Parse(s);       //������ ���ڿ��� �ٽ� int�� parse������ ���߿� gethashcode�� �ٲ㼭 �ϴ°� ������
    }
}

public class Recipe
{
    public uint hash;
    public _ITEMCODE result;
    public int resultCount;

    public List<_ITEMCODE> codes = new List<_ITEMCODE>();

    public int[] recipe;

    public bool Including(_ITEMCODE code)     //�����ϴ���
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

    public bool Comparison(uint h, List<_ITEMCODE> list)        //h�� �ؽ��ڵ� ���߿� �׳� uint�� list�� ���� ���������� �����Ȱ���
    {
        //���չ��� ���� �������� ������ ������ �ٸ�
        if(codes.Count != list.Count)
        {
            return false;
        }

        //���չ��� ���� �������� ������ �ٸ�
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

        hash = Combination.Cutting(recipe); // 1 1 0 1 1�� ��ȯ        1 1 0    0 1 1      0 0 0
        codes.Add(_ITEMCODE.SOIL);          // 1�� ���� ����������      1 1 0    0 1 1      1 1 0
                                            //                         0 0 0    0 0 0      1 1 0    �� ���� 1 1 0 1 1
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