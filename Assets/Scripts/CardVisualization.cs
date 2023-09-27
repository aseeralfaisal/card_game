using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardVisualization : MonoBehaviour
{
    public Text cardNameText; // Reference to the UI Text component to display the card's name
    public Image cardImage;   // Reference to the UI Image component to display the card's image

    // Set the card's data on the display
    public void SetCard(Card card)
    {
        // Display the card's name
        if (cardNameText != null)
        {
            cardNameText.text = card.cardName;
        }
        else
        {
            Debug.LogWarning("cardNameText not assigned in CardVisualization.");
        }

        // Display the card's image (assuming you have a SpriteRenderer or Image component)
        if (cardImage != null)
        {
            cardImage.sprite = card.cardImage; // Assuming your Card class has a 'cardImage' field of type Sprite
        }
        else
        {
            Debug.LogWarning("cardImage not assigned in CardVisualization.");
        }
    }
}

