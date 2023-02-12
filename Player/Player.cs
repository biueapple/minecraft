using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(PlayerMouse))]
public class Player : MonoBehaviour
{
    private PlayerInven playerInven;
    private int invenHotkey;

    private bool isReady;
    
    private PlayerMove move;
    private PlayerMouse mouse;

    private Camera cam;

    public Transform head;

    //
    protected Decryption decryption;
    protected Ray ray;
    protected Ray applyRay;
    protected RaycastHit hit;
    protected Vector3 Screen_Center;
    protected Transform startPoint;         //시작점
    protected Transform endPoint;           //끝점        방향은 끝 - 시작의 nomal;
    private bool isNomalRay;
    public float nomal_Distance_Item;          //아이템 상호작용 거리

    void Start()
    {
        playerInven = GetComponent<PlayerInven>();

        cam = Camera.main;

        move = GetComponent<PlayerMove>();
        mouse = GetComponent<PlayerMouse>();

        move.Init(GetComponent<Rigidbody>());
        mouse.Init(transform, cam, head);

        decryption = GetComponent<Decryption>();
        Screen_Center = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2);

        Ready(true);

    }


    void Update()
    {
        if(isReady)
        {
            move.Repetition();
            mouse.Repetition();
            CenterRay();
            HotketInput();
            RightClickDown();
        }

    }
    
    public void LeftClick()
    {
        if(Input.GetMouseButton(1))
        {

        }
    }

    public void RightClickDown()
    {
        if(Input.GetMouseButtonDown(1))
        {
            playerInven.RightClickHotkey(this, decryption);
        }
    }

    public void Ready(bool b)
    {
        isReady = b;
    }

    public void CenterRay()         //ray쏘기 
    {
        if (isNomalRay)
        {
            applyRay = new Ray(startPoint.position, endPoint.position - startPoint.position);
        }
        else
        {
            applyRay = cam.ScreenPointToRay(Screen_Center);
        }

        ray = applyRay;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            decryption.DecryptionSetting(hit);
        }
        else
        {
            decryption.Init();
        }
    }

    public void SetRay(Transform start, Transform end)      //ray를 다른걸로 바꾸기
    {
        if (start == null || end == null)
        {
            isNomalRay = false;
            return;
        }
        startPoint = start;
        endPoint = end;
        isNomalRay = true;
    }

    public void HotketInput()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerInven.SetHotkey(0);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerInven.SetHotkey(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            playerInven.SetHotkey(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            playerInven.SetHotkey(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            playerInven.SetHotkey(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            playerInven.SetHotkey(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            playerInven.SetHotkey(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            playerInven.SetHotkey(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            playerInven.SetHotkey(8);
        }
    }
}
