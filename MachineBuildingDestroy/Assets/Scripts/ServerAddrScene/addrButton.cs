using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class addrButton : MonoBehaviour
{
    public InputField[] ipField;


    // Start is called before the first frame update
    void Start()
    {
        print(PhotonNetwork.PhotonServerSettings.AppSettings.Server);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OK_Button()
    {
        string address = "";
        foreach (var v in ipField)
        {
            if (255 < Int32.Parse(v.text)) 
                v.text = "255";
            else if (0 > Int32.Parse(v.text))
                v.text = "0";
        }
        address += Int32.Parse(ipField[0].text).ToString();
        address += ".";
        address += Int32.Parse(ipField[1].text).ToString();
        address += ".";
        address += Int32.Parse(ipField[2].text).ToString();
        address += ".";
        address += Int32.Parse(ipField[3].text).ToString();

        PhotonNetwork.PhotonServerSettings.AppSettings.Server = address;
        //print(PhotonNetwork.PhotonServerSettings.AppSettings.Server);
        SceneManager.LoadScene("login_test");
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
