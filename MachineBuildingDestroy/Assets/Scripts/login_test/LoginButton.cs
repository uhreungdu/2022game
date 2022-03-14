using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class LoginButton : MonoBehaviour
{
    [FormerlySerializedAs("IDInput")] public GameObject idInput;
    [FormerlySerializedAs("PWInput")] public GameObject pwInput;
    [FormerlySerializedAs("MakeCharWindow")] public GameObject makeCharWindow;
    [FormerlySerializedAs("ErrText")] public GameObject errText;
    public GameObject nManager;
    [SerializeField] private string[] accountVal;
    
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
        idInput.GetComponent<InputField>().interactable = false;
        pwInput.GetComponent<InputField>().interactable = false;
        
        // 로그인 요청
        StartCoroutine(LoginRequest());
    }
    
    IEnumerator LoginRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", "\""+idInput.GetComponent<InputField>().text+"\"") ;
        form.AddField("pw", "\"" + pwInput.GetComponent<InputField>().text + "\"");

        UnityWebRequest www = UnityWebRequest.Post("http://121.139.87.70/login/login_account.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            // 로그인 창 활성화
            GetComponent<Button>().interactable = true;
            idInput.GetComponent<InputField>().interactable = true;
            pwInput.GetComponent<InputField>().interactable = true;
        }
        else
        {
            string results = www.downloadHandler.text;
            Debug.Log(results);
            accountVal = results.Split(';');
            // 로그인 창 활성화
            GetComponent<Button>().interactable = true;
            idInput.GetComponent<InputField>().interactable = true;
            pwInput.GetComponent<InputField>().interactable = true;
            if (GetStringDataValue(accountVal[0],"Msg:") == "OK")
            {
                // 캐릭터 보유, 로비씬 이동
                errText.SetActive(false);
                GameObject.Find("Account").GetComponent<Account>().WriteAccount(
                    GetStringDataValue(accountVal[0],"account_id:"),
                    GetStringDataValue(accountVal[0],"character_name:"));
                nManager.GetComponent<NetworkManager>().ConnectPhotonServer();
                SceneManager.LoadScene("lobby_test");

            }
            else if (GetStringDataValue(accountVal[0],"Msg:") == "Need Character")
            {
                // 캐릭터 미보유, 설정 필요
                makeCharWindow.SetActive(true);
            }
            else
            {
                // 로그인 오류
                errText.SetActive(true);
                errText.GetComponent<Text>().text = GetStringDataValue(accountVal[0],"Msg:");
            }
        }
    }
    
    string GetStringDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|")) value = value.Remove(value.IndexOf("|"));
        return value;
    }
}
