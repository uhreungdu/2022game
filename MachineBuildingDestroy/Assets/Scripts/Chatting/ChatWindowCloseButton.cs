using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatWindowCloseButton : MonoBehaviour
{
    public GameObject ChatWindow;
    public GameObject ChatOpenButton;
    public GameObject ChatModule;

    public void Onclick()
    {
        ChatWindow.SetActive(false);
        ChatOpenButton.GetComponent<Button>().interactable = true;
        ChatModule.GetComponent<ChatClient>().DisconnectFromChatServer();
    }
}
