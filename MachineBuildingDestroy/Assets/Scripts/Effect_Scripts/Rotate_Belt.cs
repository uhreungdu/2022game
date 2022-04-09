using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_Belt : MonoBehaviour
{
    // Start is called before the first frame update
    public float times;
    public float move_V;
    void Start()
    {
        move_V = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0,Time.deltaTime * 50f,0));
    }
}
