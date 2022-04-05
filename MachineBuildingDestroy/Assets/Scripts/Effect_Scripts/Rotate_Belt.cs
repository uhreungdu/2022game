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
        times = 0f;
        move_V = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        times += Time.deltaTime;
        
        transform.Rotate(new Vector3(0,Time.deltaTime * 50f,0));
        if (times >= 10f)
        {
            Destroy(gameObject);
        }
    }
}
