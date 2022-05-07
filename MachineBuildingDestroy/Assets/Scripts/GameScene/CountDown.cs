using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    private Text _text;
    public GameManager _gameManager;
    
    private float gameStartTime;    // 게임 시작 시간
    private int startfontSize = 200;             // 처음 사이즈
    private int lastfontSize = 40;              // 마지막 사이즈
    private float fontSizeVar;     // 계산용
    private Color fontColor;        // 처음 알파
    private float lasttextAlpha = 1.0f;        // 마지막 알파
    private float starttextAlpha = 0.3f;        // 처음 알파
    private float textAlphaVar = 0.3f;        // 계산용
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<Text>();
        fontColor = Color.black;
        
        gameStartTime = Time.time;
        _text.text = "3";
        fontSizeVar = startfontSize;
        _text.fontSize = (int)fontSizeVar;
        
        _gameManager = GameManager.GetInstance();
        
        // textAlphaVar = starttextAlpha;
        // fontColor.a = starttextAlpha;
    }

    private void OnEnable()
    {
        _text = GetComponent<Text>();
        fontColor = Color.black;
        
        gameStartTime = Time.time;
        _text.text = "3";
        fontSizeVar = startfontSize;
        _text.fontSize = (int)fontSizeVar;
        
        _gameManager = GameManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        CountDownTimer();
        FontSizeUpdate();
    }

    void CountDownTimer()
    {
        if (Time.time >= gameStartTime + 4.0f && _text.text == "Start!")
        {
            _text.gameObject.SetActive(false);
        }
        else if (Time.time >= gameStartTime + 3.0f && _text.text == "1")
        {
            _text.text = "Start!";
            _gameManager.SetGameStart(true);
            ResetFontSize();
        }
        else if (Time.time >= gameStartTime + 2.0f && _text.text == "2")
        {
            _text.text = "1";
            ResetFontSize();
        }
        else if (Time.time >= gameStartTime + 1.0f && _text.text == "3")
        {
            _text.text = "2";
            ResetFontSize();
        }
    }

    void FontSizeUpdate()
    {
        if (_text.fontSize >= lastfontSize && Time.time <= gameStartTime + 4.0f)
        {
            fontSizeVar -= (Time.deltaTime * (startfontSize - lastfontSize));
            _text.fontSize = (int)fontSizeVar;
        }
    }
    
    void FontAlphaUpdate()
    {
        if (_text.color.a <= lasttextAlpha && Time.time <= gameStartTime + 3.0f)
        {
            Color color = fontColor;
            textAlphaVar += Time.deltaTime * (lasttextAlpha - starttextAlpha);
            _text.color = color;
        }
    }

    void ResetFontSize()
    {
        fontSizeVar = startfontSize;
        _text.fontSize = startfontSize;
    }
    
    void ResetFontAlpha()
    {
        textAlphaVar = starttextAlpha;
        _text.color = fontColor;
    }
}
