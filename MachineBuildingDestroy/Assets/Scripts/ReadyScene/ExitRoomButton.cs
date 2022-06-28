using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitRoomButton : MonoBehaviour
{
    public void OnClick()
    {
        PhotonNetwork.LeaveRoom();
        ExitRoom();
        SceneManager.LoadScene("lobby_test");
    }

    private void ExitRoom()
    {
        byte[] sendBuf = new byte[1];
        sendBuf[0] = (byte) LoginDBConnection.ChatCode.ExitRoom;

        LoginDBConnection.GetInstance().GetClientSocket().Send(sendBuf);
    }
}
