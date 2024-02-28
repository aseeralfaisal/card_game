using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;

public class BalanceDecrement : MonoBehaviour
{
    private string userIdInput;
    public TextMeshProUGUI responseText;
    public static BalanceDecrement Instance;

    public int amount;
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
        // Start your balance decrement when needed, e.g., on a button click
        userIdInput = LoginManager.Instance.phoneNumber;
    }
    
    public void DecrementBalance()
    {
        string theAmount = amountWritten.text;
        // Attempt to parse the input string to an integer
        if (int.TryParse(theAmount, out amount))
        {
            // Conversion successful, proceed with sending the balance decrement request
            SendBalanceDecrementRequest(amount);
        }
        else
        {
            // Conversion failed, handle the error (e.g., display an error message)
            Debug.LogError("Invalid input: The amount is not a valid integer.");
            responseText.text = "Error: Invalid input";
        }
    }

    // Public method to be called with balanceAmount as an input
    public void SendBalanceDecrementRequest(int balanceAmount)
    {
        string url = "https://ccg.pedagogyshop.com/api/balance/decrement";
        string phone = userIdInput;

        // Create JSON data
        string jsonData = "{\"phone\":\"" + phone + "\",\"amount\":" + balanceAmount + "}";

        StartCoroutine(SendRequest(url, jsonData));
    }

    IEnumerator SendRequest(string url, string jsonData)
    {
        // Create a UnityWebRequest
        UnityWebRequest request = UnityWebRequest.PostWwwForm(url, jsonData);
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
