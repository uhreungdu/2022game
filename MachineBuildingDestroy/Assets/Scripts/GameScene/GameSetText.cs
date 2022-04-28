using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSetText : MonoBehaviour
{
    public TextMeshProUGUI _textMeshPro;
    private GameManager _gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.GetInstance();
        _textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManager.EManager.gameSet)
            _textMeshPro.text = "Game Set!";
    }
}
