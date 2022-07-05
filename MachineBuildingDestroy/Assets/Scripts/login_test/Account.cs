using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Networking;

public class Account : MonoBehaviour
{
    private static Account instance;
    [SerializeField] private string pID;
    [SerializeField] private string pNickname;
    private int _win;
    private int _lose;
    private int _costume;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    public static Account GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<Account>();
            if (instance == null)
            {
                GameObject container = new GameObject("Account");
                instance = container.AddComponent<Account>();
            }
        }
        return instance;
    }
    
    void Awake()
    {
        var obj = FindObjectsOfType<Account>(); 
        if (obj.Length == 1) 
        { 
            DontDestroyOnLoad(gameObject); 
        } 
        else 
        { 
            Destroy(gameObject); 
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void WriteAccount(string id, string nickname, int win, int lose, int costume)
    {
        pID = id;
        pNickname = nickname;
        _win = win;
        _lose = lose;
        _costume = costume;
        PhotonNetwork.NickName = nickname;
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    
    public void RefreshAccount(int win, int lose)
    {
        _win = win;
        _lose = lose;
    }

    public string GetPlayerID()
    {
        return pID;
    }

    public string GetPlayerNickname()
    {
        return pNickname;
    }

    public int GetPlayerWin()
    {
        return _win;
    }
    
    public int GetPlayerLose()
    {
        return _lose;
    }
    
    public int GetPlayerCostume()
    {
        return _costume;
    }
}
