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
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        StartCoroutine(WebRequest());
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
        GameObject.Find("RoomList").GetComponent<RoomList>().SetRoomBlocks();
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


    IEnumerator WebRequest()
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
            GameObject.Find("RoomList").GetComponent<RoomList>().SetRoomList(results.Split(';'));
            GameObject.Find("RoomList").GetComponent<RoomList>().SetRoomBlocks();
        }
    }
}
