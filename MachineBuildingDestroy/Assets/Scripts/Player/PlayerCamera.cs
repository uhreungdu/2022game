using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using DG.Tweening;

public class PlayerCamera : MonoBehaviour
{
    public Camera _Camera;
    public LayerMask _fieldLayer;
    // Start is called before the first frame update
    void Start()
    {
        
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
        var maxDistance = 10f;
        // 광선 디버그 용도
        Debug.DrawRay(_Camera.transform.position, _Camera.transform.forward * maxDistance, Color.blue);
        RaycastHit _raycastHit;
        Physics.Raycast(ray, out _raycastHit, maxDistance, _fieldLayer);
        if (_raycastHit.transform != null)
        {
            if (_raycastHit.transform.tag == "Wall")
            {
                MeshRenderer hitMeshRenderer = _raycastHit.transform.GetComponent<MeshRenderer>();
                if (hitMeshRenderer)
                {
                    hitMeshRenderer.enabled = false;
                    // Material[] materials = hitMeshRenderer.sharedMaterials;
                    // foreach (var material in materials)
                    // {
                    //     // material.DOFade(0, 0.1f);
                    // }
                }
            }
        }
    }
}
