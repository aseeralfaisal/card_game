using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySpawnManager : MonoBehaviour
{
	public static MySpawnManager Instance;

    PlayerSlot[] playerSlots;

	void Awake()
	{
		Instance = this;
		playerSlots = GetComponentsInChildren<PlayerSlot>();
	}

	public Transform GetSpawnpoint()
	{
		return playerSlots[Random.Range(0, playerSlots.Length)].transform;
	}
}
