using System;
using System.Collections;
using UnityEngine;


[System.Serializable]
public class Item : MonoBehaviour
{
    protected Unit unit;
    public ItemScriptble scriptble;

    /// <summary>
    /// 아이템이 드롭될때 호출
    /// </summary>
    /// <param name="position">어디에서 드롭될건지</param>
    public virtual void Drop(Vector3 position)
    {
        DropItemManager manager = FindObjectOfType<DropItemManager>();
        gameObject.SetActive(true);
        transform.SetParent(manager.transform, false);
        transform.position = position;
        manager.DropItem(this);
        CollisionCreation();
        GetComponent<Rigidbody>().AddForce(Vector3.up * 100f);
    }

    /// <summary>
    /// 아이템을 블록으로 설치할때 다른 무언가가 거기 있다면 설치가 불가능한 상태
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public bool BlockCk(Vector3 vector)       //설치가능한지 체크
    {
        Collider[] coll = Physics.OverlapBox(vector, new Vector3(0.3f, 0.3f, 0.3f));
        if(coll.Length > 0)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 아이템이 충돌됐을때 플레이어면 아이템 획득
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.GetComponent<CharacterInven>() != null)
        {
            if(collision.transform.GetComponent<CharacterInven>().ItemAdd(this))
            {
                Acquired(collision.transform.GetComponent<Unit>());
            }
        }
    }

    public void Acquired(Unit unit)
    {
        this.unit = unit;
        FindObjectOfType<DropItemManager>().AcquiredItem(this);
    }

    public void CollisionRemoval()
    {
        if(GetComponent<Rigidbody>() != null)
        Destroy(GetComponent<Rigidbody>());
        if(GetComponent<Collider>() != null)
        Destroy(GetComponent<Collider>());
    }

    public void CollisionCreation()
    {
        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody body = gameObject.AddComponent<Rigidbody>();
            body.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
        } 
        if(GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
        }
    }

    public void Throw(Vector3 posi, Vector3 dir, Action<Item> action)
    {
        if (GetComponent<Rigidbody>() == null || GetComponent<Collider>() == null) { CollisionCreation(); }
        gameObject.GetComponent<Collider>().isTrigger = true;
        transform.GetComponent<Rigidbody>().AddForce(dir * 300);
        StartCoroutine(CollCheck(posi, dir, action));
    }

    private IEnumerator CollCheck(Vector3 posi, Vector3 dir, Action<Item> action)
    {
        bool one = false;
        bool two = false;
        transform.position = posi;
        while (true)
        {
            yield return null;

            if (Physics.Raycast(transform.position, dir, transform.localScale.z * 0.5f + 0.1f) && !one)
            {
                Vector3 v = transform.GetComponent<Rigidbody>().velocity;
                transform.GetComponent<Rigidbody>().velocity = new Vector3(0, v.y, 0);
                one = true;
            }

            if (Physics.Raycast(transform.position, Vector3.down, transform.localScale.y * 0.5f + 0.1f) && !two)
            {
                transform.GetComponent<Collider>().isTrigger = false;
                Destroy(GetComponent<Rigidbody>());
                one = true;
                two = true;
            }

            if (one && two)
            {
                action(this);
                break;
            }
        }
    }

    public virtual Item Init() { return this; }
}
