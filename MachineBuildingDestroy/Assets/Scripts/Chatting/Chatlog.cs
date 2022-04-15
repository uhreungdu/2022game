using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chatlog : MonoBehaviour
{
    private Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
    }

    public void AddLine(string str)
    {
        _text.text += str;
        //_text.text=_text.text.Replace("\\n", "\n");
    }
    
    
}
