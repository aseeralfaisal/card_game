using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameManager : MonoBehaviour
{
    public CardDatabase cardDatabase;
    public Deck deck;
    public GameObject cardPrefab;
    public GameObject playerprefabs;
    public List<GameObject> cardSlot = new List<GameObject>();
    public Canvas canvas;
    
    //player size
    public int tableSize;
    public GameObject[] playerPos;
    public List<Player> players = new List<Player>();
    public int potAmount;// Updated to use List<Player> for storing players.

    void Start()
    {
        deck.InitializeDeck(cardDatabase);
        //SpawnPlayer();
        DealInitialCards();
        // ... Continue with other initialization as needed ...
    }

    void DealInitialCards()
    {
        // Deal 3 cards to each player's hand
        for (int i = 0; i < 3; i++)
        {
            foreach (Player player in players)
            {
                // Draw a card from the deck
                Card drawnCard = deck.DrawCard();

                if (drawnCard != null)
                {
                    // Add the drawn card to the player's hand
                    player.AddCardToHand(drawnCard);
                }
                else
                {
                    Debug.Log("No more cards in the deck.");
                }
            }
        }
    }

    // Compare hands and determine the winner based on Indian Teen Patti rules
    private Player DetermineWinner()
    {
        Player winner = null;
        HandRank highestHandRank = HandRank.HighCard;

        foreach (Player player in players)
        {
            CardHand playerHand = new CardHand(player.hand);
            HandRank playerHandRank = playerHand.EvaluateHand();

            if (playerHandRank > highestHandRank)
            {
                highestHandRank = playerHandRank;
                winner = player;
            }
        }

        return winner;
    }

    public void EndBettingRoundAndCompareHands()
    {
        Player winner = DetermineWinner();

        if (winner != null)
        {
            // Distribute the pot to the winner
            winner.chipCount += potAmount;
            Debug.Log("Player " + winner.userName + " wins the pot of " + potAmount + " chips!");
        }
        else
        {
            Debug.Log("No winner. Pot carries forward.");
        }

        // Clear the pot for the next round
        potAmount = 0;

        // ... Continue with other actions to end the round and reset for the next round ...
    }

    // ... Other existing functions ...

}
