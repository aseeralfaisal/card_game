using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviourPun
{
    [Header("Card Settings")]
    public CardDatabase cardDatabase;
    public CardDisplay cardPrefab;
    public cardPlayer playerPrefab;
    public GameObject card;
  

    [Header("Player Settings")]
    public List<PlayerController> playerControllers = new List<PlayerController>();
    public Player[] players;
    public Canvas canvas;
    public int tableSize = 6;
    public PlayerSlot[] playerPos;
    public List<cardPlayer> cardPlayers;
    public int playerNo;
    public int MinPlayer;
    public int MaxPlayer = 1;
    int spawnCount = 0;

    [Header("Deck Settings")]
    public List<Card> cardsInDeck;

    [Header("UI Elements")]
    public bool isPressed;

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitDeck();
    }

    [PunRPC]
    public void InitDeck()
    {
        InitializeDeck(cardDatabase);
    }

    private void Update()
    {
        if (isPressed)
        {
            InitDeck();
            isPressed = false;
        }
    }

    #region Deck Management

    public void InitializeDeck(CardDatabase cardDatabase)
    {
        cardsInDeck = new List<Card>(cardDatabase.cards);
        ShuffleDeck();
    }

    public void ShuffleDeck()
    {
        int n = cardsInDeck.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            (cardsInDeck[k], cardsInDeck[n]) = (cardsInDeck[n], cardsInDeck[k]);
        }
    }

    #endregion

    #region Card Drawing

    public Card DrawCard()
    {
        if (cardsInDeck.Count > 34)
        {
            Card drawnCard = cardsInDeck[0];
            cardsInDeck.RemoveAt(0);
            return drawnCard;
        }
        else
        {
            return null; // or handle the situation as needed
        }
    }

    public Card FindDrawCard(CardData cardData)
    {
        Card matchingCard = cardDatabase.cards.Find(x => x.cardName == cardData.cardName
                                                        && x.cardValue == cardData.cardValue
                                                        && x.suit.ToString() == cardData.suit
                                                        && x.rank.ToString() == cardData.rank);

        if (matchingCard != null)
        {
            return matchingCard;
        }
        else
        {
            return null; // or handle the situation as needed
        }
    }

    #endregion

    #region Player Management

    public void SpawnPlayer()
    {
        // Implement player spawning logic here
    }

    #endregion
}
