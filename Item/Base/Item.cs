using UnityEngine;

[System.Serializable]
public class Item : MonoBehaviour
{
    public ItemScriptble scriptble;

    private void Start()
    {
        
    }

    /// <summary>
    /// �������� ��ӵɶ� ȣ��
    /// </summary>
    /// <param name="position">��𿡼� ��ӵɰ���</param>
    public virtual void Drop(Vector3 position)
    {
        DropItemManager manager = FindObjectOfType<DropItemManager>();
        transform.SetParent(manager.transform, false);
        transform.position = position;
        manager.DropItem(this);
        GetComponent<Rigidbody>().AddForce(Vector3.up * 100f);
    }

    /// <summary>
    /// ���߿� ��Ŭ������ ȿ���� ���鶧 override
    /// </summary>
    /// <param name="player">����</param>
    /// <param name="decryption">����</param>
    /// <returns></returns>
    public virtual bool RightClick(Player player, Decryption decryption)
    {
        return false;
    }


    /// <summary>
    /// �������� ������� ��ġ�Ҷ� �ٸ� ���𰡰� �ű� �ִٸ� ��ġ�� �Ұ����� ����
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public bool BlockCk(Vector3 vector)       //��ġ�������� üũ
    {
        Collider[] coll = Physics.OverlapBox(vector, new Vector3(0.3f, 0.3f, 0.3f));
        if(coll.Length > 0)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// �������� �浹������ �÷��̾�� ������ ȹ��
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
