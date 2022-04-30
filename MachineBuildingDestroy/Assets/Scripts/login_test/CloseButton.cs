using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    public GameObject window;
    public GameObject darkBackground;

    public void OnClick()
    {
        window.SetActive(false);
        darkBackground.SetActive(false);
    }
}
