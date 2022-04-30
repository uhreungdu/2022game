using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginQuitButton : MonoBehaviour
{
    public GameObject window;
    public GameObject darkBackground;
    public void OnClick()
    {
        window.SetActive(true);
        darkBackground.SetActive(true);
    }
}
