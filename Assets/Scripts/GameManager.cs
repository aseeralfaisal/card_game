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
    public GameObject[] players;

    void Start()
    {
        deck.InitializeDeck(cardDatabase);
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        // Instantiate player GameObjects based on tableSize
        players = new GameObject[tableSize];
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

        for (int i = 0; i < tableSize; i++)
        {
            string theRank= players[i].GetComponent<Player>().DetermineHandType();
            Debug.Log("The Rank of player" + i + " is "+ theRank);
            
        }
        
    }
    





    

    
    
    
}