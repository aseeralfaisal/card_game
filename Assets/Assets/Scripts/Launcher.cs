using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using System.IO;
using Unity.VisualScripting;

public class Launcher : MonoBehaviourPunCallbacks
{
	public static Launcher Instance;

	[SerializeField] TMP_InputField roomNameInputField;
	[SerializeField] TMP_Text errorText;
	[SerializeField] TMP_Text roomNameText;
	[SerializeField] Transform roomListContent;
	[SerializeField] GameObject roomListItemPrefab;
	[SerializeField] Transform playerListContent;
	[SerializeField] GameObject PlayerListItemPrefab;
	[SerializeField] GameObject startGameButton;
	[SerializeField] private GameObject startTheGameobject;

	[SerializeField] private int roomNumber;
	[SerializeField] private int maxPlayer = 6;
	[SerializeField] private RoomInfo[] roomInfoArray;
	[SerializeField] Player[] players;

	private bool isGameLoad = false;
  void Awake()
    {
        Instance = this;
        PhotonNetwork.AutomaticallySyncScene = true;
        //roomNameInputField.text = LoginManager.Instance.name;
    }

    void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("Joined Lobby-----");

		// Check if there are any available rooms, and if not, create a new room
		//  RoomInfo[] rooms = PhotonNetwork.GetRoomList();

		if (string.IsNullOrEmpty(roomNameInputField.text))
		{
			return;
		}
		PhotonNetwork.NickName = "Player " +roomNameInputField.text;

		if (roomNumber == 0)
        {
            CreateRoom();
        }
        else
        {
			Debug.Log(roomNumber);
            JoinRandomRoom();
        }
    }

	public void JoinOrCreateRoom()
	{

		if (!PhotonNetwork.IsConnected)
		{
			Debug.LogError("Not connected to Photon. Check your connection.");
			return;
		}

		string createRoomName = roomNameInputField.text + UnityEngine.Random.Range(1, 999).ToString();
		string roomName = "";
		bool isRoomFull = true;

		PhotonNetwork.NickName = "Player " + roomNameInputField.text;
		if (string.IsNullOrEmpty(createRoomName))
		{
			errorText.text = "Please enter a room name.";
			MenuManager.Instance.OpenMenu("error");
			return;
		}

		RoomInfo[] rooms = roomInfoArray;
		foreach (RoomInfo room in roomInfoArray)
		{

			int currentPlayers = room.PlayerCount;
			int maxPlayers = room.MaxPlayers;
			
			isRoomFull = currentPlayers == maxPlayers;
			roomName = room.Name;
			Debug.Log("Room Name: " + roomName);
			Debug.Log("Current Players: " + currentPlayers);
			Debug.Log("Max Players: " + maxPlayers);
			Debug.Log("Is Room Full: " + isRoomFull);


		}

		// Use the isRoomFull boolean as needed
		if (!isRoomFull && rooms.Length > 0)
		{
			PhotonNetwork.JoinRoom(roomName);
			MenuManager.Instance.OpenMenu("loading");
			return;

		}
		else
		{
			//Invoke("CreateRoom", 20);
			CreateRoom();
			MenuManager.Instance.OpenMenu("loading");
			return;
		}
	}

	public void CreateRoom()
    {
	
	    RoomOptions roomOptions = new RoomOptions();
	    roomOptions.MaxPlayers = (byte)GameManager.Instance.MaxPlayer;
	    
	    //roomOptions.CustomRoomProperties = (string)GameManager.Instance.MinPlayer;
	    
        PhotonNetwork.CreateRoom("MyRoom "+ Random.RandomRange(10,9999).ToString(), roomOptions, null);
    }

    private void JoinRandomRoom()
    {
	    PhotonNetwork.JoinRandomRoom();
    }
	
	public override void OnJoinedRoom()
	{
		MenuManager.Instance.OpenMenu("room");
		roomNameText.text = PhotonNetwork.CurrentRoom.Name;

		players = PhotonNetwork.PlayerList;
		
		if (players.Length > GameManager.Instance.MaxPlayer)
		{
			CreateRoom();
			return;
		}
		foreach(Transform child in playerListContent)
		{
			Destroy(child.gameObject);
		}

		for(int i = 0; i < players.Count(); i++)
		{
			Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
		}


		
	}


    private void Update()
    {
	
		if (PhotonNetwork.IsMasterClient && !isGameLoad)
		{
	
			players = PhotonNetwork.PlayerList;
			if (players.Length >= GameManager.Instance.MinPlayer && players.Length <= GameManager.Instance.MaxPlayer)
			{
				isGameLoad = true;
				if (PhotonNetwork.IsMasterClient)
				{
					startTheGameobject.SetActive(true);
				}
				
			}
			
		}
	}

    public void StarTheRoom()
    {
	    if (PhotonNetwork.IsMasterClient)
	    {
		    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "GameManager"), Vector3.zero, Quaternion.identity);
		    StartGame();
	    }
	    
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
	{
		
	     startGameButton.SetActive(PhotonNetwork.IsMasterClient);
	
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		errorText.text = "Room Creation Failed: " + message;
		Debug.LogError("Room Creation Failed: " + message);
		MenuManager.Instance.OpenMenu("error");
	}

	public void StartGame()
	{
		PhotonNetwork.LoadLevel(1);
	}

	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
		MenuManager.Instance.OpenMenu("loading");
	}

	public void JoinRoom(RoomInfo info)
	{
		PhotonNetwork.JoinRoom(info.Name);
		MenuManager.Instance.OpenMenu("loading");
	}

	public override void OnLeftRoom()
	{
		MenuManager.Instance.OpenMenu("title");
	}



	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		Debug.Log("Room List " + roomList.Count);
		roomInfoArray = roomList.ToArray();
		if (roomList.Count == 0)
		{
			MenuManager.Instance.OpenMenu("title");

			return;
		}
		Debug.Log("Room List " + roomList.Count);
		roomNumber = roomList.Count;
		foreach (Transform trans in roomListContent)
		{
			Destroy(trans.gameObject);
		}

		for(int i = 0; i < roomList.Count; i++)
		{
			if(roomList[i].RemovedFromList)
				continue;
			Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
		}
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
	}
}