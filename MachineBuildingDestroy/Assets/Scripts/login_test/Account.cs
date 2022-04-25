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

    public void WriteAccount(string id, string nickname)
    {
        var m_id = id.Remove(id.Length - 1, 1);
        m_id = m_id.Remove(0, 1);
        pID = m_id;
        pNickname = nickname;
        PhotonNetwork.NickName = nickname;
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public string GetPlayerID()
    {
        return pID;
    }

    public string GetPlayerNickname()
    {
        return pNickname;
    }
}
