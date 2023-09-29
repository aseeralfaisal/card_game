using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Card> hand = new List<Card>();
    public Transform[] spawnPoints;
    public GameObject cardPrefab; // Reference to your card prefab.
    public GameObject[] backsideOfCards;
    
    
    //Player Details
    public TextMeshProUGUI nameText;
    public GameObject avatarImage;
    public TextMeshProUGUI chipText;
    public int chipCount;
    public string userName;

    
    public HandRank EvaluateHand()
    {
        if (IsTrail())
            return HandRank.Trail;

        if (IsPureSequence())
            return HandRank.PureSequence;

        if (IsSequence())
            return HandRank.Sequence;

        if (IsColor())
            return HandRank.Color;

        if (IsPair())
            return HandRank.Pair;

        return HandRank.HighCard;
    }
    

    private bool IsTrail()
    {
        return hand.TrueForAll(card => card.rank == hand[0].rank);
    }

    private bool IsPureSequence()
    {
        return IsSequence() && IsColor();
    }

    private bool IsSequence()
    {
        var sortedRanks = hand.ConvertAll(card => (int)card.rank);
        sortedRanks.Sort();
        return sortedRanks[2] - sortedRanks[1] == 1 && sortedRanks[1] - sortedRanks[0] == 1;
    }

    private bool IsColor()
    {
        return hand.TrueForAll(card => card.suit == hand[0].suit);
    }

    private bool IsPair()
    {
        var cardRanks = new HashSet<CardRank>();
        foreach (var card in hand)
        {
            if (cardRanks.Contains(card.rank))
                return true;

            cardRanks.Add(card.rank);
        }
        return false;
    } 
    

    // Add a card to the player's hand and instantiate it visually.
    public void AddCardToHand(Card card)
    {
        hand.Add(card);
        Debug.Log("Added card to hand: " + card.cardName);
        
        // Instantiate the card visually at the next available spawn point.
        if (spawnPoints.Length > 0)
        {
            Transform spawnPoint = spawnPoints[hand.Count - 1 % spawnPoints.Length];
            GameObject cardObject = Instantiate(cardPrefab, spawnPoint.position, Quaternion.identity);
            cardObject.transform.SetParent(spawnPoint); // Set the card's parent to the spawn point.

            // Set the card's properties using the CardDisplay script.
            CardDisplay cardDisplay = cardObject.GetComponent<CardDisplay>();
            if (cardDisplay != null)
            {
                cardDisplay.SetCard(card);
            }
            else
            {
                Debug.LogWarning("CardDisplay component not found on the card prefab.");
            }
        }
        else
        {
            Debug.LogWarning("No spawn points defined.");
        }
        
    }
    

    // Optionally, implement functions to remove cards, check hand size, and access specific cards as you mentioned in your original code.

    // Remove a card from the hand and destroy its visual representation.
    public void RemoveCardFromHand(int index)
    {
        if (index >= 0 && index < hand.Count)
        {
            Card removedCard = hand[index];
            hand.RemoveAt(index);
            Debug.Log("Removed card from hand: " + removedCard.cardName);

            // Destroy the visual representation of the card.
            Transform cardTransform = transform.GetChild(index);
            if (cardTransform != null)
            {
                Destroy(cardTransform.gameObject);
            }
            else
            {
                Debug.LogWarning("Visual representation not found for the removed card.");
            }
        }
        else
        {
            Debug.LogWarning("Invalid card index.");
        }
    }

    // ...

    // Other functions and properties as needed.
}