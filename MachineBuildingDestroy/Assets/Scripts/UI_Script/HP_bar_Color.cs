using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_bar_Color : MonoBehaviour
{
    
    public Image hpbar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hpbar.fillAmount <= 0.1f)
        {
            hpbar.color = Color.red;
        }
        else
        {
            hpbar.color = Color.green;
        }
    }
}
