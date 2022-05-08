using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public struct PlayersInfomation
{
    public string Name;
    public int Costume;
    public int SlotNum;
    public int Platform;
    
    public int TotalGetPoint;
    public float TotalCauseDamage;
    public int TotalDeath;
    public int TotalKill;
}

public class MyInRoomInfo : MonoBehaviour
{
    public PlayersInfomation[] Infomations;
    public string MapName;
    
    private static MyInRoomInfo _instance;
    private int _mySlotNum = 0;
    private bool _isMaster = false;
    private bool _isReady = false;

    public static MyInRoomInfo GetInstance()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<MyInRoomInfo>();
            if (_instance == null)
            {
                GameObject container = new GameObject("Myroominfo");
                _instance = container.AddComponent<MyInRoomInfo>();
            }
        }

        return _instance;
    }

    void Awake()
    {
        var obj = FindObjectsOfType<MyInRoomInfo>();
        Infomations = new PlayersInfomation[6];
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        isMaster = PhotonNetwork.IsMasterClient;
    }

    public void RenewInfo(int index, Slot slot)
    {
        Infomations[index].Name = slot.Nickname;
        Infomations[index].Costume = 0;
        Infomations[index].SlotNum = index;
        Infomations[index].Platform = slot.Platform;
    }

    public int mySlotNum
    {
        set => _mySlotNum = value;
        get => _mySlotNum;
    }
    
    public bool isMaster
    {
        get => _isMaster;
        set => _isMaster = value;
    }
    
    public bool isReady
    {
        get => _isReady;
        set => _isReady = value;
    }
}
