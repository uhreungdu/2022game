using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginButton : MonoBehaviour
{
    private GameObject IDInput;
    private GameObject PWInput;
    
    // Start is called before the first frame update
    void Start()
    {
        IDInput = GameObject.Find("IDInputField");
        PWInput = GameObject.Find("PWInputField");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        GetComponent<Button>().interactable = false;
        IDInput.GetComponent<InputField>().interactable = false;
        PWInput.GetComponent<InputField>().interactable = false;
    }
}
