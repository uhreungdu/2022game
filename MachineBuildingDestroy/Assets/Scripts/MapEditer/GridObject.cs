using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public GameObject GridGameObject;
    public GameObject Pointer;
    // Update is called once per frame
    private void Start()
    {
        // GridDraw();
    }

    private void Update()
    {
        transform.SetSiblingIndex(11);
    }
}