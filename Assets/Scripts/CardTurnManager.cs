using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum BettingState
{
    WaitingForBets,
    Folded,
    Called,
    Raised,
    Playing,
}
public enum GameState
{
    NoState,
    IsGameStarted,
    IsGameEnded
}

public class CardTurnManager : MonoBehaviourPun
{
    public static CardTurnManager turnManager;
    public List<PlayerController> playerControllers = new List<PlayerController>();
    public PlayerController[] players;
    private int currentPlayerIndex;
    public chipPanel _chipPrefab;
    [Header("Betting System")] private BettingState currentBettingState;
    public int currentBetAmount;
    public int totalChips =0;
    public chipPanel _chipPanel;
    public int turnCount;

    [Header("Winner Check")] public bool isWinnerCheck;
    public GameState currentGameState;
    public List<PlayerData> playerList = new List<PlayerData>();
    public PlayerController[] sortedPlayerlist;
    public PlayerData winner;
    public List<PlayerData> winnerList = new List<PlayerData>();

    [Header("Winner UI")] public GameObject winPanel;
    public TextMeshProUGUI playerName;

    private bool isGameStarted;

    private int betAmount;

    private void Awake()
    {
        turnManager = this;
        turnCount = 0;
        winPanel.SetActive(false);

        betAmount = TableSetScript.Instance.chipStake;
        foreach (var player in players)
        {
            player.photonView.RPC("updateCurrentAmount", RpcTarget.All);
        }

    }

    private void Update()
    {

        Debug.Log("turn count " + turnCount);
        TriggerWinCheck();
        
    }

    void TriggerWinCheck()
    {
        if (playerControllers.Count == 2)
        {
            if (currentGameState == GameState.IsGameStarted)
            {
                if (turnCount >= 6 ) //for 2 players 3 turns are 18 so do 21
                {
                
                    if (!isWinnerCheck)
                    {
                        CheckWinner();
                        isWinnerCheck = true;

                    }
                }
            }
        }

        if (playerControllers.Count == 3)
        {
            if (currentGameState == GameState.IsGameStarted)
            {
                if (turnCount >= 9 ) //for 2 players 3 turns are 18 so do 21
                {
                
                    if (!isWinnerCheck)
                    {
                        CheckWinner();
                        isWinnerCheck = true;

                    }
                }
            }
            
        }
        if (playerControllers.Count == 4)
        {
            if (currentGameState == GameState.IsGameStarted)
            {
                if (turnCount >= 12 ) //for 2 players 3 turns are 18 so do 21
                {
                
                    if (!isWinnerCheck)
                    {
                        CheckWinner();
                        isWinnerCheck = true;

                    }
                }
            }
            
        }
        if (playerControllers.Count == 5)
        {
            if (currentGameState == GameState.IsGameStarted)
            {
                if (turnCount >= 15 ) //for 2 players 3 turns are 18 so do 21
                {
                
                    if (!isWinnerCheck)
                    {
                        CheckWinner();
                        isWinnerCheck = true;

                    }
                }
            }
            
        }
        if (playerControllers.Count == 6)
        {
            if (currentGameState == GameState.IsGameStarted)
            {
                if (turnCount >= 18 ) //for 2 players 3 turns are 18 so do 21
                {
                
                    if (!isWinnerCheck)
                    {
                        CheckWinner();
                        isWinnerCheck = true;

                    }
                }
            }
            
        }
    }

    private IEnumerator WaitToRestartGame()
    {
        yield return new WaitForSecondsRealtime(5.0f);
        restartTheGame();
    }
    PlayerController[] ReverseArray(PlayerController[] arrayToReverse)
    {
        // Clone the array to avoid modifying it directly
        sortedPlayerlist = (PlayerController[])arrayToReverse.Clone();
        
        for (int i = 0; i < players.Length; i++)
        {
            sortedPlayerlist[i] = players[players.Length - 1 - i];
        }

        return sortedPlayerlist;
    }

