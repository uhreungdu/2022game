using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomBlock : MonoBehaviour
{
    public string iname;
    public string ename;
    public int nowP;
    public int maxP;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVariables(string internal_name, string external_name, int nowPlayerNum, int maxPlayerNum)
    {
        iname = internal_name;
        ename = external_name;
        nowP = nowPlayerNum;   
        maxP = maxPlayerNum;
        SetText();
    }

    void SetText()
    {
        transform.Find("Text").gameObject.GetComponent<Text>().text =
            ename + "\nÀÎ¿ø: " + nowP + "/" + maxP;
    }
}
