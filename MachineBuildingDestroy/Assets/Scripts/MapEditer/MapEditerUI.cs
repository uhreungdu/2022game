using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapEditerUI : MonoBehaviour
{
    public bool SaveMode = false;
    public Text PathText;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SaveModeEnable()
    {
        SaveMode = true;
    }
    
    public void SaveModeDisable()
    {
        SaveMode = false;
    }

}
