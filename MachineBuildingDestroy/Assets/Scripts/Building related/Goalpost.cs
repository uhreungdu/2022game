using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Goalpost : MonoBehaviourPun
{
    private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEvent()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (_gameManager.EManager.goalpost_Create)
        {
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

    }
}
