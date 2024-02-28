using System;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks //, IDamageable
{
    [Header("Player UI")] [SerializeField] private CardFillController m_cardFillController;
    [Header("Player INFO")] [SerializeField]


    public List<CardData> cardDataList = new List<CardData>();
    [SerializeField] callbtn _callbtn;
    [SerializeField] public cardPlayer m_cardPlayer;
    [SerializeField] private PlayerController playerController;

    [Header("Transform INFO")] public Transform targetPose;
    int itemIndex;
    int previousItemIndex = -1;


    PhotonView PV;
    PlayerManager playerManager;

    //public static PlayerController playerController;
    public static PlayerController Instance { get; internal set; }

    public string playerName;
    public bool isTurn;
    public bool isMine;
    
    public BettingState bettingState;

    [Header("Chips Count")]
    public int myChips;
    public int boardAmount;
    public int bettedAmount;
    public TextMeshProUGUI chipCount;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        PV = GetComponent<PhotonView>();

        isMine = PV.IsMine;
        if (isMine)
        {
            _callbtn = FindObjectOfType<callbtn>();
            string balanceString = LoginManager.Instance.balance;
            int.TryParse(LoginManager.Instance.balance, out myChips);
        }
    }
    
    

    void Start()
    {
        //myChips = 10000; //take this from the profile of player
        //boardAmount = 100; //this will be decided from room options
        if (isMine)
        {
            string balance = LoginManager.Instance.balance;
            myChips = int.Parse(balance);
            
            // Update UI with the initial currency amount
            photonView.RPC("updateCurrentAmount", RpcTarget.All);
            //chipCount.text = ""+myChips;
        }
        CardManager.OnSharedCardDataUpdated += UpdateCardDataList;
    }

    private void LateUpdate()
    {
        //chipCount.text = ""+myChips;
    }
    
    [PunRPC]
    public void UpdateCurrency(int newCurrencyAmount)
    {
        myChips = newCurrencyAmount;
        photonView.RPC("updateCurrentAmount", RpcTarget.All);
    }


    private void UpdateCardDataList(List<CardData> updatedCardDataList)
    {
        
        CardTurnManager.turnManager.AddPlayerInList(playerController);
        if (!PV.IsMine) return;
        m_cardPlayer.hand.Clear();

        if (m_cardPlayer.spawnedCards.Count != 0)
        {
            for (int i = 0; i < 3; i++)
            {
                Destroy(m_cardPlayer.spawnedCards[i]);
            }
        }
        
        
        m_cardPlayer.spawnedCards.Clear();
        
        //Debug.Log("Recive Data CardData "+updatedCardDataList +"  isMaster "+ PhotonNetwork.MasterClient);
        
        cardDataList = updatedCardDataList;
        if (cardDataList.Count == 3)
        {
            Debug.Log("cardDataList: " + cardDataList.Count);
            foreach (CardData cardData in cardDataList)
            {
                DealCards(m_cardPlayer, cardData);
            }
        }
    }
    
    [PunRPC]
    private void ClearCards()
    {
        
        //cardDataList.Clear(); 
        //m_cardPlayer.hand.Clear();
        
    }

    


    public void DealCards(cardPlayer cardPlayer, CardData cardData)
    {
        Card cards = GameManager.Instance.FindDrawCard(cardData);
        cardPlayer.AddCardToHand(cards);
    }


    [PunRPC]
    public void SetCardData(int i)
    {
        if (!PV.IsMine) return;
        PlayerCardRank.Instance.SetCardRankForPlayer(photonView.ViewID, m_cardPlayer.GetCardRank());
    }

    [PunRPC]
    public void updateCurrentAmount()
    {
        chipCount.text = ""+ myChips;
    }


    [PunRPC]
    public void SetTurn(bool isTurn)
    {
      //  if (!PV.IsMine) return;
        this.isTurn = isTurn;
        

        // Additional turn-related logic can be added here
       

        if (this.isTurn)
        {
            Debug.Log(playerController.photonView.ViewID + "'s turn has ended their turn.");
            CardTurnManager.turnManager.turnCount++;
            this.m_cardFillController.StartCount();
        }
    }


    [PunRPC]
    public void PlayerRaise(int amount)
    {
        m_cardFillController.FillTotal();
    }




    void Update()
    {
        if (targetPose == null) return;
    }

    void EquipItem(int _index)
    {
        if (_index == previousItemIndex)
            return;

        itemIndex = _index;
        previousItemIndex = itemIndex;

        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("itemIndex") && !PV.IsMine && targetPlayer == PV.Owner)
        {
            EquipItem((int)changedProps["itemIndex"]);
        }
    }

    void FixedUpdate()
    {
        if (!PV.IsMine)
            return;

          _callbtn.gameObject.SetActive(isTurn);
          
    }

    #region Betting System

    [PunRPC]
    public void Fold()
    {
        bettingState = BettingState.Folded;
        
        if (this.isTurn)
        {
            CardTurnManager.turnManager.turnCount++;
        }
        // Handle player folding
        // Update UI, adjust betting state, etc.

        // You can leave the method empty or add additional logic as needed.
    }

    [PunRPC]
    public void Call()
    {
        bettedAmount = 0;
        bettingState = BettingState.Called;
        myChips -= boardAmount;
        bettedAmount += boardAmount;
    }

    [PunRPC]
    public void Raise(int amount)
    {
        bettingState = BettingState.Raised;

        chipPanel chip = Instantiate(CardTurnManager.turnManager._chipPrefab, transform.position, Quaternion.identity);
        /*CardTurnManager.turnManager._chipPrefab.transform.position = transform.position;
        CardTurnManager.turnManager._chipPrefab.gameObject.SetActive(true);*/
        //chip.chipTxt.text = "" + amount;
        myChips -= amount;
        chip.AddChips(amount);

        if (isMine)
        {
            Debug.Log("bettingState  " + amount);
            photonView.RPC("UpdateCurrency", RpcTarget.All, myChips);
            CardTurnManager.turnManager.EndTurn();
        }
        
        // Invoke the method to update currency and UI
        

     

        // Handle player raising
        // Update UI, adjust betting state, increase bet amount, deduct chips, etc.

        // You can leave the method empty or add additional logic as needed.
    }

    #endregion
 
}