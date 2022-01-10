using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    string networkState;
    public GameObject Player;

    // Start is called before the first frame update
    void Start() =>
        PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster() =>
        PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby() {
        PhotonNetwork.JoinOrCreateRoom("HJroom", new RoomOptions { MaxPlayers = 6 }, null);
   }

    public override void OnJoinedRoom()
    {
        Vector3 Pos = new Vector3(0, 10, 0);
        PhotonNetwork.Instantiate(Player.name, Pos, Quaternion.identity);
        RaiseEventSample();
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

    void RaiseEventSample()
    {
        byte evCode = 0;
        object[] data = new object[] { "test", "sample", 7, 7, 1 };
        RaiseEventOptions RaiseOpt = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOpt = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, data, RaiseOpt, sendOpt);
    }
}

