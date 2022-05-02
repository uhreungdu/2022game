using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using DG.Tweening;

public class PlayerCamera : MonoBehaviour
{
    public Camera _Camera;
    public LayerMask _fieldLayer;
    public GameObject _Building;

    private bool Colliding = false;
    // Start is called before the first frame update
    void Start()
    {
        _Building = null;
    }

    // Update is called once per frame
    void Update()
    {
        CameraForwardRaycast();
    }

    void HindBuilding()
    {
        
    }
    
    void CameraForwardRaycast()
    {
        var ray = new Ray(_Camera.transform.position, _Camera.transform.forward);
        var maxDistance = 20f;
        // 광선 디버그 용도
        Debug.DrawRay(_Camera.transform.position, _Camera.transform.forward * maxDistance, Color.blue);
        RaycastHit _raycastHit;
        Physics.Raycast(ray, out _raycastHit, maxDistance, _fieldLayer);
        if (_raycastHit.transform != null && !Colliding)
        {
            if (_raycastHit.transform.tag == "Wall" && _raycastHit.transform.gameObject != _Building)
            {
                MeshRenderer hitMeshRenderer = _raycastHit.transform.GetComponent<MeshRenderer>();
                if (hitMeshRenderer)
                {
                    _Building = _raycastHit.transform.gameObject;
                    hitMeshRenderer.enabled = false;
                }
            }
        }
        else
        {
            if (_Building != null && !Colliding)
            {
                MeshRenderer beforeCollisionBuilding = _Building.transform.GetComponent<MeshRenderer>();
                beforeCollisionBuilding.enabled = true;
                _Building = null;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform != null)
        {
            if (other.transform.tag == "Wall" && other.transform.gameObject != _Building)
            {
                Colliding = true;
                MeshRenderer hitMeshRenderer = other.transform.GetComponent<MeshRenderer>();
                if (hitMeshRenderer)
                {
                    _Building = other.transform.gameObject;
                    hitMeshRenderer.enabled = false;
                }
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (Colliding && other.tag == "Wall")
            Colliding = false;
    }
}
