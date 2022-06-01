using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Random = UnityEngine.Random;

public class CreateRoomButton : MonoBehaviour
{
    public GameObject DarkBackground;
    public GameObject Window;

    public void OnClick()
    {
        DarkBackground.SetActive(true);
        Window.SetActive(true);
    }
    
}
