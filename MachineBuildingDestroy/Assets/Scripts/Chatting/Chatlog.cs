using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Chatlog : MonoBehaviour
{
    private Text _text;
    public GameObject logWindow;
    public GameObject gScrollbar;

    private void Awake()
    {
        _text = GetComponent<Text>();
    }

    public void AddLine(string str)
    {
        _text.text += str+System.Environment.NewLine;
        var enterNum = Regex.Matches(_text.text, "\n").Count;
        logWindow.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 20.2f * enterNum);
        var mScrollbar = gScrollbar.GetComponent<Scrollbar>();
        if (mScrollbar.enabled)
        {
            mScrollbar.value = 0;
        }
    }
    
    
}
