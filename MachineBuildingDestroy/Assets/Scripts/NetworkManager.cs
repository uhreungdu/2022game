using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    string networkState;

    // Start is called before the first frame update
    void Start() =>
        PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster() =>
        PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby() {
        PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions { MaxPlayers = 6 }, null);
   }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
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
}

