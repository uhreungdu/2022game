using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve_Control : MonoBehaviour
{
    // Start is called before the first frame update
    public Material disslve_MAt;
    public float temp_t;
    public bool dissolve_switch;
    void Start()
    {
        disslve_MAt = gameObject.GetComponent<Renderer>().material;
        disslve_MAt.SetFloat("CutoffHeight",-2f);
        temp_t = -2f;
        dissolve_switch = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (dissolve_switch == true)
        {
            undissolve_effect();
        }
        else
        {
            dissolve_effect();
        }
    }

    public void undissolve_effect()
    {
        if (temp_t <= 5.2)
        {
            temp_t += Time.deltaTime * 5f;
        }
        disslve_MAt.SetFloat("CutOff_Height",temp_t);
    }

    public void dissolve_effect()
    {
        if (temp_t >= -2)
        {
            temp_t -= Time.deltaTime * 5f;
        }
        disslve_MAt.SetFloat("CutOff_Height",temp_t);
        if (temp_t < -2f)
        {
            Destroy(gameObject);
        }
    }
}
