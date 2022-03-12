using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class IDCheckButton : MonoBehaviour
{
    [FormerlySerializedAs("IDInput")] public GameObject idInput;
    [FormerlySerializedAs("PWInput")] public GameObject pwInput;
    [FormerlySerializedAs("ErrText")] public GameObject errText;
    
    private bool _working = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!_working && idInput.GetComponent<InputField>().text.Length != 0)
        {
            GetComponent<Button>().interactable = true;    
        }
        else
        {
            GetComponent<Button>().interactable = false;
        }
    }

    public void OnClick()
    {
        // IDCheck 비활성화
        GetComponent<Button>().interactable = false;
        idInput.GetComponent<InputField>().interactable = false;

        // ID 검증 요청
        _working = true;
        StartCoroutine(IDCheckRequest());
    }

    IEnumerator IDCheckRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", "\"" + idInput.GetComponent<InputField>().text + "\"");

        UnityWebRequest www = UnityWebRequest.Post("http://121.139.87.70/login/id_check.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            // IDCheck 활성화
            GetComponent<Button>().interactable = true;
            idInput.GetComponent<InputField>().interactable = true;
            pwInput.GetComponent<InputField>().interactable = false;
        }
        else
        {
            Debug.Log("Form upload complete!");
            string results = www.downloadHandler.text;
            Debug.Log(results);
            // IDCheck 활성화
            GetComponent<Button>().interactable = true;
            idInput.GetComponent<InputField>().interactable = true;
            if (results == "OK")
            {
                pwInput.GetComponent<InputField>().interactable = true;
                errText.SetActive(false);
            }
            else
            {
                errText.GetComponent<Text>().text = results;
                errText.SetActive(true);
            }
            _working = false;
        }
    }
}
