using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButton : MonoBehaviour
{
    public bool isPressed;

    public void ButtonUp()
    {
        isPressed = false;
    }
    
    public void ButtonDown()
    {
        isPressed = true;
    }
}
