using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CardDatabase cardDatabase;
    public Deck deck;
    public GameObject cardPrefab;
    public Transform spawnPoint;
    public Canvas canvas;

    void Start()
    {
        deck.InitializeDeck(cardDatabase);
        DrawCard();
    }

    public void DrawCard()
    {
        // Example: Drawing and spawning the first 5 cards from the deck
        for (int i = 0; i < 3; i++)
        {
            Card drawnCard = deck.DrawCard();
            if (drawnCard != null)
            {
                Debug.Log("Drawn Card: " + drawnCard.cardName);
                SpawnCard(drawnCard);
            }
            else
            {
                Debug.Log("No more cards in the deck.");
            }
        }
    }

    void SpawnCard(Card card)
    {
        GameObject newCard = Instantiate(cardPrefab, spawnPoint.position, Quaternion.identity, canvas.transform);
        CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();

        if (cardDisplay != null)
        {
            cardDisplay.SetCard(card);
        }
        else
        {
            Debug.LogWarning("CardDisplay component not found on the cardPrefab.");
        }
    }
}