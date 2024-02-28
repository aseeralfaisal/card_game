using System;
using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;
using Unity.VisualScripting;

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public string playerID;
    public int photonViewID;
    public HandRank rank;
    public CardRank valueIntial;
    public CardRank valueKicker;
    public CardRank valueFlop;
    public GameState lastState;

    
    // Add other fields as needed
}

public class PlayerCardRank : MonoBehaviourPun
{
    public static PlayerCardRank Instance;

    public static event System.Action<List<String>> OnSharedallCardDataUpdated;
    
    public List<string> allCardData = new List<string>();
    public List<string> playerIDs = new List<string>();

    public string[] splittedString;

    public List<string> rankSubstrings = new List<string>();
    public List<string> valueSubstrings = new List<string>();
    public List<string> topSubstrings = new List<string>();
    public List<string> kickerSubstrings = new List<string>();
    public List<string> flopSubstrings = new List<string>();

    public List<PlayerData> playerDataList = new List<PlayerData>();

    //public string WinnerID;
    public CardTurnManager ctManager;
    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        ctManager = gameObject.GetComponent<CardTurnManager>();
    }

    [PunRPC]
    public void SetCardRankForPlayer(int playerId, string cardRank)
    {
      //  Debug.Log(playerId+" is the playerID");
        if (!allCardData.Contains(cardRank))
        {
            allCardData.Add(cardRank);
            playerIDs.Add(playerId + "");

        }

        photonView.RPC("UpdateCardRanksRPC" , RpcTarget.All, cardRank,playerId);
    }

    public string GetCardRankForPlayer(int playerId)
    {
        return allCardData.Count > playerId ? allCardData[playerId] : "Unknown";
    }

    [PunRPC]
    private void UpdateCardRanksRPC(string updatedCardRanks, int updatedPlayerIDs)
    {
      //  Debug.Log("data --- " + updatedCardRanks);

        if (!allCardData.Contains(updatedCardRanks))
        {
            allCardData.Add(updatedCardRanks);
            playerIDs.Add(updatedPlayerIDs + "");
        }
    }
    
    string[] SplitString(string input, string delimiter)
    {
        // Split the string using the specified delimiter
        string[] substrings = input.Split(delimiter);

        // Return the array of substrings
        return substrings;
    }

    void CreatePlayerList()
    {
        for (int i = 0; i < allCardData.Count; i++)
        {
            SplittingDemo(i);
        }
        
    }

    public void CheckPlayerRanks()
    {
        CreatePlayerList();
    }

    
    [PunRPC]
    public void ClearAllListRPC()
    {
        allCardData.Clear();
        OnSharedallCardDataUpdated?.Invoke(allCardData);
        
        playerIDs.Clear();
        
        Array.Clear(splittedString, 0, splittedString.Length);
        
        rankSubstrings.Clear();
        valueSubstrings.Clear();
        topSubstrings.Clear();
        kickerSubstrings.Clear();
        flopSubstrings.Clear();
        playerDataList.Clear();
        
    }

    public void ClearAllList()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("ClearAllListRPC", RpcTarget.All);
        }
    }
    public void SplittingDemo(int index)
    {
        splittedString = SplitString(allCardData[index], ",");
        rankSubstrings.Add(splittedString[0]);
        valueSubstrings.Add(splittedString[1]);
        topSubstrings.Add(splittedString[2]);
        kickerSubstrings.Add(splittedString[3]);
        flopSubstrings.Add(splittedString[4]);
        AddPlayerInfos(index);

    }

    public void AddPlayerInfos(int index)
    {
        PlayerData player = new PlayerData();
        player.playerName = playerIDs[index];
        player.playerID =  playerIDs[index];
        player.photonViewID = int.Parse(playerIDs[index]);
        SetRankings(player, index);
        SetInitialValue(player,index);
        SetKickerValue(player,index);
        SetFlopValue(player,index);
        playerDataList.Add(player);
    }

    public void SetRankings(PlayerData player,int index)
    {
        if (rankSubstrings[index] == "Rank: Trail")
        {
            player.rank = HandRank.Trail;
        }
        if (rankSubstrings[index] == "Rank: Pure Sequence")
        {
            player.rank = HandRank.PureSequence;
        }
        if (rankSubstrings[index] == "Rank: Sequence")
        {
            player.rank = HandRank.Sequence;
        }
        if (rankSubstrings[index] == "Rank: Color")
        {
            player.rank = HandRank.Color;
        }
        if (rankSubstrings[index] == "Rank: Pairs")
        {
            player.rank = HandRank.Pair;
        }
        if (rankSubstrings[index] == "Rank: High Card")
        {
            player.rank = HandRank.HighCard;
        }
    }

    public void SetInitialValue(PlayerData player, int index)
    {
        if (topSubstrings[index] == "Top: Two")
        {
            player.valueIntial = CardRank.Two;
        }
        if (topSubstrings[index] == "Top: Three")
        {
            player.valueIntial = CardRank.Three;
        }
        if (topSubstrings[index] == "Top: Four")
        {
            player.valueIntial = CardRank.Four;
        }
        if (topSubstrings[index] == "Top: Five")
        {
            player.valueIntial = CardRank.Five;
        }
        if (topSubstrings[index] == "Top: Six")
        {
            player.valueIntial = CardRank.Six;
        }
        if (topSubstrings[index] == "Top: Seven") 
        {
            player.valueIntial = CardRank.Seven;
        }
        if (topSubstrings[index] == "Top: Eight") 
        {
            player.valueIntial = CardRank.Eight;
        }
        if (topSubstrings[index] == "Top: Nine") 
        {
            player.valueIntial = CardRank.Nine;
        }
        if (topSubstrings[index] == "Top: Ten") 
        {
            player.valueIntial = CardRank.Ten;
        }
        if (topSubstrings[index] == "Top: Jack") 
        {
            player.valueIntial = CardRank.Jack;
        }
        if (topSubstrings[index] == "Top: Queen") 
        {
            player.valueIntial = CardRank.Queen;
        }
        if (topSubstrings[index] == "Top: King") 
        {
            player.valueIntial = CardRank.King;
        }
        if (topSubstrings[index] == "Top: Ace") 
        {
            player.valueIntial = CardRank.Ace;
        }
        
    }

    public void SetKickerValue(PlayerData player, int index)
    {
        if (kickerSubstrings[index] == "Kicker: Two")
        {
            player.valueKicker = CardRank.Two;
        }
        if (kickerSubstrings[index] == "Kicker: Three")
        {
            player.valueKicker = CardRank.Three;
        }
        if (kickerSubstrings[index] == "Kicker: Four")
        {
            player.valueKicker = CardRank.Four;
        }
        if (kickerSubstrings[index] == "Kicker: Five")
        {
            player.valueKicker = CardRank.Five;
        }
        if (kickerSubstrings[index] == "Kicker: Six")
        {
            player.valueKicker = CardRank.Six;
        }
        if (kickerSubstrings[index] == "Kicker: Seven") 
        {
            player.valueKicker = CardRank.Seven;
        }
        if (kickerSubstrings[index] == "Kicker: Eight") 
        {
            player.valueKicker = CardRank.Eight;
        }
        if (kickerSubstrings[index] == "Kicker: Nine") 
        {
            player.valueKicker = CardRank.Nine;
        }
        if (kickerSubstrings[index] == "Kicker: Ten") 
        {
            player.valueKicker = CardRank.Ten;
        }
        if (kickerSubstrings[index] == "Kicker: Jack") 
        {
            player.valueKicker = CardRank.Jack;
        }
        if (kickerSubstrings[index] == "Kicker: Queen") 
        {
            player.valueKicker = CardRank.Queen;
        }
        if (kickerSubstrings[index] == "Kicker: King") 
        {
            player.valueKicker = CardRank.King;
        }
        if (kickerSubstrings[index] == "Kicker: Ace") 
        {
            player.valueKicker = CardRank.Ace;
        }
        if (kickerSubstrings[index] == "Kicker: 0")
        {
            player.valueKicker = CardRank.Zero;
        }
        
    }

    public void SetFlopValue(PlayerData player, int index)
    {
        if (flopSubstrings[index] == "Flop: Two")
        {
            player.valueFlop = CardRank.Two;
        }
        if (flopSubstrings[index] == "Flop: Three")
        {
            player.valueFlop = CardRank.Three;
        }
        if (flopSubstrings[index] == "Flop: Four")
        {
            player.valueFlop = CardRank.Four;
        }
        if (flopSubstrings[index] == "Flop: Five")
        {
            player.valueFlop = CardRank.Five;
        }
        if (flopSubstrings[index] == "Flop: Six")
        {
            player.valueFlop = CardRank.Six;
        }
        if (flopSubstrings[index] == "Flop: Seven") 
        {
            player.valueFlop = CardRank.Seven;
        }
        if (flopSubstrings[index] == "Flop: Eight") 
        {
            player.valueFlop = CardRank.Eight;
        }
        if (flopSubstrings[index] == "Flop: Nine") 
        {
            player.valueFlop = CardRank.Nine;
        }
        if (flopSubstrings[index] == "Flop: Ten") 
        {
            player.valueFlop = CardRank.Ten;
        }
        if (flopSubstrings[index] == "Flop: Jack") 
        {
            player.valueFlop = CardRank.Jack;
        }
        if (flopSubstrings[index] == "Flop: Queen") 
        {
            player.valueFlop = CardRank.Queen;
        }
        if (flopSubstrings[index] == "Flop: King") 
        {
            player.valueFlop = CardRank.King;
        }
        if (flopSubstrings[index] == "Flop: Ace") 
        {
            player.valueFlop = CardRank.Ace;
        }

        if (flopSubstrings[index] == "Flop: 0")
        {
            player.valueFlop = CardRank.Zero;
        }
    }
    
    
    
    
}
