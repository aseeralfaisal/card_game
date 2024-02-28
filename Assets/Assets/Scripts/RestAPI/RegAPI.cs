using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using Newtonsoft.Json.Linq;

public class RegAPI : MonoBehaviour
{
    public string registrationUrl = "https://ccg.pedagogyshop.com/api/register";
    public TMP_InputField nameInputField;
    public TMP_InputField emailInputField;
    public TMP_InputField phoneInputField;
    public TMP_InputField referralIdInputField;
    public TMP_InputField passwordInputField;
    public TMP_InputField confirmPasswordInputField;

    public string userID;

    

    public void RegisterUser()
    {
        string name = nameInputField.text;
        string email = emailInputField.text;
        string phone = phoneInputField.text;
        string referralId = referralIdInputField.text;
        string password = passwordInputField.text;
        string confirmPassword = confirmPasswordInputField.text;

        StartCoroutine(SendRegistrationRequest(name, email, phone, referralId, password, confirmPassword));
    }

    IEnumerator SendRegistrationRequest(string name, string email, string phone, string referralId, string password, string confirmPassword)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("email", email);
        form.AddField("phone", phone);

        // Include referral_id only if it's not empty
        if (!string.IsNullOrEmpty(referralId))
        {
            form.AddField("referral_id", referralId);
        }

        form.AddField("password", password);
        form.AddField("password_confirmation", confirmPassword); 
        using (UnityWebRequest www = UnityWebRequest.Post(registrationUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Registration successful!");

                // Handle the response data as needed
                string responseText = www.downloadHandler.text;
                HandleRegistrationResponse(responseText);
            }
            else
            {
                Debug.LogError("Registration failed: " + www.error);
            }
        }
    }

    void HandleRegistrationResponse(string responseText)
    {
        // Parse the JSON response using Newtonsoft.Json
        JObject responseJson = JObject.Parse(responseText);

        // Check if OTP status is 200
        if (responseJson["OTP-status"].Value<int>() == 200)
        {
            string message = responseJson["message"].Value<string>();
            userID = responseJson["user_id"].Value<string>();

            Debug.Log("Registration successful. " + message);
            Debug.Log("User ID: " + userID);
        }
        else
        {
            Debug.LogError("Registration failed. OTP status: " + responseJson["OTP-status"].Value<int>());
            Debug.LogError("Message: " + responseJson["message"].Value<string>());
        }
    }

        
    
}