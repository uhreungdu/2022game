using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharChangeButton : MonoBehaviour
{
    public GameObject window;
    public GameObject darkBackground;
    
    void OnClick()
    {
        window.SetActive(true);
    }
}
