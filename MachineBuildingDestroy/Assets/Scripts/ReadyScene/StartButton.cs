using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviourPun
{
    private GameObject _info;

    private GameObject _NetworkManager;
    private int maxPlayer;
    public MapDropdown _MapDropdown;

    // Start is called before the first frame update
    void Start()
    {
        _NetworkManager = GameObject.Find("NetworkManager");
        _info=GameObject.Find("Myroominfo");
        maxPlayer = PhotonNetwork.CurrentRoom.MaxPlayers;
    }

    private void FixedUpdate()
    {
        var info = _info.GetComponent<MyInRoomInfo>();

        if (info.isMaster)
        {
            transform.GetChild(0).GetComponent<Text>().text = "START";
            GetComponent<Button>().interactable = true;
            var slots = GameObject.Find("CharacterSlots").GetComponent<CharacterSlots>();
            for (int i = 0; i < maxPlayer; ++i)
            {
                if(i==info.mySlotNum) continue;
                
                var target = slots.slots[i].GetComponent<Slot>();
                if (target.Nickname != "" && !target.IsReady)
                {
                    GetComponent<Button>().interactable = false;
                    break;
                }
            }
        }
        else
        {
            transform.GetChild(0).GetComponent<Text>().text = info.isReady ? "READY CANCEL" : "READY";
        }
    }

    public void OnClick()
    {
        var info = _info.GetComponent<MyInRoomInfo>();
        if (info.isMaster)
        {
            info.MapName = _MapDropdown.SelectText();
            _NetworkManager.GetComponent<NetworkManager>().LoadGameEvent();
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
