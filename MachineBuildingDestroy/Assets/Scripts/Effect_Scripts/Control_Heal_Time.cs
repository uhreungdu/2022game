using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Control_Heal_Time : MonoBehaviour
{
    private float Effect_Time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Effect_Time += Time.deltaTime;
        if (Effect_Time >= 5f)
        {
            Destroy(gameObject);
        }
    }
}
