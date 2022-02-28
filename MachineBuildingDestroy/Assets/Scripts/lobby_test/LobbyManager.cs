using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    private static LobbyManager instance;
    [SerializeField]
    private string PlayerName;
    string networkState;
    

    public static LobbyManager GetInstance()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<LobbyManager>();
            if(instance == null)
            {
                GameObject container = new GameObject("LobbyManager");
                instance = container.AddComponent<LobbyManager>();
            }
        }
        return instance;
    }

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start() =>
        PhotonNetwork.ConnectUsingSettings();

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

    public void SetName(string text)
    {
        PlayerName = text;
    }

}
