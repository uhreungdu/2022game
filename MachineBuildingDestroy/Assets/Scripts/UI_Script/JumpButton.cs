using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpButton : MonoBehaviour
{
    public bool isPressed;

    private void Awake()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            gameObject.SetActive(false);
        }
    }

    public void ButtonUp()
    {
        isPressed = false;
    }
    
    public void ButtonDown()
    {
        isPressed = true;
    }
}
