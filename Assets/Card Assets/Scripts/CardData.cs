using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardData
{
    public string cardName;
    public int cardValue;
    public string suit;
    public string rank;
    public Sprite cardImage;

    // ... other methods

    private static byte[] ConvertSpriteToByteArray(Sprite sprite)
    {
        if (sprite == null)
        {
            return null;
        }

        Texture2D tex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        tex.SetPixels(sprite.texture.GetPixels((int)sprite.textureRect.x,
                                               (int)sprite.textureRect.y,
                                               (int)sprite.textureRect.width,
                                               (int)sprite.textureRect.height));
        tex.Apply();

        return tex.EncodeToPNG();
    }

    private static Sprite ConvertByteArrayToSprite(byte[] byteArray)
    {
        if (byteArray == null)
        {
            return null;
        }

        var tex = new Texture2D(1, 1);
        tex.LoadImage(byteArray);
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
    }

    public Dictionary<string, object> ToDictionary()
    {
        Dictionary<string, object> dict = new Dictionary<string, object>
        {
            { "cardName", cardName },
            { "cardValue", cardValue },
            { "suit", suit },
            { "rank", rank }
        };

        // Check if cardImage is not null before converting to byte array
        if (cardImage != null)
        {
            dict.Add("cardImage", ConvertSpriteToByteArray(cardImage));
        }

        return dict;
    }

    public static CardData FromDictionary(Dictionary<string, object> cardDataDict)
    {
        CardData cardData = new CardData
        {
            cardName = cardDataDict.ContainsKey("cardName") ? (string)cardDataDict["cardName"] : "",
            cardValue = cardDataDict.ContainsKey("cardValue") ? (int)cardDataDict["cardValue"] : 0,
            suit = cardDataDict.ContainsKey("suit") ? cardDataDict["suit"].ToString() : "",
            rank = cardDataDict.ContainsKey("rank") ? cardDataDict["rank"].ToString() : ""
        };

        if (cardDataDict.ContainsKey("cardImage") && cardDataDict["cardImage"] is byte[])
        {
            cardData.cardImage = ConvertByteArrayToSprite((byte[])cardDataDict["cardImage"]);
        }

        return cardData;
    }


    // ... other methods
}
