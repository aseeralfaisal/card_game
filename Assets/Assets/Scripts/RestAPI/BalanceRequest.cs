using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;

public class BalanceRequest : MonoBehaviour
{
    public string userIdInput; // not inputable
    public string operatorInput; // not inputable
    public TMP_InputField amountInput; // put input
    public TMP_InputField txtInput; // put input
    public TextMeshProUGUI responseText; // show the response in a UI Text

    void Start()
    {
        // Start your deposit request when needed, e.g., on a button click
        userIdInput = LoginManager.Instance.phoneNumber;
    }

    public void SetOperator(string operatorType)
    {
        operatorInput = operatorType;
    }

    public void SendDepositRequest()
    {
        string url = "https://ccg.pedagogyshop.com/api/balance/request";
        string userPhone = userIdInput; // Example user phone, replace with actual input
        string accType = operatorInput; // Example account type, replace with actual input
        int amount = int.Parse(amountInput.text); // Assuming amount is an integer
        string txt = txtInput.text;

        // Create JSON data
        string jsonData = "{\"user_phone\":\"" + userPhone + "\",\"accType\":\"" + accType + "\",\"amount\":" + amount + ",\"txt\":\"" + txt + "\"}";

        StartCoroutine(SendRequest(url, jsonData));
    }

    IEnumerator SendRequest(string url, string jsonData)
    {
        // Create a UnityWebRequest
        UnityWebRequest request = UnityWebRequest.PostWwwForm(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Successful response
            string response = request.downloadHandler.text;
            Debug.Log("Response: " + response);
            // Display the response in TextMeshProUGUI
            responseText.text = response;
            // Parse the JSON response if needed
        }
        else
        {
            // Error occurred
            string error = request.error;
            Debug.LogError("Error: " + error);
            // Display the error in TextMeshProUGUI
            responseText.text = "Error: " + error;
        }
    }
}
