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
        if (tableSize == 1)
        {
            GameObject newplayer1 = Instantiate(playerprefabs, playerPos[0].transform.position, Quaternion.identity, canvas.transform);
        }
        if (tableSize == 2)
        {
            GameObject newplayer1 = Instantiate(playerprefabs, playerPos[0].transform.position, Quaternion.identity, canvas.transform);
            GameObject newplayer2 = Instantiate(playerprefabs, playerPos[1].transform.position, Quaternion.identity, canvas.transform);
        }
        if (tableSize == 3)
        {
            GameObject newplayer1 = Instantiate(playerprefabs, playerPos[0].transform.position, Quaternion.identity, canvas.transform);
            GameObject newplayer2 = Instantiate(playerprefabs, playerPos[1].transform.position, Quaternion.identity, canvas.transform);
            GameObject newplayer3 = Instantiate(playerprefabs, playerPos[2].transform.position, Quaternion.identity, canvas.transform);
        }
        if (tableSize == 4)
        {
            GameObject newplayer1 = Instantiate(playerprefabs, playerPos[0].transform.position, Quaternion.identity, canvas.transform);
            GameObject newplayer2 = Instantiate(playerprefabs, playerPos[1].transform.position, Quaternion.identity, canvas.transform);
            GameObject newplayer3 = Instantiate(playerprefabs, playerPos[2].transform.position, Quaternion.identity, canvas.transform);
            GameObject newplayer4 = Instantiate(playerprefabs, playerPos[3].transform.position, Quaternion.identity, canvas.transform);
            
        }
        if (tableSize == 5)
        {
            GameObject newplayer1 = Instantiate(playerprefabs, playerPos[0].transform.position, Quaternion.identity, canvas.transform);
            GameObject newplayer2 = Instantiate(playerprefabs, playerPos[1].transform.position, Quaternion.identity, canvas.transform);
            GameObject newplayer3 = Instantiate(playerprefabs, playerPos[2].transform.position, Quaternion.identity, canvas.transform);
            GameObject newplayer4 = Instantiate(playerprefabs, playerPos[3].transform.position, Quaternion.identity, canvas.transform);
            GameObject newplayer5 = Instantiate(playerprefabs, playerPos[4].transform.position, Quaternion.identity, canvas.transform);
            
        }
        if (tableSize == 6)
        {
            GameObject newplayer1 = Instantiate(playerprefabs, playerPos[0].transform.position, Quaternion.identity, canvas.transform);
            GameObject newplayer2 = Instantiate(playerprefabs, playerPos[1].transform.position, Quaternion.identity, canvas.transform);
            GameObject newplayer3 = Instantiate(playerprefabs, playerPos[2].transform.position, Quaternion.identity, canvas.transform);
            GameObject newplayer4 = Instantiate(playerprefabs, playerPos[3].transform.position, Quaternion.identity, canvas.transform);
            GameObject newplayer5 = Instantiate(playerprefabs, playerPos[4].transform.position, Quaternion.identity, canvas.transform);
            GameObject newplayer6 = Instantiate(playerprefabs, playerPos[5].transform.position, Quaternion.identity, canvas.transform);
            
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