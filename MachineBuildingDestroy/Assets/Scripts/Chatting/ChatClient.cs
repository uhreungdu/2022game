using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine.UI;

public class ChatClient : MonoBehaviour
{
    private const string ServerAddress = "127.0.0.1";
    private const int Port = 9888;
    private const int BufSize = 128;
    public byte[] sendbuf = new byte[BufSize-1];
    public byte[] recvbuf = new byte[BufSize];
    private Socket _client;
    private IPEndPoint _ipep;

    enum ChatCode : byte
    {
        Normal,
        Exit
    }

    private void Awake()
    {
        _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _ipep = new IPEndPoint(IPAddress.Parse(ServerAddress), Port);
    }

    public void ConnectToChatServer()
    {
        _client.Connect(_ipep);
        _client.BeginReceive(recvbuf, 0, BufSize, 0,
            ReceiveCallback, _client);
    }

    public void DisconnectFromChatServer()
    {
        sendbuf[0] = (byte)ChatCode.Exit;
        _client.Send(sendbuf);
    }

    public void SendChat(GameObject obj)
    {
        // InputField에서 텍스트 받아오기
        var inputfield = obj.GetComponent<InputField>();
        string msg = inputfield.text;
        
        // 메세지 코드를 temp에 저장 
        byte[] tempbuf = new byte[BufSize];
        tempbuf[0] = (byte) ChatCode.Normal;
        
        // send버퍼 초기화 후 메세지 받아오기
        sendbuf.Initialize();
        sendbuf = Encoding.UTF8.GetBytes(msg);
        
        // 메세지를 temp뒤에 병합
        Array.Copy(sendbuf, 0, tempbuf, 1, sendbuf.Length);
        
        // 메세지 전송 후 InputField비우기
        _client.Send(tempbuf);
        inputfield.text = string.Empty;
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        string data = string.Empty;
        Socket socket = (Socket) ar.AsyncState;

        try
        {
            int recvsize = socket.EndReceive(ar);

            if (recvsize > 0)
            {
                data = Encoding.UTF8.GetString(recvbuf, 0, recvsize);
                print(data);
                _client.BeginReceive(recvbuf, 0, BufSize, 0,
                    ReceiveCallback, _client);
            }
        }
        catch (Exception ex)
        {
            print("Disconnected");
        }
    }

    private void OnApplicationQuit()
    {
        sendbuf[0] = (byte)ChatCode.Exit;
        _client.Send(sendbuf);
    }
}
