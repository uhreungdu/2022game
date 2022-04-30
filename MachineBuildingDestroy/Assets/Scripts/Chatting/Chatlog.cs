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
        var name = Encoding.UTF8.GetString(data, 3, data[1]);
        var chat = Encoding.UTF8.GetString(data, 3 + data[1], data[2]);
        var printData = name + ": " + chat;
        
        print(printData);
        var obj = Instantiate(textObject, transform);
        obj.GetComponent<Text>().text = printData;
    }
    
    
}
