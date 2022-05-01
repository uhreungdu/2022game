using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstcle_put_down : MonoBehaviour
{
    public bool can_put_down;
    // Start is called before the first frame update
    private void Start()
    {
        can_put_down = true;
    }

    private void OnTriggerStay(Collider other)
    {
        can_put_down = false;
    }

    private void OnTriggerExit(Collider other)
    {
        can_put_down = true;
    }
}
