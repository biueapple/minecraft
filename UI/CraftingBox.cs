using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CraftingBox : MonoBehaviour
{
    public ItemBox[] Boxes = new ItemBox[9];
    public ItemBox result;
    
    private AddImages addImages;
    int index;


    void Start()
    {
        addImages = FindObjectOfType<AddImages>();
        for(int i = 0; i < Boxes.Length; i++)
        {
            if (Boxes[i] != null)
            {
                Boxes[i].Init(null, FindObjectOfType<InventoryView>());
                Boxes[i].put = true;
                Boxes[i].SetPopD(Comparison);
                Boxes[i].SetPutD(Comparison);
            }
        }
        result.AddPoitnerDown(ItemActualization);
        result.AddPoitnerDown(FinishComparison);
        result.Init(null, FindObjectOfType<InventoryView>());
    }


    void Update()
    {
        
    }

    public void ItemActualization()
    {
        for (int i = 0; i < Combination.allRecipes[index].resultCount; i++)
        {
            result.ItemAdd(addImages.CreateItem(Combination.allRecipes[index].result));
        }
    }

    public void FinishComparison()
    {
        for (int i = 0; i < Boxes.Length; i++)
        {
            if (Boxes[i] != null)
            {
                Boxes[i].ItemDelete(1);
                Boxes[i].Setting();
            }
        }

    }

    public void Comparison()
    {
        // 아이템 code 한자리수 int로 변환하는 과정
        int[] recipe = new int[Boxes.Length];
        List<_ITEMCODE> list = new List<_ITEMCODE>();
        for(int i = 0; i < Boxes.Length; i++)
        {
            if (Boxes[i] != null)
            {
                if (Boxes[i].GetCode() != _ITEMCODE.NONE && !list.Contains(Boxes[i].GetCode()))
                {
                    list.Add(Boxes[i].GetCode());
                }
            }
        }

        for(int i = 0; i < Boxes.Length; i++)
        {
            if (Boxes[i] != null && Boxes[i].GetCode() != _ITEMCODE.NONE)
            {
                for(int j = 0; j < list.Count; j++)
                {
                    if (Boxes[i].GetCode() == list[j])
                    {
                        recipe[i] = j + 1;
                        break;
                    }
                    else
                    {
                        recipe[i] = 0;
                    }
                }
            }
            else
            {
                recipe[i] = 0;
            }
        }
        //

        //Combination에서 allRecipe 하나하나 비교하면서 찾는중
        for(int i = 0; i < Combination.allRecipes.Count; i++)
        {
            if (Combination.allRecipes[i].Comparison(Combination.Cutting(recipe), list))
            {
                index = i;
                result.SetView(addImages.GetDataItem(Combination.allRecipes[i].result), Combination.allRecipes[i].resultCount.ToString());
                //result.Setting();
                break;
            }
            else
            {
                result.OffView();
            }
        }
        //


    }



}
