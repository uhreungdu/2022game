using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using DG.Tweening;
using ExitGames.Client.Photon.StructWrapping;

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
        HindBuilding();
    }

    void HindBuilding()
    {
        if (_Building != null)
        {
            Material material = _Building.GetComponent<Renderer>().material;
            if (material)
            {
                float Opacity;
                    Opacity = material.GetFloat("_Opacity");

                if (Opacity >= 0.3)
                {
                    Opacity -= 2f * Time.deltaTime;
                    material.SetFloat("_Opacity", Opacity);
                }
            }
        }
    }
    
    void CameraForwardRaycast()
    {
        var ray = new Ray(_Camera.transform.position, _Camera.transform.forward);
        var maxDistance = 10f;
        // 광선 디버그 용도
        Debug.DrawRay(_Camera.transform.position, _Camera.transform.forward * maxDistance, Color.blue);
        RaycastHit _raycastHit;
        Physics.Raycast(ray, out _raycastHit, maxDistance, _fieldLayer);
        if (_raycastHit.transform != null && !Colliding)
        {
            if (_raycastHit.transform.tag == "Wall" && _raycastHit.transform.gameObject != _Building)
            {
                // MeshRenderer hitMeshRenderer = _raycastHit.transform.GetComponent<MeshRenderer>();
                // if (hitMeshRenderer)
                // {
                //     hitMeshRenderer.enabled = false;
                // }
                _Building = _raycastHit.transform.gameObject;
            }
        }
        else
        {
            if (_Building != null && !Colliding)
            {
                Material material = _Building.GetComponent<Renderer>().material;
                // MeshRenderer beforeCollisionBuilding = _Building.transform.GetComponent<MeshRenderer>();
                // beforeCollisionBuilding.enabled = true;
                material.SetFloat("_Opacity", 1f);
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
                // MeshRenderer hitMeshRenderer = other.transform.GetComponent<MeshRenderer>();
                // if (hitMeshRenderer)
                // {
                //     
                //     hitMeshRenderer.enabled = false;
                // }
                _Building = other.transform.gameObject;
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (Colliding && other.tag == "Wall")
            Colliding = false;
    }
}
