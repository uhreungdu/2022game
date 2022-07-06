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
    void Start()
    {
        lastTime = -1;
        gamePlayerInput = GetComponentInParent<GamePlayerInput>();
        playerState = GetComponentInParent<PlayerState>();
        gManager = GameManager.GetInstance();
        _UImanager = UImanager.GetInstance();
        
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
            if (gamePlayerInput.Interaction && neargoalpost)
            {
                if (lastTime < 0)
                {
                    lastTime = Time.time;
                    if (photonView.IsMine)
                        _Progressbar.gameObject.SetActive(true);
                }

                if (Time.time >= lastTime + timeBet)
                {
                    if (photonView.IsMine)
                    {
                        photonView.RPC("NetWorkaddScore", RpcTarget.All);
                        int Slotnum = -1;
                        MyInRoomInfo inRoomInfo = MyInRoomInfo.GetInstance();
                        foreach (var info in inRoomInfo.Infomations)
                        {
                            if (info.Name == playerState.NickName)
                               Slotnum = info.SlotNum;
                        }
                        if (Slotnum != -1)
                        {
                            photonView.RPC("AddPointCount", RpcTarget.All, Slotnum, playerState.point);
                            photonView.RPC("GetPointCount", RpcTarget.All, Slotnum, 0);
                        }
                        photonView.RPC("SetOnHeadCoinNum", RpcTarget.All, playerState.point.ToString());
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
        ProgressbarUpdate();
        playerState.update_stat();
    }
    
    [PunRPC]
    public void NetWorkaddScore()
    {
        gManager.addScore(playerState.team, playerState.point);
        playerState.ResetPoint();
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
    
    [PunRPC]
    public void AddPointCount(int SlotNum, int Point)
    {
        MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
        myInRoomInfo.AddPointCount(SlotNum, Point);
    }
    
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
