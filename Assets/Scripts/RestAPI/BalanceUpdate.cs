using System;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;

public class BalanceUpdate : MonoBehaviour
{
    private string userIdInput;
    public TextMeshProUGUI responseText;
    public static BalanceUpdate Instance;

    public int amount;
    public string theAmount;
    public TMP_InputField amountWritten;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        // Ensure that this GameObject persists through scene changes
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        // keep Empty
        
    }
    
    

    public void BUpdateWithdraw()
    {
        userIdInput = LoginManager.Instance.phoneNumber;
        theAmount = amountWritten.text;
        // Attempt to parse the input string to an integer
        if (int.TryParse(theAmount, out amount))
        {
            // Conversion successful, proceed with sending the balance update request
            SendBalanceUpdateRequest(amount);
        }
        else
        {
            // Conversion failed, handle the error (e.g., display an error message)
            Debug.LogError("Invalid input: The amount is not a valid integer.");
            responseText.text = "Error: Invalid input";
        }
    }

    // Public method to be called with balanceAmount as an input
    public void SendBalanceUpdateRequest(int balanceAmount)
    {
        string url = "https://ccg.pedagogyshop.com/api/balance/update";
        string user_id = userIdInput;

        // Create JSON data
        string jsonData = "{\"user_id\":\"" + user_id + "\",\"balanceAmount\":" + balanceAmount + "}";

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
            // Example: You can extract values like status, userName, and newBalance from the JSON response
            // and use them accordingly
            // JSONObject jsonResponse = new JSONObject(response);
            // int status = (int)jsonResponse["status"].n;
            // string userName = jsonResponse["userName"].str;
            // string newBalance = jsonResponse["newBalance"].str;
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
