using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CardDatabase cardDatabase;
    public Deck deck;

    void Start()
    {
        deck.InitializeDeck(cardDatabase);
        // Example: Drawing and printing the first 5 cards from the deck
        for (int i = 0; i < 5; i++)
        {
            Card drawnCard = deck.DrawCard();
            if (drawnCard != null)
            {
                Debug.Log("Drawn Card: " + drawnCard.cardName);
            }
            else
            {
                Debug.Log("No more cards in the deck.");
            }
        }
    }
}
