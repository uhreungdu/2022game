using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;

public class PlayerCamera : MonoBehaviourPun
{
    public Camera _Camera;
    public LayerMask _fieldLayer;
    public List<GameObject> _Buildings;
    public List<GameObject> _ExitBuildings;

    private bool Colliding = false;
    // Start is called before the first frame update
    void Start()
    {
        _Buildings = new List<GameObject>();
        _ExitBuildings = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView != null)
        {
            if (photonView.IsMine)
            {
                CameraForwardRaycast();
                HideBuilding();
                AppearBuilding();
            }
        }
        else
        {
            CameraForwardRaycast();
            HideBuilding();
            AppearBuilding();
        }
    }

    void HideBuilding()
    {
        if (_Buildings.Count > 0)
        {
            foreach (var building in _Buildings)
            {
                if (building != null)
                {
                    Renderer buildingrenderer = building.GetComponent<Renderer>();
                    if (buildingrenderer != null)
                    {
                        Material material = buildingrenderer.material;
                        if (material)
                        {
                            float Opacity;
                            Opacity = material.GetFloat("_Opacity");

                            if (Opacity >= 0.3)
                            {
                                Opacity -= 2f * Time.deltaTime;
                                material.SetFloat("_Opacity", Opacity);
                            }
                            else if (Opacity < 0.3f)
                            {
                                Opacity = 0.3f;
                                material.SetFloat("_Opacity", Opacity);
                            }
                        }
                    }
                }
            }
        }
    }

    void AppearBuilding()
    {
        if (_ExitBuildings.Count > 0)
        {
            List<GameObject> _exitBuildings = _ExitBuildings;
            List<GameObject> _removeBulidings = new List<GameObject>();
            foreach (var building in _exitBuildings)
            {
                if (building != null)
                {
                    Renderer buildingrenderer = building.GetComponent<Renderer>();
                    if (buildingrenderer != null)
                    {
                        Material material = buildingrenderer.material;
                        if (material)
                        {
                            float Opacity;
                            Opacity = material.GetFloat("_Opacity");

                            if (Opacity <= 1.0f)
                            {
                                Opacity += 4f * Time.deltaTime;
                                material.SetFloat("_Opacity", Opacity);
                            }
                            else if (Opacity > 1f)
                            {
                                Opacity = 1f;
                                material.SetFloat("_Opacity", Opacity);
                                _removeBulidings.Add(building);
                            }
                        }
                    }
                }
            }
            foreach (var building in _removeBulidings)
            {
                _ExitBuildings.Remove(building);
            }
        }
    }

    void CameraForwardRaycast()
    {
        var ray = new Ray(_Camera.transform.position, _Camera.transform.forward);
        var maxDistance = 10f;
        // 광선 디버그 용도
        Debug.DrawRay(_Camera.transform.position, _Camera.transform.forward * maxDistance, Color.blue);
        RaycastHit[] _raycastHits;
        _raycastHits = Physics.RaycastAll(ray, maxDistance, _fieldLayer);
        
        if (_raycastHits.Length > 0)
        {
            _ExitBuildings.AddRange(_Buildings);
            _ExitBuildings = _ExitBuildings.Distinct().ToList();
            _Buildings.Clear();
            
            foreach (var raycastHit in _raycastHits)
            {
                //GameObject gameObject = _ExitBuildings.Find(x => x == raycastHit.transform.gameObject);
                if (raycastHit.transform.tag == "Wall")
                {
                    if (raycastHit.transform.gameObject != null)
                    {
                        _ExitBuildings.Remove(raycastHit.transform.gameObject);
                    }
                    if (!_Buildings.Contains(raycastHit.transform.gameObject))
                        _Buildings.Add(raycastHit.transform.gameObject);
                    // MeshRenderer hitMeshRenderer = _raycastHit.transform.GetComponent<MeshRenderer>();
                    // if (hitMeshRenderer)
                    // {
                    //     hitMeshRenderer.enabled = false;
                    // }
                }
            }
        }
        else if (!Colliding)
        {
            _ExitBuildings.AddRange(_Buildings);
            _ExitBuildings = _ExitBuildings.Distinct().ToList();
            _Buildings.Clear();
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform != null)
        {
            if (other.transform.tag == "Wall")
            {
                Colliding = true;
                if (other.transform.gameObject != null)
                {
                    _ExitBuildings.Remove(other.transform.gameObject);
                }
                if (!_Buildings.Contains(other.transform.gameObject))
                    _Buildings.Add(other.transform.gameObject);
                // MeshRenderer hitMeshRenderer = _raycastHit.transform.GetComponent<MeshRenderer>();
                // if (hitMeshRenderer)
                // {
                //     hitMeshRenderer.enabled = false;
                // }
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wall")
        {
            _Buildings.Remove(other.transform.gameObject);
            if (!_ExitBuildings.Contains(other.transform.gameObject))
            {
                _ExitBuildings.Add(other.transform.gameObject);
            }
            Colliding = false;
        }
    }
}
