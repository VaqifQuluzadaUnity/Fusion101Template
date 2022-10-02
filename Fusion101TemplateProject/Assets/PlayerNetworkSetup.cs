using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworkSetup : NetworkBehaviour
{
  [SerializeField] private Camera playerCam;

  [SerializeField] private MeshRenderer playerModel;

  [SerializeField] private Material enemyMat;

	private void Start()
	{
		if (!Object.HasInputAuthority)
		{
			playerModel.material = enemyMat;
			playerModel.name = "Enemy " + Object.Id;
			playerCam.enabled = false;

			Destroy(playerCam.GetComponent<AudioListener>());
			return;
		}

		playerModel.name = "Player " + Object.Id;
	}
}
