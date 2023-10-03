using System;
using System.Collections.Generic;
using System.Linq;

public class CardHand
{
    private List<Card> cards;

    public CardHand(List<Card> cards)
    {
        this.cards = cards;
    }

    public HandRank EvaluateHand()
    {
        if (IsTrail())
            return HandRank.Trail;
        if (IsPureSequence())
            return HandRank.PureSequence;
        if (IsSequence())
            return HandRank.Sequence;
        if (IsColor())
            return HandRank.Color;
        if (IsPair())
            return HandRank.Pair;

        return HandRank.HighCard;
    }

    private bool IsTrail()
    {
        return cards.All(card => card.rank == cards[0].rank);
    }

    private bool IsPureSequence()
    {
        return IsSequence() && IsColor();
    }

    private bool IsSequence()
    {
        cards.Sort((x, y) => x.rank.CompareTo(y.rank));
        for (int i = 1; i < cards.Count; i++)
        {
            if (cards[i].rank - cards[i - 1].rank != 1)
                return false;
        }
        return true;
    }

    private bool IsColor()
    {
        return cards.All(card => card.suit == cards[0].suit);
    }

    private bool IsPair()
    {
        var groupedByRank = cards.GroupBy(card => card.rank);
        return groupedByRank.Any(group => group.Count() >= 2);
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
