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

    private void OnApplicationQuit()
    {
        LogoutAccount();
    }

    void LogoutAccount()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", "\""+pID+"\"") ;
        UnityWebRequest www = UnityWebRequest.Post("http://121.139.87.70/login/logout_account.php", form);
        www.SendWebRequest();
    }
    
    public void WriteAccount(string id, string nickname)
    {
        pID = id;
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
