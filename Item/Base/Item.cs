using UnityEngine;

[System.Serializable]
public class Item : MonoBehaviour
{
    public ItemScriptble scriptble;

    private void Start()
    {
        
    }

    /// <summary>
    /// 아이템이 드롭될때 호출
    /// </summary>
    /// <param name="position">어디에서 드롭될건지</param>
    public virtual void Drop(Vector3 position)
    {
        DropItemManager manager = FindObjectOfType<DropItemManager>();
        transform.SetParent(manager.transform, false);
        transform.position = position;
        manager.DropItem(this);
        GetComponent<Rigidbody>().AddForce(Vector3.up * 100f);
    }

    /// <summary>
    /// 나중에 우클릭으로 효과를 만들때 override
    /// </summary>
    /// <param name="player">누가</param>
    /// <param name="decryption">정보</param>
    /// <returns></returns>
    public virtual bool RightClick(Player player, Decryption decryption)
    {
        return false;
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
        if(collision.transform.GetComponent<PlayerInven>() != null)
        {
            FindObjectOfType<DropItemManager>().AcquiredItem(this);
            collision.transform.GetComponent<PlayerInven>().ItemInput(this);
        }
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
            gameObject.AddComponent<Collider>();
    }
}
