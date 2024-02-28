using UnityEngine;
using Photon.Pun;

public class CardDatabaseManager : MonoBehaviourPun
{
    public CardDatabase cardDatabase;

    void Awake()  
    {
        // Ensure only one instance of CardDatabaseManager exists
        if (FindObjectsOfType<CardDatabaseManager>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            cardDatabase = GameManager.Instance.cardDatabase;
            SyncCardDatabase();
        }
    }

    [PunRPC]
    void SyncCardDatabase()
    {
        if (photonView.IsMine)
        {
            // Synchronize the card database to all players
            photonView.RPC("ReceiveCardDatabase", RpcTarget.Others, JsonUtility.ToJson(cardDatabase));
        }
    }

    [PunRPC]
    void ReceiveCardDatabase(string json)
    {
        // Deserialize the received data and update the card database for all players

        Debug.Log("Deserialize the received data and update the card database for all players");

        JsonUtility.FromJsonOverwrite(json, cardDatabase);
    }
}
