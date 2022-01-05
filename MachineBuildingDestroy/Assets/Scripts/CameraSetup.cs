using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class CameraSetup : MonoBehaviourPun, IPunObservable
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(Camera.main.transform);
        }
        else
        {
            // Network player, receive data
            GetComponent<thirdpersonmove>().cam = (Transform)stream.ReceiveNext();
            
        }
    }
}
