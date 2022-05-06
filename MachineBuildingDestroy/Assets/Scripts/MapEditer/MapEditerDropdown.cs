using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapEditerDropdown : MonoBehaviour
{
    public Map _Map;
    public Dropdown _Dropdown;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable()
    {
        if (_Dropdown == null)
        {
            _Dropdown = GetComponent<Dropdown>();
        }
        Init();
    }

    private void Update()
    {
        SelectButton();
    }

    private void Init()
    {
        _Map.LoadMapList();
        _Dropdown.options.Clear();
        foreach (var maptile in _Map.MapList)
        {
            Dropdown.OptionData optionData = new Dropdown.OptionData();
            if (maptile.MapName != null)
                optionData.text = maptile.MapName;
            else
                optionData.text = "NoName";
            _Dropdown.options.Add(optionData);
        }
    }
    
    public void SelectButton()// SelectButton을 누름으로써 값 테스트.
    {
        Debug.Log("Dropdown Value: "+ _Dropdown.value +", List Selected: " + (_Dropdown.value + 1));
    }

    public string SelectText()
    {
        return _Dropdown.options[_Dropdown.value].text;
    }

    public void LoadButton()
    {
        _Map.LoadMapFile(Application.dataPath + "/" + "Map", SelectText());
        _Map.MapLoad();
    }
}
