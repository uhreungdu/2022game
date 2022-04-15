using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class UpdateLogWindowSize : MonoBehaviour
{
    public GameObject logObj;
    private Text _logText;
    private int _enterNum;

    private void Start()
    {
        _logText = logObj.GetComponent<Text>();
    }

    private void FixedUpdate()
    {
        _enterNum = Regex.Matches(_logText.text, "\n").Count;
        print(_enterNum);
    }
}
