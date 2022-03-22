using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Slot num 0~2 is team1, 3~5 is team2
public class ChangeTeamButton : MonoBehaviour
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
        
        switch (Info.GetComponent<MyInRoomInfo>().MySlotNum)
        {
            case 0:
            case 1:
            case 2:
                for (int i = 3; i < 6; ++i)
                {
                    var target = slots.slots[i].GetComponent<Slot>();
                    if (target.Nickname == "")
                    {
                        target.Nickname = myslot.Nickname;
                        myslot.Nickname = "";
                        Info.GetComponent<MyInRoomInfo>().MySlotNum = i;
                        return;
                    }
                }        
                break;
            case 3:
            case 4:
            case 5:
                for (int i = 0; i < 2; ++i)
                {
                    var target = slots.slots[i].GetComponent<Slot>();
                    if (target.Nickname == "")
                    {
                        target.Nickname = myslot.Nickname;
                        myslot.Nickname = "";
                        Info.GetComponent<MyInRoomInfo>().MySlotNum = i;
                        return;
                    }
                }  
                break;
        }
    }
}
