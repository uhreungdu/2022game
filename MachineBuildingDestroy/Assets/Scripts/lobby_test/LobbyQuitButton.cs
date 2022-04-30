using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyQuitButton : MonoBehaviour
{
    public GameObject window;
    public GameObject darkBackground;
    public void OnClick()
    {
        window.SetActive(true);
        darkBackground.SetActive(true);
    }
}
