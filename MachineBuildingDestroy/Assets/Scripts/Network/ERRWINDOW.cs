using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERRWINDOW : MonoBehaviour
{
    private void Awake()
    {
        var obj = FindObjectsOfType<ERRWINDOW>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
