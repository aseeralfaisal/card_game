using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public static SpawnManager Instance;

	public Spawnpoint[] spawnpoints;
	void Awake()
	{
		Instance = this;
		//spawnpoints = GetComponentsInChildren<Spawnpoint>();
	}

	public Transform GetSpawnpoint()
	{
		return spawnpoints[Random.Range(0, spawnpoints.Length)].transform;
	}



	public void SetPlayersPose(Transform playerPose,int i)
	{


		PlayerController playerController = playerPose.GetComponent<PlayerController>();

		if (playerController.isMine)
		{

			Debug.Log("SetPlayersPose");
			playerController.targetPose = spawnpoints[0].transform;
			playerPose.position = spawnpoints[0].transform.position;
		}
		else
		{
			playerController.targetPose = spawnpoints[i + 1].transform;
			playerPose.position = spawnpoints[i + 1].transform.position;
		}
	}
}
