using UnityEngine;
using Photon.Pun;

public class MyGameManager : MonoBehaviourPunCallbacks
{
    public GameObject itemPrefab;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnItems();
        }
    }

    void SpawnItems()
    {
        // Spawn items on the master client
        for (int i = 0; i < 5; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-5f, 5f), 0.5f, Random.Range(-5f, 5f));
            PhotonNetwork.InstantiateSceneObject(itemPrefab.name, spawnPosition, Quaternion.identity);
        }
    }

    // Called when a player joins the room
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // If the master client, send the items to the newly joined player
            SendItemsToPlayer(newPlayer);
        }
    }

    void SendItemsToPlayer(Photon.Realtime.Player player)
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");

        // Iterate through all items and send them to the newly joined player
        foreach (GameObject item in items)
        {
            photonView.RPC("PickUpItem", player, item.GetComponent<PhotonView>().ViewID);
        }
    }

    // RPC method to pick up an item for all players
    [PunRPC]
    void PickUpItem(int itemID)
    {
        PhotonView itemPV = PhotonView.Find(itemID);

        // Check if the item exists and hasn't been picked up
        if (itemPV != null && !itemPV.IsMine)
        {
            itemPV.TransferOwnership(PhotonNetwork.LocalPlayer);
            Destroy(itemPV.gameObject);
        }
    }
}
