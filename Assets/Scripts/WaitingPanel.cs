using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingPanel : MonoBehaviour
{
    [SerializeField] private float WaitTime =1.5f;

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf && CardTurnManager.turnManager.playerControllers.Count >= GameManager.Instance.MaxPlayer)
        {
            Invoke("StartGame", WaitTime);
        }
    }

    private void StartGame()
    {
        CancelInvoke("StartGame");
        for (int i = 0; i < CardTurnManager.turnManager.players.Length; i++)
        {
            SpawnManager.Instance.SetPlayersPose(CardTurnManager.turnManager.players[i].transform,i);
        }

       // CardTurnManager.turnManager.RequestSyncAllPlayerCardRanks();
        CardTurnManager.turnManager.StartTurn();
        this.gameObject.SetActive(false);

       
           
    }
}
