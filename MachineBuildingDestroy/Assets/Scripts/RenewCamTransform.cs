using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenewCamTransform : MonoBehaviour
{
    Transform tr;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.SetPositionAndRotation(GameObject.FindWithTag("MainCamera").GetComponent<Transform>().position, GameObject.FindWithTag("MainCamera").GetComponent<Transform>().rotation);
        tr = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.SetPositionAndRotation(tr.position, tr.rotation);
    }
}
