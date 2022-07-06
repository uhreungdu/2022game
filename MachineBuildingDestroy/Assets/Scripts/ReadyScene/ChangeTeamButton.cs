using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

// Slot num 0~2 is team1, 3~5 is team2
public class ChangeTeamButton : MonoBehaviourPun
{
    private GameObject _info;
    
    // Start is called before the first frame update
    void Start()
    {
        _info = GameObject.Find("Myroominfo");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.GetComponent<Button>().interactable = !_info.GetComponent<MyInRoomInfo>().isReady;
    }

    public void OnClick()
    {
        int mynum = _info.GetComponent<MyInRoomInfo>().mySlotNum;
        var slots = GameObject.Find("CharacterSlots").GetComponent<CharacterSlots>();
        var myslot = slots.slots[mynum].GetComponent<Slot>();
        int maxPlayer = PhotonNetwork.CurrentRoom.MaxPlayers;
        
        int mySlotNum = _info.GetComponent<MyInRoomInfo>().mySlotNum;
        if (mySlotNum % 2 == 0)
        {
            for (int i = 1; i < maxPlayer; i+=2)
            {
                var target = slots.slots[i].GetComponent<Slot>();
                if (target.Nickname == "")
                {
                    photonView.RPC("SetSlotByNum",RpcTarget.MasterClient, 
                        myslot.Nickname, mySlotNum, i, Application.platform);
                    return;
                }
            }    
        }
        else
        {
            for (int i = 0; i < maxPlayer; i+=2)
            {
                var target = slots.slots[i].GetComponent<Slot>();
                if (target.Nickname == "")
                {
                    photonView.RPC("SetSlotByNum",RpcTarget.MasterClient, 
                        myslot.Nickname, mySlotNum, i, Application.platform);
                    return;
                }
            }    
        }
        
    }
    
    [PunRPC]
    void SetSlotByNum(string nickname, int from, int to, RuntimePlatform platform)
    {
        var slots = GameObject.Find("CharacterSlots").GetComponent<CharacterSlots>();
        var target = slots.slots[to].GetComponent<Slot>();
        if (target.Nickname == "")
        {
            target.Nickname = nickname;
            target.Platform = platform == RuntimePlatform.Android ? 1 : 0;
            slots.slots[from].GetComponent<Slot>().Nickname = "";
            slots.slots[from].GetComponent<Slot>().Platform = 2;
            Debug.Log(target.Platform);
        }
    }
}
