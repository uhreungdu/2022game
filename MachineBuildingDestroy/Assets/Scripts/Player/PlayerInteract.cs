using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviourPun
{
    [FormerlySerializedAs("playerInput")] public GamePlayerInput gamePlayerInput;
    public PlayerState playerState;
    public GameManager gManager;
    // Start is called before the first frame update
    
    public float timeBet = 3f; // ���� ����
    public LayerMask _LayerMask;
    private float lastTime; // ������ �������� �� ����
    private bool neargoalpost;
    
    public UImanager _UImanager;
    private Slider _Progressbar;
    private Button _interactButton;
    void Start()
    {
        lastTime = -1;
        gamePlayerInput = GetComponentInParent<GamePlayerInput>();
        playerState = GetComponentInParent<PlayerState>();
        gManager = GameManager.GetInstance();
        _UImanager = UImanager.GetInstance();
        
        if (photonView.IsMine && Application.platform == RuntimePlatform.Android)
            _interactButton = GameObject.Find("InteractButton").GetComponent<Button>();
        
        for (int i = 0; i < _UImanager.Canvas.transform.childCount; ++i)
        {
            if (_UImanager.Canvas.transform.GetChild(i).name == "Goalprogression")
            {
                _Progressbar = _UImanager.Canvas.transform.GetChild(i).GetComponent<Slider>();
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
            Collider[] colliders =
                Physics.OverlapSphere(transform.position + new Vector3(0f, 2.5f, 0f), 10, _LayerMask);
            if (colliders.Length >= 1)
                neargoalpost = true;
            else
                neargoalpost = false;

            // 모바일용 동전 넣는버튼 활성화 & 비활성화
            if (photonView.IsMine && Application.platform == RuntimePlatform.Android)
            {
                _interactButton.interactable = neargoalpost;   
            }
            
            if (photonView.IsMine && gamePlayerInput.Interaction && neargoalpost)
            {
                if (lastTime < 0)
                {
                    lastTime = Time.time;
                    _Progressbar.gameObject.SetActive(true);
                }

                if (Time.time >= lastTime + timeBet)
                {
                    if (photonView.IsMine && !playerState.IsCrowdControl())
                    {
                        int Slotnum = -1;
                        MyInRoomInfo inRoomInfo = MyInRoomInfo.GetInstance();
                        foreach (var info in inRoomInfo.Infomations)
                        {
                            if (info.Name == playerState.NickName)
                               Slotnum = info.SlotNum;
                        }
                        if (Slotnum != -1)
                        {
                            //photonView.RPC("AddPointCount", RpcTarget.All, Slotnum, playerState.point);
                            photonView.RPC("AddTotalGetPointCount", RpcTarget.AllViaServer, Slotnum, playerState.point);
                            photonView.RPC("SetPointCount", RpcTarget.AllViaServer, Slotnum, 0);
                            photonView.RPC("NetWorkResetPoint", RpcTarget.AllViaServer);
                        }
                        photonView.RPC("SetOnHeadCoinNum", RpcTarget.AllViaServer, 0.ToString());
                    }
                    if (photonView.IsMine)
                        _Progressbar.gameObject.SetActive(false);
                    lastTime = -1;
                }
            }
            else
            {
                if (photonView.IsMine)
                {
                    _Progressbar.gameObject.SetActive(false);
                    lastTime = -1;
                }
            }

            if (photonView.IsMine)
            {
                ProgressbarUpdate();
            }

            playerState.update_stat();
    }
    
    [PunRPC]
    public void NetWorkaddScore(int point)
    {
        gManager.addScore(playerState.team, point);
        //playerState.ResetPoint();
    }
    [PunRPC]
    public void NetWorkdelScore(int point)
    {
        gManager.delScore(playerState.team, point);
    }
    [PunRPC]
    public void NetWorkResetPoint()
    {
        playerState.ResetPoint();
    }
    
    [PunRPC]
    public void AddTotalGetPointCount(int SlotNum, int Point)
    {
        MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
        myInRoomInfo.AddTotalGetPointCount(SlotNum, Point);
    }

    private void ProgressbarUpdate()
    {
        if (_Progressbar.gameObject.activeSelf)
        {
            _Progressbar.value = (Time.time - lastTime) / 3.0f;
        }
        else
            _Progressbar.value = 0;
    }
    
    // [PunRPC]
    // public void GetPointCount(int Point)
    // {
    //     MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
    //     myInRoomInfo.GetPointCount(myInRoomInfo.mySlotNum, Point);
    // }

    // private void OnTriggerStay(Collider other)
    // {
    //     if (other.tag == "Goalpost")
    //     {
    //         neargoalpost = true;
    //     }
    // }
    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.tag == "Goalpost")
    //     {
    //         neargoalpost = false;
    //     }
    // }
}
