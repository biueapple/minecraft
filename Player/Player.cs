using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(PlayerMouse))]
public class Player : MonoBehaviour
{
    private PlayerInven playerInven;
    private PlayerMouse playerMouse;
    private UIController controller;
    private TopographyParent topographyParent;



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
    //

    public Animator animator;

    private List<Action<Vector3Int>> install = new List<Action<Vector3Int>>();
    private List<Action<Vector3Int>> mining = new List<Action<Vector3Int>>();

    void Start()
    {
        playerInven = GetComponent<PlayerInven>();
        playerMouse = GetComponent<PlayerMouse>();
        controller = FindObjectOfType<UIController>();
        topographyParent = FindObjectOfType<TopographyParent>();

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
            LeftDown();
            LeftUp();
        }

    }
    
    public void LeftDown()
    {
        if(Input.GetMouseButton(0) && playerMouse.isMove)
        {
            if(decryption.solidBlock != null)
            {
                decryption.solidBlock.Mining(playerInven.GetHotkeyBox().GetItem());
                if (decryption.solidBlock.GetDurability() >= decryption.solidBlock.blockScriptble.GetStrength())
                {
                    topographyParent.BrokenBlock(decryption.solidBlock.GetPosition());
                    decryption.solidBlock.Destruction(playerInven.GetHotkeyBox().GetItem());
                    for(int i = 0; i < mining.Count ; i++)
                    {
                        mining[i](decryption.solidBlock.GetPosition());
                    }
                }
            }
            animator.SetBool("Mining", true);
        }
    }

    public void LeftUp()
    {
        if(Input.GetMouseButtonUp(0))
        {
            if (animator.GetBool("Mining") == true)
            {
                animator.SetBool("Mining", false);
            }
        }
    }

    public void RightClickDown()
    {
        if(Input.GetMouseButtonDown(1) && playerMouse.isMove)
        {
            if(playerInven.hand != null && playerInven.hand.GetItemList().Count > 0)
            {
                if (playerInven.hand.GetItemList()[0].GetComponent<InstallItem>() != null)
                {
                    Vector3Int po = playerInven.hand.GetItemList()[0].GetComponent<InstallItem>().CalculatePosition(this, decryption);
                    if (po != default(Vector3Int) && playerInven.hand.GetItemList()[0].GetComponent<InstallItem>().Installation(po))
                    {
                        playerInven.HandConsume();
                        for(int i = 0; i < install.Count ; i++)
                        {
                            install[i](po);
                        }
                    } 
                }
            }
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
            if(decryption.DecryptionSetting(hit))
            {

            }
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

    public void OpenCrafting_9x9()
    {
        controller.InventoryKey();
        controller.CraftingKey(2);
        if (controller.uiOpens.Count <= 0)
        {
            playerMouse.isMove = true;
        }
        else
        {
            playerMouse.isMove = false;
        }
    }

    public void AddMiningAction(Action<Vector3Int> action)
    {
        mining.Add(action);
    }
    public void RemoveMiningAction(Action<Vector3Int> action)
    {
        mining.Remove(action);
    }
    public void AddInstall(Action<Vector3Int> action)
    {
        install.Add(action);
    }
    public void RemoveInstall(Action<Vector3Int> action)
    {
        install.Remove(action);
    }
}
