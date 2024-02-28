using System;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum HandRank
{
    HighCard = 1,
    Pair = 2,
    Color = 3,
    Sequence = 4,
    PureSequence = 5,
    Trail = 6
}
public class cardPlayer : MonoBehaviourPunCallbacks
{
    public List<Card> hand = new List<Card>();
    public List<GameObject> spawnedCards = new List<GameObject>();
    public Transform[] spawnPoints;
    public GameObject cardPrefab; // Reference to your card prefab.
    public GameObject[] backsideOfCards;
    public GameObject canvasPlayer;

    //Player Details
    public TextMeshProUGUI nameText;
    public GameObject avatarImage;
    public TextMeshProUGUI chipText;
    public int chipCount;
    public string userName;
    public HandRank rank;
    public CardRank valueIntial;
    public CardRank valueKicker;
    public CardRank valueFlop;
    PhotonView PV;
    //Ranking details
    public Card highestCard; 

    // Determine the 2nd highest card in the hand
    public Card secondHighestCard;

    // Determine the 3rd highest card in the hand
    public Card thirdHighestCard;

    private PlayerManager playerManager;

    private bool callOnce;
    void Awake()
    {
        
    }
    void Start()
    {
        //if (PV.IsMine)
        //{
            
        //}
    }
    private void Update()
    {
        /*if (spawnedCards.Count > 3)
        {
            //Debug.Log(spawnedCards.Count);
            callOnce = true;
            if (callOnce)
            {
                //DestroyFirstThree();
                callOnce = false;
            }
        }

        if (spawnedCards.Count == 3)
        {
            SetParenter();
        }*/
    }

    void DestroyFirstThree()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject objectToDelete = spawnedCards[i];
            spawnedCards.RemoveAt(i);
            Destroy(objectToDelete);
        }
        SetParenter();
    }

    void SetParenter()
    {
        for (int i = 0; i < 3; i++)
        {
            spawnedCards[i].transform.SetParent(spawnPoints[i]);
            spawnedCards[i].transform.localPosition = Vector3.zero;
            spawnedCards[i].transform.localRotation = Quaternion.identity;
            spawnedCards[i].transform.localScale = Vector3.one;
        }
        
        //spawnedCards[1].transform.SetParent(spawnPoints[1]);
        //spawnedCards[2].transform.SetParent(spawnPoints[2]);
    }



    // Add a card to the player's hand and instantiate it visually.
    public void AddCardToHand(Card card)
    {
        if (card == null)
        {
            Debug.LogWarning("Trying to add a null card to the hand.");
            return;
        }
        hand.Add(card);
        Debug.Log("Added card to hand: " + card.cardName);

        // Instantiate the card visually at the next available spawn point.

        #region Hide these too
        
        if (spawnPoints.Length > 0)
        {
            Transform spawnPoint = spawnPoints[hand.Count - 1 % spawnPoints.Length];
            GameObject cardObject = Instantiate(cardPrefab, spawnPoint.position, Quaternion.identity);

            
            cardObject.transform.SetParent(spawnPoint); // Set the card's parent to the spawn point.
            cardObject.transform.localPosition = Vector3.zero;
            cardObject.transform.localRotation = Quaternion.identity;
            cardObject.transform.localScale = Vector3.one; //new Vector3(1.5f, 1.5f, 1.5f); 
            
            spawnedCards.Add(cardObject);

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


        #endregion
        
        //string theRank= DetermineHandType();
        //Debug.Log("The Rank of this player is "+ theRank);
    }

    public void DetermineHandRank()
    {
      /*  string theRank = DetermineHandType();
        Debug.Log("The Rank of this player is " + theRank);*/
    }

    public HandRank setRank(HandRank theRank)
    {
         return rank = theRank;
    }

    public CardRank setValue(CardRank thevalue)
    {
        return valueIntial = thevalue;
    }
    public CardRank setValueKicker(CardRank thevalue)
    {
        return valueKicker = thevalue;
    }
    public CardRank setValueFlop(CardRank thevalue)
    {
        return valueFlop = thevalue;
    }

    public string GetCardRank()
    {
        return DetermineHandType(); // Assuming DetermineHandType() returns the card rank as a string.
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
        highestCard = hand[hand.Count - 1];

        // Determine the 2nd highest card in the hand
        secondHighestCard = hand[hand.Count - 2];

        // Determine the 3rd highest card in the hand
        thirdHighestCard = hand[hand.Count - 3];

        // Check for Trail (three cards of the same rank)
        if (hand[0].rank == hand[1].rank && hand[1].rank == hand[2].rank)
        {
            
            return "Rank: Trail, Values: , Top: " + highestCard.rank+"Kicker: 0,Flop: 0";
        }
            

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
        {
            setRank(HandRank.PureSequence);
            setValue(highestCard.rank);
            return "Rank: Pure Sequence, Values: ,Top: " + highestCard.rank + "Kicker: 0,Flop: 0"; //+ secondHighestCard.rank + thirdHighestCard.rank;
        }
            

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
        {
            setRank(HandRank.Sequence);
            setValue(highestCard.rank);
            return "Rank: Sequence, Values: ,Top: " + highestCard.rank + "Kicker: 0,Flop: 0" ;// + secondHighestCard.rank + thirdHighestCard.rank;
        }

        // Check for Color (three cards of the same suit)
        if (hand[0].suit == hand[1].suit && hand[1].suit == hand[2].suit)
        {
            setRank(HandRank.Color);
            setValue(highestCard.rank);
            setValueKicker(secondHighestCard.rank);
            setValueFlop(thirdHighestCard.rank);
            return "Rank: Color, Value:  ,Top: " + highestCard.rank + ",Kicker: "+secondHighestCard.rank + ",Flop: "+thirdHighestCard.rank;
        }
            

        // Check for pairs
        if (hand[0].rank == hand[1].rank)
        {
            setRank(HandRank.Pair);
            setValue(hand[0].rank);
            setValueKicker(hand[2].rank);
            return "Rank: Pairs, Value: ,Top: " + hand[0].rank + ",Kicker: "+ hand[2].rank + ",Flop: 0";
        }
            
        if (hand[0].rank == hand[2].rank)
        {
            setRank(HandRank.Pair);
            setValue(hand[0].rank);
            setValueKicker(hand[1].rank);
            return "Rank: Pairs, Value: ,Top: " + hand[0].rank + ",Kicker: "+ hand[1].rank + ",Flop: 0";
        }
            
        if (hand[1].rank == hand[2].rank)
        {
            setRank(HandRank.Pair);
            setValue(hand[1].rank);
            setValueKicker(hand[0].rank);
            return "Rank: Pairs, Values: ,Top: " + hand[1].rank + ",Kicker: "+ hand[0].rank + ",Flop: 0";
        }
            
        
        

        // If none of the above, it's a High Card
        else
        {
            setRank(HandRank.HighCard);
            setValue(highestCard.rank);
            setValueKicker(secondHighestCard.rank);
            setValueFlop(thirdHighestCard.rank);
            
            return "Rank: High Card, Value:  ,Top: " + highestCard.rank + ",Kicker: " + secondHighestCard.rank + ",Flop: " +
                   thirdHighestCard.rank;
        }
        
    }
}