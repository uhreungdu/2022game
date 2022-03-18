using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class NNetworkManager : MonoBehaviourPunCallbacks
{
    enum EventCode : byte
    {
        Test,
        RenewScore,
        CreateItem
    }
    private static NNetworkManager instance;
    private Account _account;
    private LobbyManager _lobbyManager;
    string networkState;
    public GameObject Player;
    public GameObject Map;

    public static NNetworkManager GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<NNetworkManager>();
            if (instance == null)
            {
                GameObject container = new GameObject("LobbyManager");
                instance = container.AddComponent<NNetworkManager>();
            }
        }
        return instance;
    }

    void Awake()
    {
        var obj = FindObjectsOfType<NNetworkManager>(); 
        if (obj.Length == 1) 
        { 
            DontDestroyOnLoad(gameObject); 
        } 
        else 
        { 
            Destroy(gameObject); 
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //_account = Account.GetInstance();
        ConnectPhotonServer();
    }

    public override void OnConnectedToMaster() =>
        PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby() {
       // _lobbyManager = LobbyManager.GetInstance();
        PhotonNetwork.CreateRoom("ddd");
    }

    public override void OnJoinedRoom()
    {
       // Map = GameObject.Find("Map");
        Vector3 Pos = new Vector3(0, 10, 0);
        if (PhotonNetwork.IsMasterClient)
        {
           // Map.GetComponent<Map>().CreateNetworkMap();
        }
        PhotonNetwork.Instantiate(Player.name, Pos, Quaternion.identity);
    }
    
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
      //  StartCoroutine(_lobbyManager.GetRoomList());
    }
    
    public override void OnDisconnected(DisconnectCause cause)
    {
     //   ExitRoom(_account.GetPlayerID(),_lobbyManager.GetInRoomName());
    }

    private void OnApplicationQuit()
    {
    //    ExitRoom(_account.GetPlayerID(),_lobbyManager.GetInRoomName());
    }

    // Update is called once per frame
    void Update()
    {
        string curNetworkState = PhotonNetwork.NetworkClientState.ToString();
       if (networkState != curNetworkState)
        {
            networkState = curNetworkState;
            print(networkState);
        }
    }

    void ExitRoom(string id, string roomname)
    {
        WWWForm form = new WWWForm();
        form.AddField("Pname", "\""+id+"\"") ;
        form.AddField("iname", "\""+roomname+"\"") ;
        UnityWebRequest www = UnityWebRequest.Post("http://121.139.87.70/player_exit_room.php", form);
        www.SendWebRequest();
    }

    void RaiseEventSample()
    {
        byte evCode = (byte)EventCode.Test;
        object[] data = new object[] { "test", "sample", 7, 7, 1 };
        RaiseEventOptions RaiseOpt = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOpt = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, data, RaiseOpt, sendOpt);
    }

    public void RenewGameScore(int team, int point)
    {
        byte evCode = (byte)EventCode.RenewScore;
        object[] data = new object[] { team, point };
        RaiseEventOptions RaiseOpt = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOpt = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, data, RaiseOpt, sendOpt);
    }

    public void RequestCreateItem(int type)
    {
        byte evCode = (byte)EventCode.CreateItem;
        object[] data = new object[] { 0, false };  // type, result
        RaiseEventOptions RaiseOpt = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOpt = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, data, RaiseOpt, sendOpt);
    }

    public void ConnectPhotonServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    
}

