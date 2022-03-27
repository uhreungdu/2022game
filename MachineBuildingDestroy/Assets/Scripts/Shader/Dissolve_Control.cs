using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve_Control : MonoBehaviour
{
    // Start is called before the first frame update
    public Material disslve_MAt;
    public float temp_t;
    void Start()
    {
        disslve_MAt = gameObject.GetComponent<Renderer>().material;
        disslve_MAt.SetFloat("CutoffHeight",-2f);
        temp_t = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (temp_t <= 5.2)
        {
            temp_t += Time.deltaTime * 5f;
        }
        disslve_MAt.SetFloat("CutOff_Height",temp_t);
    }
}
