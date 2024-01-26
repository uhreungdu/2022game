using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 
    _controller : MonoBehaviour
{
    // Start is called before the first frame update

    public float player_speed = 30f;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");
    }
}
