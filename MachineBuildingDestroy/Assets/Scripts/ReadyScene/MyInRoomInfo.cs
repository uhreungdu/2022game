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

    public int Point;
    public int TotalGetPoint;
    public float TotalCauseDamage;
    public int TotalDeath;
    public int TotalKill;
}

public class MyInRoomInfo : MonoBehaviourPun
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
                container.AddComponent<PhotonView>();
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
    
    public void NetworkCauseDamageCount(int index, int damage)
    {
        photonView.RPC("CauseDamageCount", RpcTarget.AllViaServer, index, damage);
    }
    public void NetworkKillCount(int index)
    {
        photonView.RPC("KillCount", RpcTarget.AllViaServer, index);
    }
    public void NetworkDeathCount(int index)
    {
        photonView.RPC("DeathCount", RpcTarget.AllViaServer, index);
    }
    public void NetworkSetPointCount(int index, int Point)
    {
        photonView.RPC("SetPointCount", RpcTarget.AllViaServer, index, Point);
    }
    
    [PunRPC]
    public void CauseDamageCount(int index, int damage)
    {
        MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
        myInRoomInfo.Infomations[index].TotalCauseDamage += damage;
        print($"{index} TotalCauseDamage : {myInRoomInfo.Infomations[index].TotalCauseDamage}");
    }
    
    [PunRPC]
    public void KillCount(int index)
    {
        MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
        myInRoomInfo.Infomations[index].TotalKill++;
        print($"{index} TotalKill : {myInRoomInfo.Infomations[index].TotalKill}");
    }
    
    [PunRPC]
    public void DeathCount(int index)
    {
        MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
        myInRoomInfo.Infomations[index].TotalDeath++;
        print($"{index} TotalDeath : {myInRoomInfo.Infomations[index].TotalDeath}");
    }
    
    public void SetPointCount(int index, int Point)
    {
        MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
        myInRoomInfo.Infomations[index].Point = Point;
    }
    
    public void AddPointCount(int index, int Point)
    {
        MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
        myInRoomInfo.Infomations[index].Point += Point;
    }
    
    public void SetTotalGetPointCount(int index, int Point)
    {
        MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
        myInRoomInfo.Infomations[index].TotalGetPoint = Point;
    }
    public void AddTotalGetPointCount(int index, int Point)
    {
        MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
        myInRoomInfo.Infomations[index].TotalGetPoint += Point;
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
