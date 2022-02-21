using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimeUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gManager;
    public GameObject TimeDisplay;
    private Text Timetext;
    int min;
    float secs;
    float time_temp;
    void Start()
    {
        gManager = GameManager.GetInstance();
        Timetext = TimeDisplay.GetComponent<Text>();
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
            time_temp += Time.deltaTime;
            secs += Time.deltaTime;
            //TimeDisplay.GetComponent<Text>().text = temp.ToString();
            Timetext.text = string.Format("{0:D1} : {1:D2}",min,(int)secs);
            if((int)secs > 59)
            {
                secs = 0;
                min ++;
                
            }
            gManager.now_timer.settimes(min,secs,time_temp);
            gManager.EManager.SetTime(min,secs,time_temp);
            gManager.EManager.Active_Itembox();
            gManager.EManager.Active_Goalopost();
            gManager.EManager.Active_landmakr();
        }
        else
        {
            Timetext.text = "Time up!";
        }
    }
}
