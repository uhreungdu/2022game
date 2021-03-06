using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Goalpost : MonoBehaviourPun
{
    private GameManager _gameManager;
    private MeshRenderer _meshRenderer;
    private Rigidbody _rigidbody;
    private Collider[] _Colliders;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.GetInstance();
        _meshRenderer = GetComponent<MeshRenderer>();
        _Colliders = GetComponents<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        OnEvent();
    }

    private void OnEvent()
    {
        // CAUTION:// CAUTION:// CAUTION:// CAUTION:// CAUTION:
        // CAUTION:// CAUTION:// CAUTION:// CAUTION:// CAUTION:
        // CAUTION:// CAUTION:// CAUTION:// CAUTION:// CAUTION:
        // CAUTION:// CAUTION:// CAUTION:// CAUTION:// CAUTION:
        // CAUTION:// CAUTION:// CAUTION:// CAUTION:// CAUTION:
        // CAUTION:// CAUTION:// CAUTION:// CAUTION:// CAUTION:
        // CAUTION:// CAUTION:// CAUTION:// CAUTION:// CAUTION:
        // CAUTION:// CAUTION:// CAUTION:// CAUTION:// CAUTION:
        // CAUTION:// CAUTION:// CAUTION:// CAUTION:// CAUTION:
        // CAUTION:// CAUTION:// CAUTION:// CAUTION:// CAUTION:
        // if (!PhotonNetwork.IsMasterClient) return;
        if (_gameManager.EManager.goalpost_Create)
        {
            if (_meshRenderer.enabled == false)
            {
                transform.position += new Vector3(0, 30, 0);
                _meshRenderer.enabled = true;
                _rigidbody.useGravity = true;
            }

            foreach (var colliders in _Colliders)
            {
                if (colliders.enabled == false)
                    colliders.enabled = true;
            }
        }
        else
        {
            if (_meshRenderer.enabled)
            {
                _meshRenderer.enabled = false;
                _rigidbody.useGravity = false;
            }
            foreach (var colliders in _Colliders)
            {
                if (colliders.enabled)
                    colliders.enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

    }
}
