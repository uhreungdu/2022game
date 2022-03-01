using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameButton : MonoBehaviour
{
    public LobbyManager gManager;

    // Start is called before the first frame update
    void Start()
    {
        gManager = LobbyManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetName()
    {
        gManager.SetName(GameObject.Find("Name").GetComponent<InputField>().text + Random.Range(0, 9999));
        GameObject.Find("CreateRoomButton").GetComponent<Button>().interactable = true;
        GameObject.Find("Name").GetComponent<InputField>().interactable = false;
        GetComponent<Button>().interactable = false;
    }
}
