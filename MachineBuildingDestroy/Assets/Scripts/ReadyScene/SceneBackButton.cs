using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBackButton : MonoBehaviour
{
    public void OnClick()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("lobby_test");
    }
}
