using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomList : MonoBehaviour
{
    [SerializeField]
    private GameObject[] RoomBlocks = new GameObject[6];
    [SerializeField]
    private string[] rooms;
    public int listIndex = 1;
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

    public void SetRoomBlocks()
    {
        for(int i = 0; i < rooms.Length-1; i++)
        {
            string iname = GetStringDataValue(rooms[i], "internal_name:");
            string ename = GetStringDataValue(rooms[i], "external_name:");
            int nowP = GetIntDataValue(rooms[i], "now_playernum:");
            int maxP = GetIntDataValue(rooms[i], "max_playernum:");

            RoomBlocks[i].GetComponent<RoomBlock>().
                SetVariables(iname,ename,nowP,maxP);
                
        }
        for (int i = rooms.Length-1; i < 6; i++)
        {
            string iname = "";
            string ename = "";
            int nowP = 0;
            int maxP = 0;

            RoomBlocks[i].GetComponent<RoomBlock>().
                SetVariables(iname, ename, nowP, maxP);

        }
    }

    string GetStringDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|")) value = value.Remove(value.IndexOf("|"));
        return value;
    }

    int GetIntDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|")) value = value.Remove(value.IndexOf("|"));
        return int.Parse(value);
    }
}
