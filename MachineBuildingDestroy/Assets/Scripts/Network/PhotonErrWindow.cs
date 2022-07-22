using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonErrWindow : MonoBehaviour
{
    private void Start()
    {
        NetworkManager.GetInstance().photonErrWindow = this.gameObject;
        gameObject.SetActive(false);
    }
}
