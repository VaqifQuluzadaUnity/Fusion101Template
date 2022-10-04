using Fusion;
using UnityEngine;

public class PlayerNetworkSetup : NetworkBehaviour
{
	[SerializeField] private Camera playerCam;

	[SerializeField] private MeshRenderer playerModel;

	[SerializeField] private Material enemyMat;

	[SerializeField] private GameObject colliderObject;

	[SerializeField] private Canvas remotePlayerCanvas;

	public override void Spawned()
	{
		if (!Object.HasInputAuthority)
		{
			playerModel.material = enemyMat;
			playerModel.name = "Enemy " + Object.Id;
			playerCam.enabled = false;
			remotePlayerCanvas.enabled = false;
			Destroy(playerCam.GetComponent<AudioListener>());
			return;
		}

		playerModel.name = "Player " + Object.Id;
	}
}
