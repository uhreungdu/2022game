using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class RcvEvent : MonoBehaviourPun
{
    enum EventCode : byte
    {
        Test,
        SpawnPlayer,
        StartGame,
        SetTeamOnServer
    }
    
    [SerializeField]
    private GameManager gManager;
    // Start is called before the first frame update
    void Start()
    {
        //gManager = GameManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnEvent(EventData Evdata)
    {
        // 사용자지정 Event가 아니면 return합니다.
        if (Evdata.Code > System.Enum.GetValues(typeof(EventCode)).Length) return;
        
        object[] data = (object[])Evdata.CustomData;
        switch (Evdata.Code)
        {
            case (byte)EventCode.Test:
                for (int i = 0; i < data.Length; i++)
                    Debug.Log(data[i]);
                break;
            case (byte)EventCode.SpawnPlayer:
                transform.GetComponent<NetworkManager>().SpawnPlayer();
                break;
            case(byte)EventCode.StartGame:
                var info = GameObject.Find("Myroominfo");
                int team = -2;
                if (info != null)
                {
                    team = Convert.ToInt32(info.GetComponent<MyInRoomInfo>().MySlotNum > 2);
                }
                transform.GetComponent<NetworkManager>().SetTeamNumOnServerEvent(PhotonNetwork.NickName, team);
                break;
        }
        // Debug.Log("EVENTCALL");
    }

    public void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    public void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }
}
