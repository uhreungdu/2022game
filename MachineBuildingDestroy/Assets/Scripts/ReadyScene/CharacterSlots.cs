using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlots : MonoBehaviourPun
{
    private bool init = false;
    public GameObject[] slots = new GameObject[6];
    public GameObject info;
    // Start is called before the first frame update
    void Start()
    {
        info = GameObject.Find("Myroominfo");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(!init){
            photonView.RPC("SetSlotByQuick",RpcTarget.MasterClient,
            PhotonNetwork.NickName);
            init = true;
        }
        
        int num = -1;
        // 자신 슬롯 번호 갱신
        for (int i = 0; i < 6; ++i)
        {
            var SlotComponent = slots[i].GetComponent<Slot>();
            if (SlotComponent.Nickname == PhotonNetwork.NickName)
            {
                info.GetComponent<MyInRoomInfo>().MySlotNum = i;
                num = i;
                break;
            }
        }

        // Master 넘기는 버튼 활성화 갱신
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < 6; ++i)
            {
                var SlotComponent = slots[i].GetComponent<Slot>();
                if (SlotComponent.Nickname != "")
                    SlotComponent.MasterButton.GetComponent<Button>().interactable = i != num;
                else SlotComponent.MasterButton.GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            for (int i = 0; i < 6; ++i)
            {
                slots[i].GetComponent<Slot>().MasterButton.GetComponent<Button>().interactable = false;
            }
        }
    }
    
    [PunRPC]
    void SetSlotByQuick(string nickname)
    {
        var slot = transform.GetComponent<CharacterSlots>();
        for (var i = 0; i < 6; ++i)
        {
            var target = slot.slots[i].GetComponent<Slot>();
            if (target.Nickname != "") continue;
            target.Nickname = nickname;
            target.IsReady = false;
            break;

        }
    }
    
}
