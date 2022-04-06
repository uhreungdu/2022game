using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PotionState : MonoBehaviourPun
{
    public Collider col;
    public Rigidbody rig;
    
    [PunRPC]
    public void Potion_InitializeState()
    {
        col.enabled = false;
        rig.isKinematic = true;
        rig.useGravity = false;
    }

    [PunRPC]
    public void Potion_ThrowState()
    {
        col.enabled = true;
        rig.isKinematic = false;
        rig.useGravity = true;
    }

    public void SetState(string mode)
    {
        if (mode == "init")
        {
            Potion_InitializeState();
            photonView.RPC("Potion_InitializeState",RpcTarget.Others);
        }
        else if (mode == "throw")
        {
            Potion_ThrowState();
            photonView.RPC("Potion_ThrowState",RpcTarget.Others);
        }
    }
}
