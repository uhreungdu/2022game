using System.Collections;
using System.Collections.Generic;
using System.Text;
using Photon.Pun;
using UnityEngine;

public class LandMarkObject : BulidingObject
{
    private GameManager _gameManager;
    private MeshRenderer _meshRenderer;
    private Collider[] _Colliders;
    private Rigidbody _rigidbody;
    private float appearY = -30;
    private Vector3 originTransform;

    private bool appear = false;
    private bool appearTransform = false;
    void Start()
    {
        base.Start();
        point = 50;
        _gameManager = GameManager.GetInstance();
        _meshRenderer = GetComponent<MeshRenderer>();
        _Colliders = GetComponents<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
        originTransform = transform.position;
    }
    void Update()
    {
        base.Update();
        if (_gameManager == null)
        {
            
        }
        else if (!_gameManager.EManager.gameSet) 
        {
            if (!dead)
            {
                OnEvent();
                AppearLandmark();
            }
        }
    }
    
    private void OnEvent()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (_gameManager.EManager.landmakr_Create)
        {
            if (appear == false)
            {
                appear = true;
                appearTransform = false;
                Vector3 position = originTransform;
                position.y += appearY;
                transform.position = position;
                _meshRenderer.enabled = true;
                _rigidbody.useGravity = false;
                foreach (var colliders in _Colliders)
                {
                    if (colliders.enabled == false)
                        colliders.enabled = true;
                }
            }
        }
        else
        {
            appear = false;
            if (_meshRenderer.enabled)
                _meshRenderer.enabled = false;
            foreach (var colliders in _Colliders)
            {
                if (colliders.enabled)
                    colliders.enabled = false;
            }
        }
    }
    void AppearLandmark()
    {
        if (_gameManager.EManager.landmakr_Create)
        {
            if (transform.position.y < originTransform.y && appear && !appearTransform)
            {
                Vector3 position = originTransform;
                appearY += Time.deltaTime * (30 / 3.0f);
                position.y += appearY;
                transform.position = position;
            }
            if (transform.position.y >= originTransform.y)
            {
                appearTransform = true;
                if (_rigidbody.useGravity == false)
                {
                    _rigidbody.useGravity = true;
                }
            }
        }
    }
    
}