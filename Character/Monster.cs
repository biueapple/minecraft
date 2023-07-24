
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Monster : Unit
{
    public Character[] players;
    public List<Vector3Int> way = new List<Vector3Int>();
    //private FindingManger manger;
    
    public  float moveSpeed = 3f; // 이동 속도를 조절할 변수
    public float turnSpeed = 10f; // 회전 속도를 조절할 변수
    public float jumpPower = 10f; // 점프 힘을 조절할 변수
    public float jumpInterval = 0.5f; // 점프 간격을 조절할 변수
    float jumpTimer = 1f; // 점프 타이머를 저장할 변수
    bool isJumping = false; // 점프 중인지 판단할 변수
    public float attackInterval = 0.5f; // 점프 간격을 조절할 변수
    float attackTimer = 1f; // 점프 타이머를 저장할 변수
    public Character target; // 추적 대상을 저장할 변수
    private LayerMask block;

    void Start()
    {
        rigid = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트를 가져와서 변수에 할당
        players = GameObject.FindObjectsOfType<Character>(); // 태그가 Player인 게임오브젝트의 Transform 컴포넌트를 가져와서 변수에 할당
        block = LayerMask.GetMask("Block");
        stat = GetComponent<Stat>();
    }

    void Update()
    {
        FindPlayer(); //캐릭터 찾는 함수
        Move(); // 이동 함수 호출
        Jump(); // 점프 함수 호출
        Attack(); // 공격 함수 호출
    }

    void Move()
    {
        if(target != null)
        {
            Vector3 direction = target.transform.position - transform.position; // 추적 대상과의 방향 벡터 계산
            direction.y = 0f; // y축 방향은 무시

            if (direction != Vector3.zero) // 방향 벡터가 영벡터가 아니라면
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction); // 방향 벡터로부터 목표 회전값 계산
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime); // 현재 회전값과 목표 회전값 사이를 부드럽게 보간하여 회전

                float rayDistance = 1f; // Raycast의 거리를 설정할 변수
                Vector3 rayDirection = transform.forward; // Raycast의 방향을 설정할 변수 (전진 방향)

                if (Physics.Raycast(transform.position, rayDirection, rayDistance, block)) // Raycast를 실행하여 충돌한 물체가 있다면
                {
                    //앞에 블록이 있어서 전진 멈춤
                }
                else
                {
                    transform.position += transform.forward * moveSpeed * Time.deltaTime; // 전진 방향으로 이동 속도만큼 이동
                }
            }
        }
    }

    void Jump()
    {
        if (isJumping) return; // 이미 점프 중이라면 함수 종료
        
        jumpTimer += Time.deltaTime; // 점프 타이머에 경과 시간 더함

        if (jumpTimer >= jumpInterval) // 점프 타이머가 점프 간격보다 크거나 같아지면
        {
            RaycastHit hit; // Raycast의 결과를 저장할 변수
            float rayDistance = 1f; // Raycast의 거리를 설정할 변수
            Vector3 rayDirection = transform.forward; // Raycast의 방향을 설정할 변수 (전진 방향)

            if (Physics.Raycast(transform.position, rayDirection, out hit, rayDistance)) // Raycast를 실행하여 충돌한 물체가 있다면
            {
                if (hit.collider.gameObject.tag == "Block") // 충돌한 물체의 태그가 Block이라면
                {
                    isJumping = true; // 점프 중 상태로 변경
                    jumpTimer = 0f; // 점프 타이머 초기화
                    rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse); // Rigidbody에 위쪽 방향으로 힘을 가함
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision) // 바닥과 충돌하면
    {
        if (collision.gameObject.tag == "Block") // 충돌한 게임오브젝트의 태그가 Block이라면
        {
            isJumping = false; // 점프 중 상태 해제
        }
    }

    void Attack()
    {
        if(target != null)
        {
            attackTimer += Time.deltaTime;
            if (Vector3.Distance(target.transform.position, transform.position) < 1.5f) // 추적 대상과의 거리가 2미만이면
            {
                if(attackTimer >= attackInterval)
                {
                    attackTimer = 0;
                    target.Hit(transform);
                    Debug.Log("Attack!"); // 공격 로직 구현 (여기서는 콘솔에 메시지 출력)
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
