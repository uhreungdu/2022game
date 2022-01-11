using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject[] objects;
    
    // Start is called before the first frame update
    void Start()
    {
        string path = Application.dataPath + "/Map.csv";
        string[] map = System.IO.File.ReadAllLines(path);

        if (map.Length > 0)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
