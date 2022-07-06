using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyRoomHelpButton : MonoBehaviour
{
    public GameObject helpWindow;
    public GameObject darkBackground;
    
    public void OnClick()
    {
        darkBackground.SetActive(true);
        helpWindow.SetActive(true);
    }
}
