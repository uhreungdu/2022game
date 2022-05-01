using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class RoomList : MonoBehaviour
{
    [SerializeField]
    private GameObject[] RoomBlocks = new GameObject[8];
    [SerializeField]
    private string[] rooms;
    
    public int listIndex = 0;
    private int _maxIndex = 0;

    public GameObject nextButton;
    public GameObject prevButton;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetRoomList(string[] val)
    {
        rooms = val;
    }

    public void MoveNextPage()
    {
        if (_maxIndex == listIndex) return;
        listIndex++;
        SetRoomBlocks();
    }
    
    public void MovePrevPage()
    {
        if (0 == listIndex) return;
        listIndex--;
        SetRoomBlocks();
    }

    public void SetRoomBlocks()
    {
        _maxIndex = (rooms.Length - 2) / 8;
        if (_maxIndex < listIndex) listIndex = _maxIndex;
        nextButton.GetComponent<Button>().interactable = _maxIndex != listIndex;
        prevButton.GetComponent<Button>().interactable = 0 != listIndex;

        string iname;
        string ename;
        int nowP;
        int maxP;
        bool ingame;

        var index = listIndex * 8;
        foreach (var roomBlock in RoomBlocks)
        {
            if (rooms.Length - 2 < index)
            {
                roomBlock.GetComponent<RoomBlock>().SetVariables("", "", 0, 0, false);
                continue;
            }

            iname = GetStringDataValue(rooms[index], "internal_name:");
            ename = GetStringDataValue(rooms[index], "external_name:");
            nowP = GetIntDataValue(rooms[index], "now_playernum:");
            maxP = GetIntDataValue(rooms[index], "max_playernum:");
            ingame = GetBoolDataValue(rooms[index], "ingame:");

            roomBlock.GetComponent<RoomBlock>().SetVariables(iname, ename, nowP, maxP, ingame);
            
            index++;
        }
    }

    private string GetStringDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|")) value = value.Remove(value.IndexOf("|"));
        return value;
    }

    private int GetIntDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|")) value = value.Remove(value.IndexOf("|"));
        return int.Parse(value);
    }

    private bool GetBoolDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|")) value = value.Remove(value.IndexOf("|"));
        return value == "1";
    }
}
