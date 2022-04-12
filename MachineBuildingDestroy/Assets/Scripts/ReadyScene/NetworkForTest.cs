using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkForTest : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnConnectedToMaster()
        => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby()
    => PhotonNetwork.JoinOrCreateRoom("a",new RoomOptions {MaxPlayers = 6},null);

    public override void OnJoinedRoom()
    {
        photonView.RPC("SetSlot",RpcTarget.MasterClient,"test",PhotonNetwork.IsMasterClient);
    }

    [PunRPC]
    void SetSlot(string name, bool is_master)
    {
        var slots = GameObject.Find("CharacterSlots").GetComponent<CharacterSlots>();
        var info = GameObject.Find("Myroominfo").GetComponent<MyInRoomInfo>();
        for (var i = 0; i < 6; ++i)
        {
            var target = slots.slots[i].GetComponent<Slot>();
            if (target.Nickname == "")
            {
                target.Nickname = "test" + i;
                break;
            }
            
        }
    }
    
}
