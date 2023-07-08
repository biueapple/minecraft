using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropItemManager : MonoBehaviour
{
    public List<Item> drops = new List<Item>();
    public float roatateSpeed;
    private Coroutine rotation;

    public void DropItem(Item item)
    {
        drops.Add(item);
        if(rotation == null)
        {
            rotation = StartCoroutine(RotationCoroutine());
        }
    }

    public void AcquiredItem(Item item)
    {
        drops.Remove(item);
    }

    private IEnumerator RotationCoroutine()
    {
        float ro = 0;
        while (drops.Count > 0)
        {
            ro += Time.deltaTime * roatateSpeed;
            for (int i = 0; i < drops.Count; i++)
            {
                drops[i].transform.localEulerAngles = new Vector3(0, ro, 0);
            }

            yield return null;
        }
        rotation = null;
    }

}
