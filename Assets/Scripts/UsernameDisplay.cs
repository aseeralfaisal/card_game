using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UsernameDisplay : MonoBehaviour
{
	[SerializeField] PhotonView playerPV;
	[SerializeField] TMP_Text text;

	void Start()
	{

		Debug.Log("Photon " + playerPV.Owner.NickName);

		if(playerPV.IsMine)
		{
			gameObject.SetActive(true);
		}

		text.text = playerPV.Owner.NickName;
	}
}
