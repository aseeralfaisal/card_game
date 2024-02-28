using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class BalanceRequestScript : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    

    public void GetBalanceReqData()
    {
        StartCoroutine(GetBalanceRequestData());
    }

    IEnumerator GetBalanceRequestData()
    {
        // Deposit Number List
        UnityWebRequest depositNumberListRequest = UnityWebRequest.Get("https://ccg.pedagogyshop.com/api/deposit/number/list");

        yield return depositNumberListRequest.SendWebRequest();

        if (depositNumberListRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error retrieving deposit number list: " + depositNumberListRequest.error);
            yield break;
        }

        // Parse JSON response
        string jsonResponse = depositNumberListRequest.downloadHandler.text;
        DepositNumberListResponse depositNumberListResponse = JsonUtility.FromJson<DepositNumberListResponse>(jsonResponse);

        // Display the data in TextMeshPro
        if (depositNumberListResponse != null && depositNumberListResponse.status == 200)
        {
            string resultString = "Deposit Number List:\n";

            foreach (AccountData accountData in depositNumberListResponse.allAccountData)
            {
                resultString += $"Type: {accountData.type}, Number: {accountData.number}, Status: {accountData.status}\n";
            }

            resultText.text = resultString;
        }
        else
        {
            Debug.LogError("Error parsing deposit number list response.");
        }
    }

    // Classes to represent JSON response
    [System.Serializable]
    public class DepositNumberListResponse
    {
        public int status;
        public AccountData[] allAccountData;
    }

    [System.Serializable]
    public class AccountData
    {
        public int id;
        public string type;
        public string number;
        public string status;
        public string created_at;
        public string updated_at;
    }
}
