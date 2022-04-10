using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class ChatClient : MonoBehaviour
{
    private const string ServerAddress = "127.0.0.1";
    private const int Port = 9888;
    private TcpClient _client;
    
    private void Start()
    {
        try
        {
            _client = new TcpClient(ServerAddress, Port);
        }
        catch (Exception err)
        {
            Debug.Log("Connect ERR");
        }
    }
    
}
