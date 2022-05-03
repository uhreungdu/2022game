using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public struct GameInfomation
{
    public int[] gamescore;
}

public class GameInfo : MonoBehaviour
{
    public GameInfomation Infomations;
    
    private static GameInfo _instance;
    
    public static GameInfo GetInstance()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<GameInfo>();
            if (_instance == null)
            {
                GameObject container = new GameObject("GameInfo");
                _instance = container.AddComponent<GameInfo>();
            }
        }

        return _instance;
    }

    void Awake()
    {
        var obj = FindObjectsOfType<GameInfo>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
            Infomations.gamescore = new int[2];
        }                       
        else
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        
    }
}
