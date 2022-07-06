using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHelpWindow : MonoBehaviour
{
    public GameObject helpWindow;
    public GameObject darkBackground;
    
    public void OnClick()
    {
        darkBackground.SetActive(false);
        helpWindow.SetActive(false);
    }
}
