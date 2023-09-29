using System.Collections.Generic;

public class Hand
{
    public List<Card> Cards { get; private set; }

    public Hand(List<Card> cards)
    {
        Cards = cards;
    }

    public HandRank EvaluateHand()
    {
        // Implement hand evaluation based on your game's rules
        // This is a placeholder; modify as needed
        return HandRank.HighCard;
    }
}

public enum HandRank
{
    HighCard,
    Pair,
    Color,
    Sequence,
    PureSequence,
    Trail
}