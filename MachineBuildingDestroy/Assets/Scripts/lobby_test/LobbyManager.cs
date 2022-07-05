using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Networking;
using ExitGames.Client.Photon;

public class LobbyManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static LobbyManager instance;
    public GameObject roomlist;
    private Socket _socket;

    public static LobbyManager GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<LobbyManager>();
            if (instance == null)
            {
                GameObject container = new GameObject("LobbyManager");
                instance = container.AddComponent<LobbyManager>();
            }
        }
        return instance;
    }

    void Awake()
    {
        _socket = LoginDBConnection.GetInstance().GetClientSocket();
        GameObject roominfo;
        try
        {
            roominfo = GameObject.Find("Myroominfo");
            if (roominfo != null)
            {
                Destroy(roominfo);
            }
        }
        catch (Exception ex)
        {

        }
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void GetRoomList()
    {
        byte[] sendBuf = new byte[1];
        sendBuf[0] = (byte) LoginDBConnection.DBPacketType.RoomListRequest;

        _socket.Send(sendBuf);
    }
}
