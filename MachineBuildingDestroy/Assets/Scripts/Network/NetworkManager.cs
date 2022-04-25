using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Random = UnityEngine.Random;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    enum EventCode : byte
    {
        Test,
        SpawnPlayer,
        StartGame,
        SetTeamOnServer
    }

    private static NetworkManager instance;
    private Account _account;
    private LobbyManager _lobbyManager;
    public string networkState;
    public GameObject Player;
    public Map Map;
    public GameObject ErrWindow;
    private GameObject team1spawner;
    private GameObject team2spawner;
    [SerializeField] private Player[] _players;

    public static NetworkManager GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<NetworkManager>();
            if (instance == null)
            {
                GameObject container = new GameObject("NetworkManager");
                instance = container.AddComponent<NetworkManager>();
            }
        }

        return instance;
    }

    void Awake()
    {
        var obj = FindObjectsOfType<NetworkManager>();
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
        _account = Account.GetInstance();
        _players = PhotonNetwork.PlayerList;
        ConnectPhotonServer();
    }

    public override void OnJoinedLobby()
    {
        _lobbyManager = LobbyManager.GetInstance();
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel("ReadyRoom");

    }

    public void SpawnPlayer()
    {
        Vector3 Pos = new Vector3(-15, 10.0f, -15);
        
        var info = GameObject.Find("Myroominfo");
        int team = 0;
        if (info != null)
        {
            team = Convert.ToInt32(info.GetComponent<MyInRoomInfo>().MySlotNum > 2);
            Destroy(info);
        }
        GameObject team1spawner = null;
        GameObject team2spawner = null;
        while (team1spawner == null)
        {
            team1spawner = GameObject.FindWithTag("Spawner1");
        }
        while (team2spawner == null)
        {
            team2spawner = GameObject.FindWithTag("Spawner2");
        }
        
        if (team == 0 && team1spawner != null)
        {
            Pos = team1spawner.transform.position;
            Pos += new Vector3(Random.Range(-4, 4), 10.0f, Random.Range(-4, 4));
        }
        else if (team == 1 && team2spawner != null)
        {
            Pos = team2spawner.transform.position;
            Pos += new Vector3(Random.Range(-4, 4), 10.0f, Random.Range(-4, 4));
        }
        
        PhotonNetwork.Instantiate(Player.name, Pos, Quaternion.identity);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        StartCoroutine(_lobbyManager.GetRoomList());
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Instantiate(ErrWindow, new Vector3(Screen.width/2f,Screen.height/2f,0), 
            Quaternion.identity, GameObject.Find("Canvas").transform);
        ErrWindow.SetActive(true);
        ErrWindow.GetComponentInChildren<Text>().text = cause.ToString();
        print(cause.ToString());
        Logout(_account.GetPlayerID());
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        _players = PhotonNetwork.PlayerList;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        _players = PhotonNetwork.PlayerList;
    }

    private void OnApplicationQuit()
    {
        Logout(_account.GetPlayerID());
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
        form.AddField("Pname", "\"" + id + "\"");
        form.AddField("iname", "\"" + roomname + "\"");
        UnityWebRequest www = UnityWebRequest.Post("http://121.139.87.70/player_exit_room.php", form);
        www.SendWebRequest();
    }

    void Logout(string id)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", "\"" + id + "\"");
        UnityWebRequest www = UnityWebRequest.Post("http://121.139.87.70/login/logout_account.php", form);
        www.SendWebRequest();
    }

    void RaiseEventSample()
    {
        byte evCode = (byte) EventCode.Test;
        object[] data = new object[] {"test", "sample", 7, 7, 1};
        RaiseEventOptions RaiseOpt = new RaiseEventOptions {Receivers = ReceiverGroup.All};
        SendOptions sendOpt = new SendOptions {Reliability = true};
        PhotonNetwork.RaiseEvent(evCode, data, RaiseOpt, sendOpt);
    }

    public void SpawnPlayerEvent()
    {
        byte evCode = (byte) EventCode.SpawnPlayer;
        object[] data = new object[] { };
        RaiseEventOptions RaiseOpt = new RaiseEventOptions {Receivers = ReceiverGroup.All};
        SendOptions sendOpt = new SendOptions {Reliability = true};
        PhotonNetwork.RaiseEvent(evCode, data, RaiseOpt, sendOpt);
    }

    public void StartGameEvent()
    {
        byte evCode = (byte) EventCode.StartGame;
        object[] data = new object[] { };
        RaiseEventOptions RaiseOpt = new RaiseEventOptions {Receivers = ReceiverGroup.All};
        SendOptions sendOpt = new SendOptions {Reliability = true};
        PhotonNetwork.RaiseEvent(evCode, data, RaiseOpt, sendOpt);
    }

    public void SetTeamNumOnServerEvent(string name, int num)
    {
        byte evCode = (byte) EventCode.SetTeamOnServer;
        object[] data = new object[] {name, num};
        RaiseEventOptions RaiseOpt = new RaiseEventOptions {Receivers = ReceiverGroup.All};
        SendOptions sendOpt = new SendOptions {Reliability = true};
        PhotonNetwork.RaiseEvent(evCode, data, RaiseOpt, sendOpt);
    }

    public void ConnectPhotonServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
}