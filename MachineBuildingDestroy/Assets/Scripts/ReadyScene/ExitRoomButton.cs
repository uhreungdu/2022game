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
    }

    private void ExitRoom()
    {
        byte[] sendBuf = new byte[1];
        sendBuf[0] = (byte) LoginDBConnection.DBPacketType.ExitRoom;

        LoginDBConnection.GetInstance().GetClientSocket().Send(sendBuf);
    }
}
