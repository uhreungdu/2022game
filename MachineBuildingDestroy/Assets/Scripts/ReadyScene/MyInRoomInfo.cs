using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyInRoomInfo : MonoBehaviour
{
    private int myslotnum = 0;
    private bool _is_master = false;
    public bool _is_ready = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int MySlotNum
    {
        set => myslotnum = value;
        get => myslotnum;
    }
    
    public bool IsMaster
    {
        get => _is_master;
        set => _is_master = value;
    }
    
    public bool IsReady
    {
        get => _is_ready;
        set => _is_ready = value;
    }
}
