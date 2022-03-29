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
        
        var num = -1;
        // 자신 슬롯 번호 갱신
        for (var i = 0; i < 6; ++i)
        {
            var slotComponent = slots[i].GetComponent<Slot>();
            if (slotComponent.Nickname != PhotonNetwork.NickName) continue;
            info.GetComponent<MyInRoomInfo>().MySlotNum = i;
            num = i;
            break;
        }

        // Master 넘기는 버튼 활성화 갱신
        if (PhotonNetwork.IsMasterClient)
        {
            for (var i = 0; i < 6; ++i)
            {
                var slotComponent = slots[i].GetComponent<Slot>();
                if (slotComponent.Nickname != "")
                    slotComponent.masterButton.GetComponent<Button>().interactable = i != num;
                else slotComponent.masterButton.GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            for (var i = 0; i < 6; ++i)
            {
                slots[i].GetComponent<Slot>().masterButton.GetComponent<Button>().interactable = false;
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
