using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

// Slot num 0~2 is team1, 3~5 is team2
public class ChangeTeamButton : MonoBehaviourPun
{
    public GameObject Info;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.GetComponent<Button>().interactable = !Info.GetComponent<MyInRoomInfo>().IsReady;
    }

    public void OnClick()
    {
        int mynum = Info.GetComponent<MyInRoomInfo>().MySlotNum;
        var slots = GameObject.Find("CharacterSlots").GetComponent<CharacterSlots>();
        var myslot = slots.slots[mynum].GetComponent<Slot>();
        
        int mySlotNum = Info.GetComponent<MyInRoomInfo>().MySlotNum;
        if (mySlotNum < 3)
        {
            for (int i = 3; i < 6; ++i)
            {
                var target = slots.slots[i].GetComponent<Slot>();
                if (target.Nickname == "")
                {
                    photonView.RPC("SetSlotByNum",RpcTarget.MasterClient, 
                        myslot.Nickname, mySlotNum, i);
                    return;
                }
            }    
        }
        else
        {
            for (int i = 0; i < 2; ++i)
            {
                var target = slots.slots[i].GetComponent<Slot>();
                if (target.Nickname == "")
                {
                    photonView.RPC("SetSlotByNum",RpcTarget.MasterClient, 
                        myslot.Nickname, mySlotNum, i);
                    return;
                }
            }    
        }
        
    }
    
    [PunRPC]
    void SetSlotByNum(string nickname, int from, int to)
    {
        var slots = GameObject.Find("CharacterSlots").GetComponent<CharacterSlots>();
        var target = slots.slots[to].GetComponent<Slot>();
        if (target.Nickname == "")
        {
            target.Nickname = nickname;
            slots.slots[from].GetComponent<Slot>().Nickname = "";
            return;
        }
    }
}
