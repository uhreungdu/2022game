using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class IDCheckButton : MonoBehaviour
{
    private GameObject IDInput;

    // Start is called before the first frame update
    void Start()
    {
        IDInput = GameObject.Find("R_IDInputField");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        // IDCheck 비활성화
        GetComponent<Button>().interactable = false;
        IDInput.GetComponent<InputField>().interactable = false;

        // ID 검증 요청
        StartCoroutine(IDCheckRequest());
    }

    IEnumerator IDCheckRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", "\"" + IDInput.GetComponent<InputField>().text + "\"");

        UnityWebRequest www = UnityWebRequest.Post("http://121.139.87.70/login/id_check.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            // IDCheck 활성화
            GetComponent<Button>().interactable = true;
            IDInput.GetComponent<InputField>().interactable = true;
        }
        else
        {
            Debug.Log("Form upload complete!");
            string results = www.downloadHandler.text;
            Debug.Log(results);
            // IDCheck 활성화
            GetComponent<Button>().interactable = true;
            IDInput.GetComponent<InputField>().interactable = true;
        }
    }
}
