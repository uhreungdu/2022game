using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerImpact : MonoBehaviourPun
{
    float mass = 3.0F; // defines the character mass
    Vector3 impact = Vector3.zero;
    private CharacterController character;
    // Use this for initialization
    void Start () {
        character = GetComponent<CharacterController>();
    }
    // Update is called once per frame
    void FixedUpdate () {
        // apply the impact force:
        if (impact.magnitude > 0.2F) character.Move(impact * Time.deltaTime);
        // consumes the impact energy each cycle:
        impact = Vector3.Lerp(impact, Vector3.zero, 5*Time.deltaTime);
    }

    public void NetworkAddImpact(Vector3 dir, float force)
    {
        photonView.RPC("AddImpact", RpcTarget.AllViaServer, dir, force);
    }
    
    [PunRPC]
    // call this function to add an impact force:
    public void AddImpact(Vector3 dir, float force) 
    {
        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        impact += dir.normalized * force / mass;
    }
}
