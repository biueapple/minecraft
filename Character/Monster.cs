
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Monster : Unit
{
    public Character[] players;
    public List<Vector3Int> way = new List<Vector3Int>();
    //private FindingManger manger;
    
    public  float moveSpeed = 3f; // �̵� �ӵ��� ������ ����
    public float turnSpeed = 10f; // ȸ�� �ӵ��� ������ ����
    public float jumpPower = 10f; // ���� ���� ������ ����
    public float jumpInterval = 0.5f; // ���� ������ ������ ����
    float jumpTimer = 1f; // ���� Ÿ�̸Ӹ� ������ ����
    bool isJumping = false; // ���� ������ �Ǵ��� ����
    public float attackInterval = 0.5f; // ���� ������ ������ ����
    float attackTimer = 1f; // ���� Ÿ�̸Ӹ� ������ ����
    public Character target; // ���� ����� ������ ����
    private LayerMask block;

    void Start()
    {
        rigid = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ�� �����ͼ� ������ �Ҵ�
        players = GameObject.FindObjectsOfType<Character>(); // �±װ� Player�� ���ӿ�����Ʈ�� Transform ������Ʈ�� �����ͼ� ������ �Ҵ�
        block = LayerMask.GetMask("Block");
        stat = GetComponent<Stat>();
    }

    void Update()
    {
        FindPlayer(); //ĳ���� ã�� �Լ�
        Move(); // �̵� �Լ� ȣ��
        Jump(); // ���� �Լ� ȣ��
        Attack(); // ���� �Լ� ȣ��
    }

    void Move()
    {
        if(target != null)
        {
            Vector3 direction = target.transform.position - transform.position; // ���� ������ ���� ���� ���
            direction.y = 0f; // y�� ������ ����

            if (direction != Vector3.zero) // ���� ���Ͱ� �����Ͱ� �ƴ϶��
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction); // ���� ���ͷκ��� ��ǥ ȸ���� ���
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime); // ���� ȸ������ ��ǥ ȸ���� ���̸� �ε巴�� �����Ͽ� ȸ��

                float rayDistance = 1f; // Raycast�� �Ÿ��� ������ ����
                Vector3 rayDirection = transform.forward; // Raycast�� ������ ������ ���� (���� ����)

                if (Physics.Raycast(transform.position, rayDirection, rayDistance, block)) // Raycast�� �����Ͽ� �浹�� ��ü�� �ִٸ�
                {
                    //�տ� ����� �־ ���� ����
                }
                else
                {
                    transform.position += transform.forward * moveSpeed * Time.deltaTime; // ���� �������� �̵� �ӵ���ŭ �̵�
                }
            }
        }
    }

    void Jump()
    {
        if (isJumping) return; // �̹� ���� ���̶�� �Լ� ����
        
        jumpTimer += Time.deltaTime; // ���� Ÿ�̸ӿ� ��� �ð� ����

        if (jumpTimer >= jumpInterval) // ���� Ÿ�̸Ӱ� ���� ���ݺ��� ũ�ų� ��������
        {
            RaycastHit hit; // Raycast�� ����� ������ ����
            float rayDistance = 1f; // Raycast�� �Ÿ��� ������ ����
            Vector3 rayDirection = transform.forward; // Raycast�� ������ ������ ���� (���� ����)

            if (Physics.Raycast(transform.position, rayDirection, out hit, rayDistance)) // Raycast�� �����Ͽ� �浹�� ��ü�� �ִٸ�
            {
                if (hit.collider.gameObject.tag == "Block") // �浹�� ��ü�� �±װ� Block�̶��
                {
                    isJumping = true; // ���� �� ���·� ����
                    jumpTimer = 0f; // ���� Ÿ�̸� �ʱ�ȭ
                    rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse); // Rigidbody�� ���� �������� ���� ����
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision) // �ٴڰ� �浹�ϸ�
    {
        if (collision.gameObject.tag == "Block") // �浹�� ���ӿ�����Ʈ�� �±װ� Block�̶��
        {
            isJumping = false; // ���� �� ���� ����
        }
    }

    void Attack()
    {
        if(target != null)
        {
            attackTimer += Time.deltaTime;
            if (Vector3.Distance(target.transform.position, transform.position) < 1.5f) // ���� ������ �Ÿ��� 2�̸��̸�
            {
                if(attackTimer >= attackInterval)
                {
                    attackTimer = 0;
                    target.Hit(transform);
                    Debug.Log("Attack!"); // ���� ���� ���� (���⼭�� �ֿܼ� �޽��� ���)
                }
            }
        }
    }

    public void FindPlayer()
    {
        for(int i = 0; i < players.Length; i++)
        {
            if (Vector3.Distance(transform.position, players[i].transform.position) < 10)
            {
                target = players[i];
            }
        }
    }

    public void SetWay(List<_NODE> nodes)
    {
        way.Clear();
        for (int i = 0; i < nodes.Count; i++)
        {
            way.Add(nodes[i].position);
        }
    }

}
