using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using Newtonsoft.Json.Linq; // Import the Newtonsoft.Json namespace

public class OTPVerification : MonoBehaviour
{
    public string otpVerificationUrl = "https://ccg.pedagogyshop.com/api/verifying";
    public TMP_InputField phoneInputField;
    public TMP_InputField otpInputField;
    public GameObject login;

    public void VerifyOTP()
    {
        string userId = phoneInputField.text;
        string otp = otpInputField.text;

        StartCoroutine(SendOTPVerificationRequest(userId, otp));
    }

    IEnumerator SendOTPVerificationRequest(string userId, string otp)
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", userId);
        form.AddField("otp", otp);

        using (UnityWebRequest www = UnityWebRequest.Post(otpVerificationUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("OTP verification successful!");

                // Handle the response data as needed
                string responseText = www.downloadHandler.text;
                LoginActivate();
                //HandleOTPVerificationResponse(responseText);
            }
            else
            {
                Debug.LogError("OTP verification failed: " + www.error);
            }
        }
    }

    void LoginActivate()
    {
        login.SetActive(true);
    }
    
}
