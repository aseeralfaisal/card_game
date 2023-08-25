using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI cardValueText;
    public Image cardImage;

    public void SetCard(Card card)
    {
        cardNameText.text = card.cardName;
        cardValueText.text = card.cardValue.ToString();
        cardImage.sprite = card.cardImage;
        // You can also handle suit if needed
    }
}

