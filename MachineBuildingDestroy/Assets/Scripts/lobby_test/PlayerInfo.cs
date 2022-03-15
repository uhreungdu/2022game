using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public GameObject infoWindow;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        infoWindow.SetActive(true);
        StartCoroutine(infoWindow.GetComponent<InfoWindow>().GetPlayerInfo());
    }
}
