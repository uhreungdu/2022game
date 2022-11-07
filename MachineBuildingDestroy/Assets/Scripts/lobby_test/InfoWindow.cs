using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InfoWindow : MonoBehaviour
{
    public GameObject playerName;
    public GameObject level;
    public int costume;
    //public GameObject playerModel;
    public GameObject gameResults;
    private GameObject _account;
    private Socket _socket;

    private void Awake()
    {
        _account = GameObject.Find("Account");
        _socket = LoginDBConnection.GetInstance().GetClientSocket();
        //SendPlayerInfoRequest();
        NewGetPlayerInfo();
    }

    private void NewGetPlayerInfo()
    {
        var data = _account.GetComponent<Account>();
        playerName.GetComponent<Text>().text = data.GetPlayerNickname();
        var win = data.GetPlayerWin();
        var lose = data.GetPlayerLose();
        gameResults.GetComponent<Text>().text = "승리: " + win + " "
                                                +"패배: " + lose;
        costume = data.GetPlayerCostume();
        //playerModel.GetComponent<PrintPlayerModel>().RenewPlayerModel(costume);
    }

    public void SetCostumeOnDB(int num)
    {
        StartCoroutine(CorSetCostumeOnDB(num));
    }

    private void SendPlayerInfoRequest()
    {
        byte[] sendBuf = new byte[1];
        sendBuf[0] = (byte) LoginDBConnection.DBPacketType.AccountInfoRequest;

        _socket.Send(sendBuf);
    }
    
    public IEnumerator CorSetCostumeOnDB(int num)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", "\"" + _account.GetComponent<Account>().GetPlayerID() + "\"");
        form.AddField("costume", num);

        UnityWebRequest www =
            UnityWebRequest.Post(
                "http://" + PhotonNetwork.PhotonServerSettings.AppSettings.Server + "/lobby/set_player_costume.php",
                form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
    }
    
    string GetStringDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|")) value = value.Remove(value.IndexOf("|"));
        return value;
    }

    int GetIntDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|")) value = value.Remove(value.IndexOf("|"));
        return int.Parse(value);
    }
    
}
