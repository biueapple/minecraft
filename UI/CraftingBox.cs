using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CraftingBox : MonoBehaviour
{
    //private ItemBox lastClickBox;
    public ItemBox[] Boxes;
    public ItemBox result;
    
    private AddImages addImages;



    void Start()
    {
        addImages = FindObjectOfType<AddImages>();


    }


    void Update()
    {
        
    }


    public void FinishComparison()
    {
        for (int i = 0; i < Boxes.Length; i++)
        {
            Boxes[i].DeleteItems();
        }

    }

    public void Comparison()
    {
        // 아이템 code 한자리수 int로 변환하는 과정
        int[] recipe = new int[Boxes.Length];
        List<_ITEMCODE> list = new List<_ITEMCODE>();
        for(int i = 0; i < Boxes.Length; i++)
        {
            if (Boxes[i].GetItem() != null && !list.Contains(Boxes[i].GetItem().scriptble.GetCode()))
            {
                list.Add(Boxes[i].GetItem().scriptble.GetCode());
            }
        }

        for(int i = 0; i < Boxes.Length; i++)
        {
            if (Boxes[i].GetItem() != null)
            {
                for(int j = 0; j < list.Count; j++)
                {
                    if (Boxes[i].GetItem().scriptble.GetCode() == list[j])
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
        }
        //

        //Combination에서 allRecipe 하나하나 비교하면서 찾는중
        for(int i = 0; i < Combination.allRecipes.Count; i++)
        {
            if (Combination.allRecipes[i].Comparison(Combination.Cutting(recipe), list))
            {
                List<Item> items = new List<Item>();

                for(int j = 0; j < Combination.allRecipes[i].resultCount; j++)
                {
                    items.Add(addImages.CreateItem(Combination.allRecipes[i].result));
                }
               
                result.SetItemList(items);
                result._text.text = Combination.allRecipes[i].resultCount.ToString();
                break;
            }
            else
            {
                result.SetItemList(null);
            }
        }
        //


    }



}
