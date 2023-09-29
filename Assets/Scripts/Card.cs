using UnityEngine;

public enum CardRank
{
    Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace
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
}
