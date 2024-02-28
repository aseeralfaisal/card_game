using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Deck", menuName = "Custom/Deck")]
public class Deck : ScriptableObject
{
    public static Deck Instance;
    public List<Card> cardsInDeck;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

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

    public Card DrawCard()
    {
        Card drawnCard = cardsInDeck[0];
        cardsInDeck.RemoveAt(0);
        Debug.Log("the Drawn Card "+drawnCard);
        return drawnCard;
        /*if (cardsInDeck.Count > 0)
        {
            //Card drawnCard = cardsInDeck[0];
            //cardsInDeck.RemoveAt(0);
            Debug.Log("card that is drawned "+drawnCard);
            return drawnCard;
        }
        return ;*/
    }
}
