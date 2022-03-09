using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Networking;
using ExitGames.Client.Photon;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    private static LobbyManager instance;
    [SerializeField]
    private string PlayerName;
    private string exroomName;
    private string inroomName;
    public GameObject Room;
    string networkState;


    public static LobbyManager GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<LobbyManager>();
            if (instance == null)
            {
                GameObject container = new GameObject("LobbyManager");
                instance = container.AddComponent<LobbyManager>();
            }
        }
        return instance;
    }

    void Awake()
    {
        var obj = FindObjectsOfType<LobbyManager>(); 
        if (obj.Length == 1) 
        { 
            DontDestroyOnLoad(gameObject); 
        } 
        else 
        { 
            Destroy(gameObject); 
        }
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        StartCoroutine(GetRoomList());
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

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        StartCoroutine(GetRoomList());
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Join OK");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        StartCoroutine(GetRoomList());
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        StartCoroutine(GetRoomList());
    }

    public void SetName(string text)
    {
        PlayerName = text;
    }

    public string GetName()
    {
        return PlayerName;
    }

    public void SetInRoomName(string text)
    {
        inroomName = text;
    }

    public string GetInRoomName()
    {
        return inroomName;
    }

    public void SetExRoomName(string text)
    {
        exroomName = text;
    }

    public string GetExRoomName()
    {
        return exroomName;
    }


    public IEnumerator GetRoomList()
    {
        WWWForm form = new WWWForm();

        UnityWebRequest www = UnityWebRequest.Post("http://121.139.87.70/get_room_list.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string results = www.downloadHandler.text;
            Debug.Log(results);
            GameObject.Find("RoomList").GetComponent<RoomList>().SetRoomList(results.Split(';'));
            GameObject.Find("RoomList").GetComponent<RoomList>().SetRoomBlocks();
        }
    }
}
