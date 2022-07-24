using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ParticleAutoDestroy : MonoBehaviourPun
{
    private ParticleSystem[] ps;

    void Start ()
    {
        ps = GetComponentsInChildren<ParticleSystem>();
    }
 
    void Update ()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        if (ps.Length > 0)
        {
            bool AllAlive = true;
            foreach (var particlem in ps)
            {
                if (!particlem.IsAlive ()) {
                    AllAlive = false;
                }
            }
            if (AllAlive == false)
                PhotonNetwork.Destroy(gameObject);
        } 
    }
}
