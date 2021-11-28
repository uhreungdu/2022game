using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    string networkState;
    List<RoomInfo> roomInfos = new List<RoomInfo>();
    public GameObject[] Pannel = new GameObject[4];

    // Start is called before the first frame update
    void Start() =>
        PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster() =>
        PhotonNetwork.JoinLobby();

   // public override void OnJoinedLobby() {
    //    PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions { MaxPlayers = 4 }, null);
   // }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            if(!ControllRoomlist(roomInfos, room.Name))
            {
                roomInfos.Add(room);
            }
        }

        int index = 0;
        foreach (RoomInfo room in roomInfos)
        {
            if (room == null)
                break;
            Pannel[index % 4].GetComponent<RoomPannel>()
                .ChangeText(room.Name, room.PlayerCount, room.MaxPlayers);
            index++;
        }
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

    bool ControllRoomlist(List<RoomInfo> list, string name)
    {
        foreach (RoomInfo room in list)
        {
            if(room.Name == name)
            {
                list.Remove(room);
                return true;
            }
        }
        return false;
    }
}

