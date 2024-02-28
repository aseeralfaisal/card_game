using UnityEngine;

public enum CardRank
{
    Zero = 0,Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace 
}
public enum CardSuit
{
    Hearts,
    Diamonds,
    Clubs,
    Spades
}

[CreateAssetMenu(fileName = "New Card", menuName = "Custom/Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public int cardValue;
    public Sprite cardImage;
    public CardSuit suit;
    public CardRank rank;
    private CardData cardData;

    public Card(CardData cardData)
    {
        this.cardData = cardData;
    }

    public CardData GetCardData()
    {
        return new CardData
        {
            cardName = this.cardName,
            cardValue = this.cardValue,
            suit =  suit.ToString(),
            rank = rank.ToString()
        };
    }
}
