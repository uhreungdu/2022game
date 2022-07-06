using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class RoomList : MonoBehaviour
{
    struct RoomInfo
    {
        public string internal_name;
        public string external_name;
        public int now_playernum;
        public int max_playernum;
        public bool ingame;

        public RoomInfo(string iname, string ename, int nPnum, int mPnum, bool ingame)
        {
            internal_name = iname;
            external_name = ename;
            now_playernum = nPnum;
            max_playernum = mPnum;
            this.ingame = ingame;
        }
    }
    
    [SerializeField]
    private GameObject[] RoomBlocks = new GameObject[8];
    [SerializeField]
    private List<RoomInfo> rooms = new List<RoomInfo>();
    
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

    public void SetRoomList(byte[] val)
    {
        var data = new RoomInfo(Encoding.UTF8.GetString(val, 7, val[2]),
            Encoding.UTF8.GetString(val, 7 + val[2], val[3]),
            val[4], val[5], Convert.ToBoolean(val[6]));
        rooms.Add(data);
        SetRoomBlocks();
    }

    public void CleanRoomList()
    {
        if (rooms.Count != 0)
            rooms.Clear();
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
        _maxIndex = (rooms.Count - 2) / 8;
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
            if (rooms.Count - 1 < index)
            {
                roomBlock.GetComponent<RoomBlock>().SetVariables("", "", 0, 0, false);
                continue;
            }

            iname = rooms[index].internal_name;
            ename = rooms[index].external_name;
            nowP =rooms[index].now_playernum;
            maxP = rooms[index].max_playernum;
            ingame = rooms[index].ingame;

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
        return value == "True";
    }
}
