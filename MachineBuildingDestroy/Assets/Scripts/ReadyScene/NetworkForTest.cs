using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;

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
        for (var i = 0; i < 6; ++i)
        {
            var target = slots.slots[i].GetComponent<Slot>();
            if (target.Nickname == "")
            {
                target.Nickname = "test" + i;
                if (target.IsMaster == false && is_master) target.IsMaster = true;
                break;
            }
            
        }
    }
    
}
