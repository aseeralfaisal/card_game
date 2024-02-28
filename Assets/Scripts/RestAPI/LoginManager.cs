using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public string loginUrl = "https://ccg.pedagogyshop.com/api/signin";
    public TMP_InputField phoneInputField;
    public TMP_InputField passwordInputField;
    public GameObject retryLogin;
    public string phoneNumber;
    public string balance;
    public string userName;
    
    public static LoginManager Instance;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    public void LoginSystem()
    {
        string phone = phoneInputField.text;
        string password = passwordInputField.text;
        StartCoroutine(LoginUser(phone, password));
    }

    IEnumerator LoginUser(string phone, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("phone", phone);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post(loginUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Login successful!");
                
                LoadScene("MainMenu");
                phoneNumber = phone;

                // Handle the response data as needed
                string responseText = www.downloadHandler.text;
                HandleLoginResponse(responseText);
            }
            else
            {
                Debug.LogError("Login failed: " + www.error);
            }
        }
    }

    void HandleLoginResponse(string responseText)
    {
        // Parse the JSON response and handle accordingly
        // Note: You may want to use a JSON parsing library or serialize/deserialize the JSON response
        // Example: using UnityEngine.JsonUtility
        // Example: MyResponseClass response = JsonUtility.FromJson<MyResponseClass>(responseText);
        
        Debug.Log("Response: " + responseText);
        RetryLogin();
    }

    public void LoadScene(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }

    public void RetryLogin()
    {
        retryLogin.SetActive(true);
    }
}
