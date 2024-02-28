using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;

public class DepositReq : MonoBehaviour
{
    public TMP_InputField amountInputField;
    public TMP_InputField txtInputField;
    public TMP_InputField accTypeInputField;

    public string phoneNum;

    void Start()
    {
        phoneNum = LoginManager.Instance.phoneNumber;
    }
    
    public void SetACC(string operatorType)
    {
        accTypeInputField.text = operatorType;
    }

    public void SendDepoReq()
    {
        StartCoroutine(SendDepositRequest());
    }

    IEnumerator SendDepositRequest()
    {
        string url = "https://ccg.pedagogyshop.com//api/balance/request";
        string userPhone = phoneNum;

        // Retrieve values from input fields
        string amount = amountInputField.text;
        string txt = txtInputField.text;
        string accType = accTypeInputField.text;

        // Create JSON data
        string jsonData = $"{{\"user_phone\": \"{userPhone}\", \"accType\": \"{accType}\", \"amount\": {amount}, \"txt\": \"{txt}\"}}";

        // Create UnityWebRequest
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(url, jsonData))
        {
            www.SetRequestHeader("Content-Type", "application/json");

            // Send the request and wait for a response
            yield return www.SendWebRequest();

            // Check for errors
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError($"Error: {www.error}");
            }
            else
            {
                // Parse the response
                string responseText = www.downloadHandler.text;
                Debug.Log($"Response: {responseText}");

                // Example parsing JSON response
                // Note: You should implement proper JSON parsing based on your response structure
                if (responseText.Contains("\"status\": 200") && responseText.Contains("\"message\": \"Data successfully saved\""))
                {
                    Debug.Log("Deposit request successful!");
                }
                else
                {
                    Debug.LogError("Deposit request failed. Check the response for details.");
                }
            }
        }
    }
}
