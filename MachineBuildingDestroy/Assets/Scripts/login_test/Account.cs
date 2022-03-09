using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Account : MonoBehaviour
{
    [SerializeField] private string pID;
    [SerializeField] private string pNickname;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WriteAccount(string id, string nickname)
    {
        pID = id;
        pNickname = nickname;
    }

    public string GetPlayerID()
    {
        return pID;
    }

    public string GetPlayerNickname()
    {
        return pNickname;
    }
}
