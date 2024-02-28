using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;

public class WithdrawalRequest : MonoBehaviour
{
    private string userIdInput; //not inputable
    public TMP_InputField operatorInput; // not inputable
    public TMP_InputField amountInput; //put input
    public TMP_InputField phoneInput; //put input
    public TextMeshProUGUI responseText; //showthe Respone in a UI Text

    void Start()
    {
        // Start your withdrawal request when needed, e.g., on a button click
        userIdInput = LoginManager.Instance.phoneNumber;
    }

    public void SetOperator(string operatorType)
    {
        operatorInput.text = operatorType;
    }

    public void SendWithdrawalRequest()
    {
        string url = "https://ccg.pedagogyshop.com/api/withdraw/request";
        string user_id = userIdInput;
        string operatorName = operatorInput.text;
        int amount = int.Parse(amountInput.text); // Assuming amount is an integer
        string phone = phoneInput.text;

        // Create JSON data
        string jsonData = "{\"user_id\":\"" + user_id + "\",\"operator\":\"" + operatorName + "\",\"amount\":" + amount + ",\"phone\":\"" + phone + "\"}";

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
