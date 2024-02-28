using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Card Database", menuName = "Custom/Card Database")]
public class CardDatabase : ScriptableObject
{
    public List<Card> cards;
}
