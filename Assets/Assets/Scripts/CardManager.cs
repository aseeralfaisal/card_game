using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CardManager : MonoBehaviourPunCallbacks
{
    [SerializeField] public List<CardData> sharedCardDataList = new List<CardData>();
    public static event System.Action<List<CardData>> OnSharedCardDataUpdated;
    public GameObject cardPrefab;
    public Transform playerCardSpawnPoint;
    public Transform opponentCardSpawnPoint;

    [SerializeField] private PhotonView photonView;
    [SerializeField] private bool gameInitialized;
    
    public static CardManager _cardManager;


    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView == null)
        {
            Debug.LogWarning("PhotonView component not found on CardManager GameObject.");
        }
        
        _cardManager = this;
    }

    private void Start()
    {
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    InitializeGame();
        //}
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && !gameInitialized)
        {
            
            if (GameManager.Instance != null && GameManager.Instance.cardsInDeck.Count >= 1)
            {
                List<Player> players = new List<Player>(PhotonNetwork.PlayerList);
                if (players.Count >= GameManager.Instance.MaxPlayer)
                {
                    Invoke("InitializeGame", 1.0f);
                    // InitializeGame();
                    gameInitialized = true;
                }
            
            }
        
        }
    }

    [PunRPC]
    private void SyncPlayerCardData(string playerId, Dictionary<string, object> cardDataDict)
    {
            
            CardData cardData = CardData.FromDictionary(cardDataDict);
            sharedCardDataList.Add(cardData);
            OnSharedCardDataUpdated?.Invoke(sharedCardDataList);
            Debug.Log("Player ID: "+playerId);
            Debug.Log("Recieving: "+ sharedCardDataList);
            Debug.Log("Recieving: "+ sharedCardDataList.Count);
    }


    [PunRPC]
    public void ClearCardRPC()
    {
        Debug.Log("Cleared Data " );
        /*if (photonView.IsMine)
        {*/
            sharedCardDataList.Clear();
            OnSharedCardDataUpdated?.Invoke(sharedCardDataList);

            // Notify other players about the card list being cleared
           // photonView.RPC("SyncPlayerCardData", RpcTarget.Others, PhotonNetwork.LocalPlayer.NickName, new Dictionary<string, object>());
        //}
    }

    public void ClearCard()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("ClearCardRPC", RpcTarget.All);
        }
    }

    public void InitializeGame()
    {
        
        List<Player> players = new List<Player>(PhotonNetwork.PlayerList);
        foreach (var player in players)
        {
            List<CardData> playerCards = GetPlayerCards(); // Implement your logic to get the cards for each player
            foreach (var cardData in playerCards)
            {
                photonView.RPC("SyncPlayerCardData", player, player.NickName, cardData.ToDictionary());

                Debug.Log("send Card data " + player + " Player Name " + player.NickName);
            }
        }
    }

    public List<CardData> GetSharedCardDataList()
    {
        return sharedCardDataList;
    }

    private List<CardData> GetPlayerCards()
    {
        // Implement your logic to get the cards for each player
        // For now, let's assume you have a method that returns a list of CardData
        List<CardData> cards = new List<CardData>();
        for (int i = 0; i <=2; i++)
        {
                Card newCard = GameManager.Instance.DrawCard();
                CardData cardData = new CardData();
                cardData.cardValue = newCard.cardValue;
                cardData.cardName = newCard.cardName;
                cardData.rank = newCard.rank.ToString();
                cardData.suit = newCard.suit.ToString() ;
                cards.Add(cardData);
        }

        return cards;
    }
}
