using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
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
        var info = Info.GetComponent<MyInRoomInfo>(); 
        if (info.IsMaster)
        {
            transform.GetChild(0).GetComponent<Text>().text = "START";
        }
        else
        {
            transform.GetChild(0).GetComponent<Text>().text = info.IsReady ? "READY CANCEL" : "READY";
        }
    }

    public void OnClick()
    {
        var info = Info.GetComponent<MyInRoomInfo>();
        if (info.IsMaster)
        {
            var slots = GameObject.Find("CharacterSlots").GetComponent<CharacterSlots>();
            for (int i = 0; i < 6; ++i)
            {
                if(i==info.MySlotNum) continue;
                
                var target = slots.slots[i].GetComponent<Slot>();
                if (target.Nickname != "" && !target.IsReady)
                {
                    print("NOT READY ALL PLAYER");
                    return;
                }
            }
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            if (info.IsReady)
            {
                info.IsReady = false;
                GameObject.Find("CharacterSlots").GetComponent<CharacterSlots>()
                    .slots[info.MySlotNum].GetComponent<Slot>()
                    .IsReady = false;
            }
            else
            {
                info.IsReady = true;
                GameObject.Find("CharacterSlots").GetComponent<CharacterSlots>()
                    .slots[info.MySlotNum].GetComponent<Slot>()
                    .IsReady = true;
            }
        }
    }
}
