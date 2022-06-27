using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Chatlog : MonoBehaviour
{
    private GameObject _chatClient;
    public GameObject textObject;
    public GameObject gScrollbar;

    private void Awake()
    {
        _chatClient = GameObject.Find("ChatClient");
        _chatClient.GetComponent<ChatClient>().chatLog = gameObject.GetComponent<Chatlog>();
    }

    public void AddLine(byte[] data)
    {
        var nickname = Encoding.UTF8.GetString(data, 4, data[2]);
        var chat = Encoding.UTF8.GetString(data, 4 + data[2], data[3]);
        var printData = nickname + ": " + chat;
        
        print(printData);
        var obj = Instantiate(textObject, transform);
        obj.GetComponent<Text>().text = printData;
    }

    public void AddLine(string nickname, string msg)
    {
        var printData = nickname + ": " + msg;
        
        print(printData);
        var obj = Instantiate(textObject, transform);
        obj.GetComponent<Text>().text = printData;
    }
}
