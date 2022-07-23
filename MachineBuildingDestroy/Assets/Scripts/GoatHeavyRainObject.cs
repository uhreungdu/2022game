using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GoatHeavyRainObject : MonoBehaviourPun
{
    public GoatHeavyRain.GoatHeavyRainInfo _goatHeavyRainInfo;
    private float speed = 20f;
    public GameObject Dangerzone;

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return; 
        transform.position += _goatHeavyRainInfo.Path * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        Collider[] colliders = Physics.OverlapSphere(transform.position + (transform.forward * 1.4f), 30f);
        foreach (var collider in colliders)
        {
            if (collider.tag == "Wall")
            {
                BulidingObject bulidingObject = collider.GetComponent<BulidingObject>();
                if (bulidingObject != null && !bulidingObject.dead)
                    bulidingObject.NetworkOnDamage(80);
            }
            else if (collider.tag == "Player")
            {
                PlayerState playerState = collider.GetComponent<PlayerState>();
                if (playerState != null && !playerState.dead)
                    playerState.NetworkOnDamage(80);
            }
        }
        PhotonNetwork.Destroy(Dangerzone);
        PhotonNetwork.Destroy(gameObject);
    }
}