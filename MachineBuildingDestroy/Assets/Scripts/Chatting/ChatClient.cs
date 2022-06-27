using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Photon.Pun;
using UnityEngine.UI;

public class ChatClient : MonoBehaviour
{
    private static ChatClient instance;
    
    private const string ServerAddress = "121.139.87.70";
    private const int Port = 15000;
    private const int BufSize = 256;
    private Socket _client;
    private IPEndPoint _ipep;
    private bool _isDataSend = false;

    private int _recvCursor = 0;
    private int _readCursor = -1;
    
    private byte[] sendbuf = new byte[BufSize-1];
    private byte[] recvbuf = new byte[BufSize];
    public Chatlog chatLog;
    
    public enum ChatCode : byte
    {
        Normal,
        Exit,
        Login
    }

    private void Awake()
    {
        var obj = FindObjectsOfType<NetworkManager>();
        _ipep = new IPEndPoint(IPAddress.Parse(ServerAddress), Port);
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ConnectToChatServer();
    }

    private void Update()
    {
        if (!_isDataSend) return;
        if (recvbuf[0] > _recvCursor) return;
        int loopnum = 0;
        while (_readCursor < _recvCursor)
        {
            loopnum++;
            if (loopnum > 10000)
            {
                _isDataSend = false;
                return;
            }

            switch (recvbuf[1])
            {
                case (byte) ChatCode.Normal:
                    chatLog.AddLine(recvbuf);
                    break;
            }

            _readCursor = recvbuf[0];
            Array.Copy(recvbuf, _readCursor, recvbuf, 0, BufSize - _readCursor);
            _recvCursor = _recvCursor - _readCursor;
            _readCursor = 0;
        }
        _isDataSend = false;
    }

    public Socket GetClientSocket()
    {
        return _client;
    }

    async void ConnectToChatServer()
    {
        _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        await Task.Run(() =>
        {
            try
            {
                _client.Connect(_ipep);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
        });
        StartReceive();
        SendLoginInfo();
    }

    private void StartReceive()
    {
        _client.BeginReceive(recvbuf, 0, BufSize, 0,
            ReceiveCallback, _client);
    }

    private void DisconnectFromChatServer()
    {
        if (!_client.Connected) return;
        sendbuf[0] = (byte)ChatCode.Exit;
        _client.Send(sendbuf);
        _client.Close();
    }
    
    private void SendLoginInfo()
    {
        try
        {
            byte[] name = Encoding.UTF8.GetBytes(PhotonNetwork.NickName);
            byte[] sendData = new byte[2 + name.Length];
            
            sendData[0] = (byte) ChatCode.Login;
            sendData[1] = (byte)name.Length;
            Array.Copy(name, 0, sendData, 2, name.Length);
            
            _client.Send(sendData);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        Socket socket = (Socket) ar.AsyncState;
        try
        {
            int recvsize = socket.EndReceive(ar);

            if (recvsize > 0)
            {
                Debug.Log(recvsize);
                _isDataSend = true;
                _client.BeginReceive(recvbuf, _recvCursor, BufSize-_recvCursor, 0,
                    ReceiveCallback, _client);
                _recvCursor = recvsize;
            }
        }
        catch (Exception ex)
        {
            print("Disconnected");
            DisconnectFromChatServer();
        }
    }

    private void OnDestroy()
    {
        if (_client == null) return;
        if (!_client.Connected) return;
        sendbuf[0] = (byte) ChatCode.Exit;
        _client.Send(sendbuf);
    }
}
