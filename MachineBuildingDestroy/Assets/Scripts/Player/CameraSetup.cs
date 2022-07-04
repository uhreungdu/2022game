using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using DG.Tweening;

public class CameraSetup : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            CinemachineFreeLook cam = FindObjectOfType<CinemachineFreeLook>();
            
            cam.Follow = transform;
            cam.LookAt = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
