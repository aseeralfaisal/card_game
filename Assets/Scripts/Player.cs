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
        //string theRank= DetermineHandType();
        // Debug.Log("The Rank of this player is "+ theRank);
    }

    public void DetermineHandRank()
    {
        string theRank = DetermineHandType();
        Debug.Log("The Rank of this player is " + theRank);
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

    public string DetermineHandType()
    {
        // Sort the hand based on card ranks (e.g., A, 2, 3, ..., J, Q, K)
        hand.Sort((card1, card2) => card1.rank.CompareTo(card2.rank));

        // Determine the highest card in the hand
        Card highestCard = hand[hand.Count - 1];

        // Determine the 2nd highest card in the hand
        Card secondHighestCard = hand[hand.Count - 2];

        // Determine the 3rd highest card in the hand
        Card thirdHighestCard = hand[hand.Count - 3];

        // Check for Trail (three cards of the same rank)
        if (hand[0].rank == hand[1].rank && hand[1].rank == hand[2].rank)
            return "Trail of " + highestCard.rank;

        // Check for Pure Sequence (consecutive cards of the same suit)
        bool isPureSequence = true;
        for (int i = 0; i < hand.Count - 1; i++)
        {
            if (hand[i].suit != hand[i + 1].suit || hand[i].rank + 1 != hand[i + 1].rank)
            {
                isPureSequence = false;
                break;
            }
        }

        if (isPureSequence)
            return "Pure Sequence: " + highestCard.rank + secondHighestCard.rank + thirdHighestCard.rank;

        // Check for Sequence (consecutive cards of different suits)
        bool isSequence = true;
        for (int i = 0; i < hand.Count - 1; i++)
        {
            if (hand[i].rank + 1 != hand[i + 1].rank)
            {
                isSequence = false;
                break;
            }
        }

        if (isSequence)
            return "Sequence: " + highestCard.rank + secondHighestCard.rank + thirdHighestCard.rank;

        // Check for Color (three cards of the same suit)
        if (hand[0].suit == hand[1].suit && hand[1].suit == hand[2].suit)
            return "Color: " + highestCard.rank + " top color";

        // Check for pairs
        if (hand[0].rank == hand[1].rank || hand[1].rank == hand[2].rank || hand[0].rank == hand[2].rank)
            return "Pairs: " + highestCard.rank;

        // If none of the above, it's a High Card
        return "High Card: " + highestCard.rank + " (2nd: " + secondHighestCard.rank + ", 3rd: " +
               thirdHighestCard.rank + ")";
    }
}