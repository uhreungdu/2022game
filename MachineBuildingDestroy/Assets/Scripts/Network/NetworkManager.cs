using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public enum EventCode : byte
    {
        Test,
        SpawnPlayer,
        StartGame,
        SetTeamOnServer,
        RespawnForReconnect,
        CreateBuildingFromClient,
        DestroyBuildingFromClient,
        CreateBuildingFromServer,
        DestroyBuildingFromServer,
        HideBuildingFragments,
        SpawnPlayerFinish,
        LoadGame
    }

    private static NetworkManager instance;
    private Account _account;
    private LobbyManager _lobbyManager;
    public string networkState;
    public GameObject[] player;
    public GameObject errWindow;
    private GameObject _team1Spawner;
    private GameObject _team2Spawner;
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
        Vector3 pos = new Vector3(-15, 10.0f, -15);

        var info = GameObject.Find("Myroominfo");
        int team = 0;
        if (info != null)
        {
            team = info.GetComponent<MyInRoomInfo>().mySlotNum % 2;
            //Destroy(info);
        }

        while (_team1Spawner == null)
        {
            _team1Spawner = GameObject.FindWithTag("Spawner1");
        }

        while (_team2Spawner == null)
        {
            _team2Spawner = GameObject.FindWithTag("Spawner2");
        }

        if (team == 0 && _team1Spawner != null)
        {
            pos = _team1Spawner.transform.position;
            pos += new Vector3(Random.Range(-4, 4), 10.0f, Random.Range(-4, 4));
        }
        else if (team == 1 && _team2Spawner != null)
        {
            pos = _team2Spawner.transform.position;
            pos += new Vector3(Random.Range(-4, 4), 10.0f, Random.Range(-4, 4));
        }
        
        PhotonNetwork.Instantiate(player[team].name, pos, Quaternion.identity);
        
        SpawnPlayerFinishEvent();
    }

    public void SpawnPlayer(int team)
    {
        Vector3 pos = new Vector3(-15, 10.0f, -15);
        
        while (_team1Spawner == null)
        {
            _team1Spawner = GameObject.FindWithTag("Spawner1");
        }
        while (_team2Spawner == null)
        {
            _team2Spawner = GameObject.FindWithTag("Spawner2");
        }
        
        if (team == 0 && _team1Spawner != null)
        {
            pos = _team1Spawner.transform.position;
            pos += new Vector3(Random.Range(-4, 4), 10.0f, Random.Range(-4, 4));
        }
        else if (team == 1 && _team2Spawner != null)
        {
            pos = _team2Spawner.transform.position;
            pos += new Vector3(Random.Range(-4, 4), 10.0f, Random.Range(-4, 4));
        }
        
        PhotonNetwork.Instantiate(player[team].name, pos, Quaternion.identity);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        StartCoroutine(_lobbyManager.GetRoomList());
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Instantiate(errWindow, new Vector3(Screen.width/2f,Screen.height/2f,0), 
            Quaternion.identity, GameObject.Find("Canvas").transform);
        errWindow.SetActive(true);
        errWindow.GetComponentInChildren<Text>().text = cause.ToString();
        print(cause.ToString());
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        _players = PhotonNetwork.PlayerList;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        _players = PhotonNetwork.PlayerList;
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (Application.platform != RuntimePlatform.Android) return;
        if (SceneManager.GetActiveScene().name == "login_test") return;

        PhotonNetwork.Disconnect();
    }

    private void OnApplicationQuit()
    {
        PhotonNetwork.Disconnect();
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

    public void LoadGameEvent()
    {
        byte evCode = (byte) EventCode.LoadGame;
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

    public void SpawnPlayerFinishEvent()
    {
        byte evCode = (byte) EventCode.SpawnPlayerFinish;
        object[] data = new object[] {PhotonNetwork.NickName};
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