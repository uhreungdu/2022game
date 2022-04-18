using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameRotate : MonoBehaviour
{
    private GameObject _camera;
    // Start is called before the first frame update
    void Start()
    {
        _camera = GameObject.FindWithTag("CamPos");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(_camera.transform);
        transform.Rotate(new Vector3(0, 1, 0), 180f);
    }
}
