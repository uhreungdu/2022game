using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimeUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gManager;
    public GameObject TimeDisplay;
    int min;
    float secs;
    void Start()
    {
        gManager = GameManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        Time_check();
    }
    void Time_check()
    {
        if(min< 5)
        {
            secs += Time.deltaTime;
            //TimeDisplay.GetComponent<Text>().text = temp.ToString();
            TimeDisplay.GetComponent<Text>().text = string.Format("{0:D1} : {1:D2}",min,(int)secs);
            if((int)secs > 59)
            {
                secs = 0;
                min ++;
            }
        }
        else
        {
            TimeDisplay.GetComponent<Text>().text = "Time up!";
        }
    }
}
