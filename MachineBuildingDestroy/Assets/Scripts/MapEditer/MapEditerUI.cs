using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditerUI : MonoBehaviour
{
    public bool SaveMode = false;
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