    public void CheckWinner()
    {
        PlayerCardRank.Instance.CheckPlayerRanks();
        CopyListValues(PlayerCardRank.Instance.playerDataList, playerList);
        ReverseArray(players);
        
        currentGameState = GameState.IsGameEnded;
        WinningConditions();
        //winPanel.SetActive(true);
        playerName.text = winner.playerName;
        //Debug.Log("Winner is " + winner.playerID);

        photonView.RPC("DeclareWinner", RpcTarget.All, winner.playerName);
        //tartCoroutine(WaitToRestartGame());
    }
    

    [PunRPC]
    private void DeclareWinner(string winnerName)
    {
        // Update UI or perform any other actions to declare the winner
        winPanel.SetActive(true);
        playerName.text = winnerName;
        Debug.Log("Winner is " + winnerName);
        
        PlayerController winnerPlayer = PhotonView.Find(winner.photonViewID)?.GetComponent<PlayerController>();
        winnerPlayer.myChips += totalChips;
        
        StartCoroutine(WaitToRestartGame());
    }
    void WinningConditions()
    {
        int indexer = 0;
        for (int i = 0; i < playerList.Count; i++)
        {
            if (sortedPlayerlist[i].bettingState != BettingState.Folded)
            {
                if (i == 0)
                {
                    winner = playerList[i];
                    winnerList.Add(winner);
                }

                if (playerList[i].rank > winner.rank)
                {
                    winner = playerList[i];
                    winnerList.RemoveAt(indexer);
                    winnerList.Add(playerList[i]);
                }

                if (playerList[i].rank == winner.rank)
                {
                    if (playerList[i].valueIntial > winner.valueIntial)
                    {
                        winner = playerList[i];
                        winnerList.RemoveAt(indexer);
                        winnerList.Add(playerList[i]);
                    }

                    if (playerList[i].valueIntial == winner.valueIntial)
                    {
                        if (playerList[i].valueKicker > winner.valueKicker)
                        {
                            winner = playerList[i];
                            winnerList.RemoveAt(indexer);
                            winnerList.Add(playerList[i]);
                            if (playerList[i].valueKicker == winner.valueKicker)
                            {
                                if (playerList[i].valueFlop > winner.valueFlop)
                                {
                                    winner = playerList[i];
                                    winnerList.RemoveAt(indexer);
                                    winnerList.Add(playerList[i]);

                                    if (playerList[i].valueFlop == winner.valueFlop)
                                    {
                                        winnerList.Add(playerList[i]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    // Method to copy values from one list to another
    void CopyListValues<PlayerData>(List<PlayerData> source, List<PlayerData> destination)
    {
        destination.AddRange(source);
    }


    public void AddPlayerInList(PlayerController playerController)
    {
        PlayerController[] foundControllers = FindObjectsOfType<PlayerController>();
        foreach (PlayerController controller in foundControllers)
        {
            if (!playerControllers.Contains(controller))
            {
                playerControllers.Add(controller);
            }
        }

        players = playerControllers.ToArray();
    }

    public void restartTheGame()
    {
        turnCount = 0;
        isWinnerCheck = false;
        winPanel.SetActive(false);
        totalChips = 0;
        //winner = null;
        winnerList.Clear();
        playerList.Clear();


        GameManager.Instance.InitDeck();
        CardManager._cardManager.ClearCard();
        PlayerCardRank.Instance.ClearAllList();
        
        if (PhotonNetwork.IsMasterClient)
        {
            CardManager._cardManager.InitializeGame();
            GetPlayerRank();
        }
        
        foreach (var player in players)
        {
            player.photonView.RPC("updateCurrentAmount", RpcTarget.All);
        }
        
        //StartTurn();
    }
    
    public void StartTurn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            InitializeGame();
            //StartBettingRound();
            GetPlayerRank();
            //PlayerCardRank.Instance.CheckPlayerRanks();
        }
    }
    private void InitializeGame()
    {
        if (players.Length == 0)
        {
            Debug.LogError("No players found in the array.");
            return;
        }

        // Set the first player's turn
        currentPlayerIndex = 0;
        currentGameState = GameState.IsGameStarted;


        SetTurn(players[currentPlayerIndex].photonView.ViewID);
        //PlayerCardRank.Instance.CheckPlayerRanks();
    }

    public void StartBettingRound()
    {
        currentBettingState = BettingState.WaitingForBets;
        currentBetAmount = 0;
        // Inform players that a new betting round has started (you can use RPC for this)
    }


    [PunRPC]
    private void SetTurn(int playerViewID)
    {
        // Reset turns for all players
        foreach (var player in players)
        {
            player.photonView.RPC("SetTurn", RpcTarget.All, false);
        }
        
        /*foreach (var player in players)
        {
            player.photonView.RPC("updateCurrentAmount", RpcTarget.All);
        }*/
        
        // Set the turn for the specified player
        PlayerController currentPlayer = PhotonView.Find(playerViewID).GetComponent<PlayerController>();
        
        if (currentPlayer.bettingState == BettingState.Folded)
        {
            EndTurn();
        }
        //if called use default stake value or the raised value by previous players to the additonal pot amount
        //if raised raise the stakes amount to a particular raised value to the additional pot amount
        // you can only raise a positive value
        // you can only raise till you have money left for that raise (check money from how much player entered with in room
        // if all in then you give all your money (optional feature)
        // if folded and only one player is in the lobby playing he is the winner
        // if everyone is in call state then we check for winner
        else
        {
            currentPlayer.photonView.RPC("SetTurn", RpcTarget.All, true);
            // Additional turn setup logic can be added here
        }
    }

    public void EndTurn()
    {
        if (winPanel.activeSelf) return;
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
        SetTurn(players[currentPlayerIndex].photonView.ViewID);
    }


    public void PlayerFold()
    {
        // Handle player folding
        // Update UI, adjust betting state, etc.
        players[currentPlayerIndex].photonView.RPC("Fold", RpcTarget.All);
        EndTurn();
    }

    public void PlayerCall()
    {
        // Handle player calling
        // Update UI, adjust betting state, deduct chips, etc.
       // players[currentPlayerIndex].photonView.RPC("Call", RpcTarget.All);
        EndTurn();
    }

    public void PlayerRaise()
    {
        // Handle player raising
        // Update UI, adjust betting state, increase bet amount, deduct chips, etc.
        //players[currentPlayerIndex].photonView.RPC("Raise", RpcTarget.All, amount);\
        PlayerController currentPlayer = GetCurrentPlayer();
        if (currentPlayer != null)
        {
            currentPlayer.photonView.RPC("Raise", RpcTarget.All, betAmount);
            Debug.Log("this current Player");
        }
        else
        {
            Debug.LogError("currentPlayer is null. Unable to perform RPC.");
        }

    }
    public void GetPlayerRank()
    {
        for (int i = 0; i < players.Length; i++)
        {
            PlayerController currentPlayer =
            PhotonView.Find(players[i].photonView.ViewID).GetComponent<PlayerController>();
            currentPlayer.photonView.RPC("SetCardData", RpcTarget.All, i);
           // Debug.Log(currentPlayer.name + "  player Number " + i);
        }
    }


    private PlayerController GetCurrentPlayer()
    {
        for (int i = 0; i < players.Length; i++)
        {
            PlayerController currentPlayer = PhotonView.Find(players[i].photonView.ViewID)?.GetComponent<PlayerController>();

            if (currentPlayer != null && currentPlayer.isMine)
            {
                Debug.Log(currentPlayer.photonView.ViewID + "  player Number " + i);
                currentPlayerIndex = i;
                return currentPlayer;
            }
        }

        // Return null if no player is found
        return null;
    }


    private PlayerController GetPlayerController(Photon.Realtime.Player player)
    {
        GameObject
            playerGameObject = GameObject.Find(player.NickName); // Assuming player.NickName is the GameObject name
        if (playerGameObject != null)
        {
            return playerGameObject.GetComponent<PlayerController>();
        }

        return null;
    }
}