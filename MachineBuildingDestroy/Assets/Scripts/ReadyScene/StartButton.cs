using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviourPun
{
    public GameObject Info;

    private GameObject _NetworkManager;
    
    // Start is called before the first frame update
    void Start()
    {
        _NetworkManager = GameObject.Find("NetworkManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        var info = Info.GetComponent<MyInRoomInfo>(); 
        if (info.isMaster)
        {
            transform.GetChild(0).GetComponent<Text>().text = "START";
        }
        else
        {
            transform.GetChild(0).GetComponent<Text>().text = info.isReady ? "READY CANCEL" : "READY";
        }
    }

    public void OnClick()
    {
        var info = Info.GetComponent<MyInRoomInfo>();
        if (info.isMaster)
        {
            var slots = GameObject.Find("CharacterSlots").GetComponent<CharacterSlots>();
            for (int i = 0; i < 6; ++i)
            {
                if(i==info.mySlotNum) continue;
                
                var target = slots.slots[i].GetComponent<Slot>();
                if (target.Nickname != "" && !target.IsReady)
                {
                    print("NOT READY ALL PLAYER");
                    return;
                }
            }
            _NetworkManager.GetComponent<NetworkManager>().StartGameEvent();
            PhotonNetwork.LoadLevel("SampleScene");
        }
        else
        {
            if (info.isReady)
            {
                info.isReady = false;
                photonView.RPC("ReadyPlayerSlot", RpcTarget.MasterClient, info.mySlotNum, false);
            }
            else
            {
                info.isReady = true;
                photonView.RPC("ReadyPlayerSlot", RpcTarget.MasterClient, info.mySlotNum, true);
            }
        }
    }

    [PunRPC]
    void ReadyPlayerSlot(int num, bool val)
    {
        print("ready?");
        GameObject.Find("CharacterSlots").GetComponent<CharacterSlots>()
            .slots[num].GetComponent<Slot>()
            .IsReady = val;
    }
}
