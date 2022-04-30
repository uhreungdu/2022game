using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ChatSendButton : MonoBehaviour
{
    private GameObject _chatClient;
    private Socket _client;
    private byte[] sendbuf;
    
    public GameObject inputfield;
    
    // Start is called before the first frame update

    private void Awake()
    {
        _chatClient = GameObject.Find("ChatClient");
        _client = _chatClient.GetComponent<ChatClient>().GetClientSocket();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SendChat()
    {
        // InputField에서 텍스트 받아오기
        var msg = inputfield.GetComponent<InputField>().text;

        // send버퍼 초기화 후 메세지 받아오기
        sendbuf = Encoding.UTF8.GetBytes(msg);
        
        // 메세지 코드를 temp에 저장 
        byte[] tempbuf = new byte[sendbuf.Length + 1];
        tempbuf[0] = (byte) ChatClient.ChatCode.Normal;

        // 메세지를 temp뒤에 병합
        Array.Copy(sendbuf, 0, tempbuf, 1, sendbuf.Length);
        
        // 메세지 전송 후 InputField비우기
        _client.Send(tempbuf);
        inputfield.GetComponent<InputField>().text = string.Empty;
    }
}
