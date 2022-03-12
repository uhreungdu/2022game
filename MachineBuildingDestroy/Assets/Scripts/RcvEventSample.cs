using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class RcvEventSample : MonoBehaviourPun
{
    [SerializeField]
    private GameManager gManager;
    // Start is called before the first frame update
    void Start()
    {
        gManager = GameManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnEvent(EventData Evdata)
    {
        byte eventCode = Evdata.Code;
        //Debug.Log("EVENTCALL");
        if (eventCode == 0)
        {
            object[] data = (object[])Evdata.CustomData;
            for (int i = 0; i < data.Length; i++)
                Debug.Log(data[i]);
        }
        else if(eventCode == 1)
        {
            object[] data = (object[])Evdata.CustomData;
            for(int i = 0; i < 2; ++i)
            {
                gManager.setScore(i, (int)data[i]);
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
