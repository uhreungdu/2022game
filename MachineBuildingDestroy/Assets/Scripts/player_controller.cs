using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody body;
    public float player_speed = 5.0f;
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        body.position += new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical")
        *player_speed*Time.deltaTime);
    }
}
