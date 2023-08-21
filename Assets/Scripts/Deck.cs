using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Deck", menuName = "Custom/Deck")]
public class Deck : ScriptableObject
{
    public List<Card> cardsInDeck;

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
            Card value = cardsInDeck[k];
            cardsInDeck[k] = cardsInDeck[n];
            cardsInDeck[n] = value;
        }
    }

    public Card DrawCard()
    {
        if (cardsInDeck.Count > 0)
        {
            Card drawnCard = cardsInDeck[0];
            cardsInDeck.RemoveAt(0);
            return drawnCard;
        }
        return null;
    }
}
