using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CharChangeCellGrid : MonoBehaviour
{
    public GameObject playerInfo;
    public GameObject characterWindow;
    public GameObject[] costumes = new GameObject[6];
    private GameObject _account;

    private int _costumeNum = 0;

    private void Awake()
    {
        _account = GameObject.Find("Account");
    }

    private void OnEnable()
    {
        _costumeNum = playerInfo.GetComponent<InfoWindow>().costume;
        costumes[_costumeNum].GetComponent<Image>().color = Color.yellow;
        
    }

    public void ChangeCostume(int num)
    {
        costumes[_costumeNum].GetComponent<Image>().color = Color.white;
        playerInfo.GetComponent<InfoWindow>().costume = num;
        costumes[num].GetComponent<Image>().color = Color.yellow;
        playerInfo.GetComponent<InfoWindow>().costume = _costumeNum = num;
        characterWindow.GetComponent<PrintPlayerModel>().RenewPlayerModel(num);
    }

    private void OnDisable()
    {
        playerInfo.GetComponent<InfoWindow>().SetCostumeOnDB(_costumeNum);
    }
    
    
}
