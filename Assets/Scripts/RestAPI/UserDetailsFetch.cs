using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;

public class UserDetailsFetch: MonoBehaviour
{
    public TextMeshProUGUI userNameText;
    public TextMeshProUGUI emailText;
    public TextMeshProUGUI phoneText;
    public TextMeshProUGUI balanceText;

    private string userPhoneNumber;
    
    public static UserDetailsFetch Instance;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
        }
    }

    private void Start()
    {
        RefreshUserDetails();
    }

    public void RefreshUserDetails()
    {
        userPhoneNumber = LoginManager.Instance.phoneNumber;
        StartCoroutine(GetAndDisplayUserDetails());
    }

    IEnumerator GetAndDisplayUserDetails()
    {
        string apiUrl = "https://ccg.pedagogyshop.com/api/user/details/" + userPhoneNumber;

        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                DisplayUserDetails(request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Failed to get user details. Error: " + request.error);
            }
        }
    }

    void DisplayUserDetails(string jsonResponse)
    {
        // Parse the JSON response
        UserDetails userDetails = JsonUtility.FromJson<UserDetails>(jsonResponse);

        // Display the user details in TextMeshPro
        if (userDetails != null)
        {
            userNameText.text = "User Name: " + userDetails.userName;
            emailText.text = "Email: " + userDetails.email;
            phoneText.text = "Phone: " + userDetails.phone;
            balanceText.text = "Balance: " + userDetails.balance;
            LoginManager.Instance.balance = userDetails.balance;
            LoginManager.Instance.userName = userDetails.userName;
        }
        else
        {
            Debug.LogError("Failed to parse user details from the response.");
        }
    }

    // Class to represent the user details structure
    [System.Serializable]
    public class UserDetails
    {
        public int status;
        public string userName;
        public string email;
        public string phone;
        public string nid;
        public string pic;
        public string balance;
    }
}
