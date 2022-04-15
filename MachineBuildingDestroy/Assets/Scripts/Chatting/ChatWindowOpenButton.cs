using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatWindowOpenButton : MonoBehaviour
{
    public GameObject ChatWindow;
    public GameObject ChatModule;

    public void OnClick()
    {
        GetComponent<Button>().interactable = false;
        ChatWindow.SetActive(true);
        ChatModule.GetComponent<ChatClient>().ConnectToChatServer();
    }
}
