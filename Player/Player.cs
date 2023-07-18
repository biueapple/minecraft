using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMouse))]
public class Player : MonoBehaviour
{
    private PlayerMouse playerMouse;
    private UIManager uiManager;
    private TopographyParent topographyParent;



    private bool isReady;
    
    private PlayerMouse mouse;

    private Camera cam;

    public Transform head;

    public Unit character;

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

    private bool battle = false;

    public Animator animator;

    private List<Action<Vector3Int>> install = new List<Action<Vector3Int>>();
    private List<Action<Vector3Int>> mining = new List<Action<Vector3Int>>();

    void Start()
    {
        playerMouse = GetComponent<PlayerMouse>();
        uiManager = FindObjectOfType<UIManager>();
        topographyParent = FindObjectOfType<TopographyParent>();

        cam = Camera.main;

        mouse = GetComponent<PlayerMouse>();

        mouse.Init(character.transform, cam, head);

        decryption = GetComponent<Decryption>();
        Screen_Center = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2);

        Ready(true);

        character.Init();
    }


    void Update()
    {
        if(isReady)
        {
            mouse.Repetition();
            OpenInventoryKey();
            InputCloseKey();
            OpenEquipKey();
            OpenSkillKey();
            InputModeChange();

            if (!battle)
            {
                CenterRay();
                HotketInput();
                //InstallBlock(); InteractBlock안에 있음
                InteractBlock();
                Mining();
                MiningEnd();
            }
            else
            {

            }
        }

    }


    
    public void OpenInventoryKey()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!uiManager.Contain(character.GetInven().craftingWindow))
            {
                uiManager.AllClose();
                uiManager.OpenUI(character.GetInven().craftingWindow);
                uiManager.OpenUI(character.GetInven().inventoryWindow);
                playerMouse.isMove = false;
            }
            else
            {
                uiManager.AllClose();
                MoveCheck();
            }
        }
    }

    public void OpenEquipKey()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!uiManager.Contain(character.GetInven().equipWindow))
            {
                uiManager.AllClose();
                uiManager.OpenUI(character.GetInven().inventoryWindow);
                uiManager.OpenUI(character.GetInven().equipWindow);
                character.GetEquip().StatText();
                playerMouse.isMove = false;
            }
            else
            {
                uiManager.AllClose();
                MoveCheck();
            }
        }
    }

    public void OpenSkillKey()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (!uiManager.Contain(character.GetSkillManager().skillTreeSelectWindow))
            {
                uiManager.AllClose();
                uiManager.OpenUI(character.GetSkillManager().skillTreeSelectWindow);
                playerMouse.isMove = false;
            }
            else
            {
                uiManager.AllClose();
                MoveCheck();
            }
        }
    }

    public void InputModeChange()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            battle = !battle;
            if(battle)
            {
                character.BattleMode();
            }
            else
            {
                character.PeaceMode();
            }
        }
    }


    public void InputCloseKey()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            uiManager.CloseUI();
            MoveCheck();
        }
    }

    private void MoveCheck()
    {
        if (uiManager.GetUICount() == 0)
        {
            playerMouse.isMove = true;
        }
    }

    public void Mining()
    {
        if (Input.GetMouseButton(0) && playerMouse.isMove)
        {
            if (decryption.solidBlock != null && decryption.Distance <= nomal_Distance_Item)
            {
                decryption.solidBlock.Mining(character.GetInven().GetHand().GetItem());
                if (decryption.solidBlock.GetDurability() >= decryption.solidBlock.blockScriptble.GetStrength())
                {
                    topographyParent.BrokenBlock(decryption.solidBlock.GetPosition());
                    decryption.solidBlock.Destruction(character.GetInven().GetHand().GetItem());
                    for (int i = 0; i < mining.Count; i++)
                    {
                        mining[i](decryption.solidBlock.GetPosition());
                    }
                }
            }
            animator.SetBool("Mining", true);
        }
    }

    public void MiningEnd()
    {
        if(Input.GetMouseButtonUp(0))
        {
            if (animator.GetBool("Mining") == true)
            {
                animator.SetBool("Mining", false);
            }
        }
    }

    public void InstallBlock()
    {
        if (character.GetInven().GetHand() != null && character.GetInven().GetHand().GetItem() != null)
        {
            if (character.GetInven().GetHand().GetItem().GetComponent<InstallItem>() != null)
            {
                Vector3Int po = character.GetInven().GetHand().GetItem().GetComponent<InstallItem>().CalculatePosition(this, decryption);
                if (po != default(Vector3Int) && character.GetInven().GetHand().GetItem().GetComponent<InstallItem>().Installation(po))
                {
                    character.GetInven().ItemDelete();
                    for (int i = 0; i < install.Count; i++)
                    {
                        install[i](po);
                    }
                }
            }
        }
    }

    public void InteractBlock()
    {
        if (Input.GetMouseButtonDown(1) && playerMouse.isMove)
        {
            if (decryption.interBlock != null && decryption.interBlock.RightClick(this))
            {

            }
            else
                InstallBlock();
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
            if(decryption.DecryptionSetting(hit, character.transform))
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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            character.GetInven().HandChange(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            character.GetInven().HandChange(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            character.GetInven().HandChange(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            character.GetInven().HandChange(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            character.GetInven().HandChange(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            character.GetInven().HandChange(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            character.GetInven().HandChange(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            character.GetInven().HandChange(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            character.GetInven().HandChange(8);
        }
    }

    public void HoykeySkill()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //character.GetSkillUI().hotkeys[0].GetSkill().Single_Effect();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //character.GetSkillUI().hotkeys[1].GetSkill().Single_Effect();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //character.GetSkillUI().hotkeys[2].GetSkill().Single_Effect();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //character.GetSkillUI().hotkeys[3].GetSkill().Single_Effect();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            //character.GetSkillUI().hotkeys[4].GetSkill().Single_Effect();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            //character.GetSkillUI().hotkeys[5].GetSkill().Single_Effect();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            //character.GetSkillUI().hotkeys[6].GetSkill().Single_Effect();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            //character.GetSkillUI().hotkeys[7].GetSkill().Single_Effect();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            //character.GetSkillUI().hotkeys[8].GetSkill().Single_Effect();
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



    public PlayerMouse GetPlayerMouse() {  return  playerMouse; }
}
