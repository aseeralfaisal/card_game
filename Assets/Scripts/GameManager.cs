using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
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

    void Start()
    {
        deck.InitializeDeck(cardDatabase);
        spawnPlayer();
        //GetAllChildObjects(playerprefabs.transform);
        //DrawCard();
    }

    void spawnPlayer()
    {
        // Instantiate player GameObjects based on tableSize
        GameObject[] players = new GameObject[tableSize];
        for (int i = 0; i < tableSize; i++)
        {
            players[i] = Instantiate(playerprefabs, playerPos[i].transform.position, Quaternion.identity,
                canvas.transform);
        }

        // Deal 3 cards to each player's hand
        for (int i = 0; i < 3; i++)
        {
            foreach (GameObject player in players)
            {
                // Draw a card from the deck
                Card drawnCard = deck.DrawCard();

                if (drawnCard != null)
                {
                    // Get the player's script (you should have a Player script on your playerprefabs)
                    Player playerScript = player.GetComponent<Player>();

                    if (playerScript != null)
                    {
                        // Add the drawn card to the player's hand
                        playerScript.AddCardToHand(drawnCard);
                    }
                    else
                    {
                        Debug.LogWarning("Player script not found on the playerprefabs.");
                    }
                }
                else
                {
                    Debug.Log("No more cards in the deck.");
                }
            }
        }
    }

    void GetAllChildObjects(Transform parent)
    {
        // Iterate through each child of the parent transform.
        foreach (Transform child in parent)
        {
            // Add the child GameObject to the list.
            cardSlot.Add(child.gameObject);
            
            // Recursively call the function for this child to check its children.
            GetAllChildObjects(child);
        }
    }

    public void decideIndex()
    {
        //GetAllChildObjects();
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
        int i;
        for (i = 0; i < 3; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, cardSlot[i].transform.position, Quaternion.identity, canvas.transform);
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
    
    
}