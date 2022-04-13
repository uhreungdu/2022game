using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class ChatClient : MonoBehaviour
{
    private const string ServerAddress = "127.0.0.1";
    private const int Port = 9888;
    private const int BufSize = 128;
    private Socket _client;
    private IPEndPoint _ipep;

    private void Awake()
    {
        _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _ipep = new IPEndPoint(IPAddress.Parse(ServerAddress), Port);
    }

    private void Start()
    {

            _client.Connect(_ipep);


    }

    public void SendChat(string msg)
    {
        _client.Send(Encoding.UTF8.GetBytes(msg));
    }

    private void OnApplicationQuit()
    {
        _client.Close();
    }
}
