using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class CharMakeButton : MonoBehaviour
{
    [FormerlySerializedAs("IDInput")] public GameObject idInput;
    [FormerlySerializedAs("NameInput")] public GameObject nameInput;
    [FormerlySerializedAs("ErrText")] public GameObject errText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnClick()
    {
        // 로그인 창 비활성화
        GetComponent<Button>().interactable = false;
        nameInput.GetComponent<InputField>().interactable = false;

        // 로그인 요청
        StartCoroutine(MakeCharRequest());
    }
    
    IEnumerator MakeCharRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", "\"" + idInput.GetComponent<InputField>().text + "\"");
        form.AddField("name", "\""+nameInput.GetComponent<InputField>().text+"\"") ;

        UnityWebRequest www = UnityWebRequest.Post("http://121.139.87.70/login/make_character.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            // 로그인 창 활성화
            GetComponent<Button>().interactable = true;
            nameInput.GetComponent<InputField>().interactable = true;
        }
        else
        {
            string results = www.downloadHandler.text;
            Debug.Log(results);
            // 로그인 창 활성화
            GetComponent<Button>().interactable = true;
            nameInput.GetComponent<InputField>().interactable = true;
            if (results == "OK")
            {
                errText.SetActive(false);
            }
            else
            {
                errText.SetActive(true);
                errText.GetComponent<Text>().text = results;
            }
        }
    }
}
