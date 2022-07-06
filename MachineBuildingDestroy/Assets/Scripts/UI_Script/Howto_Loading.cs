using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Howto_Loading : MonoBehaviour
{
    public GameObject howToPlay_PC;
    public GameObject howToPlay_Phone;

    private void Awake()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            howToPlay_PC.SetActive(false);
        }
        else
        {
            howToPlay_Phone.SetActive(false);
        }
    }
}
