using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class RcvEvent : MonoBehaviourPun
{
    enum EventCode : byte
    {
        Test,
        SpawnPlayer,
        StartGame,
        SetTeamOnServer,
        RespawnForReconnect
    }
    
    [SerializeField]
    private GameManager gManager;
    // Start is called before the first frame update
    void Start()
    {
        //gManager = GameManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnEvent(EventData Evdata)
    {
        // 사용자지정 Event가 아니면 return합니다.
        if (Evdata.Code > System.Enum.GetValues(typeof(EventCode)).Length) return;

        ParameterDictionary parameters = Evdata.Parameters;
        object[] data = (object[])Evdata.CustomData;
        switch (Evdata.Code)
        {
            case (byte)EventCode.Test:
                for (int i = 0; i < data.Length; i++)
                    Debug.Log(data[i]);
                break;
            case (byte)EventCode.SpawnPlayer:
                transform.GetComponent<NetworkManager>().SpawnPlayer();
                break;
            case(byte)EventCode.StartGame:
                var info = GameObject.Find("Myroominfo");
                int team = -2;
                if (info != null)
                {
                    team = Convert.ToInt32(info.GetComponent<MyInRoomInfo>().MySlotNum > 2);
                }
                transform.GetComponent<NetworkManager>().SetTeamNumOnServerEvent(PhotonNetwork.NickName, team);
                break;
            case (byte)EventCode.RespawnForReconnect:
                if ((string) parameters[0] != PhotonNetwork.NickName) break;
                StartCoroutine(SpawnPlayerForReconnect((int) parameters[1]));
                //transform.GetComponent<NetworkManager>().SpawnPlayer((int) parameters[1]);
                break;
        }
    }

    IEnumerator SpawnPlayerForReconnect(int team)
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(5f);
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "SampleScene")
            {
                transform.GetComponent<NetworkManager>().SpawnPlayer(team);
                break;
            }
        }
    }
    
    public void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    public void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }
}
