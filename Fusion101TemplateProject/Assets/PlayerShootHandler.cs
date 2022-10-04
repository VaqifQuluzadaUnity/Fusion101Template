using UnityEngine;
using Fusion;
using System;

public class PlayerShootHandler : NetworkBehaviour
{
  [Networked(OnChanged =nameof(OnShoot))]
  [SerializeField] private NetworkBool isShoot { get; set; }

	[SerializeField] private Transform shootCrosshairTransform;

	[SerializeField] private ParticleSystem shootParticle;

	[SerializeField] private LayerMask targetLayers;

	#region Networked Property Callbacks

	private static void OnShoot(Changed<PlayerShootHandler> changed)
	{
		NetworkBool isShoot = changed.Behaviour.isShoot;

		if (isShoot)
		{
			changed.Behaviour.ShootRemotely();
		}

	}

	#endregion

	#region Network Callbacks

	public override void FixedUpdateNetwork()
	{
		if(Runner.TryGetInputForPlayer<NetworkInputData>(Object.InputAuthority,out NetworkInputData data))
		{
			isShoot = data.isShoot;

			if (isShoot)
			{
				print("Player " + Object.Id + " shooted");
				ShootWithRaycast();
			}
		}
	}

	#endregion


	#region Private Methods

	private void ShootRemotely()
	{
		shootParticle.Stop();
		shootParticle.Play();
	}

	private void ShootWithRaycast()
	{
		Ray shootRay = new Ray(shootCrosshairTransform.position,shootCrosshairTransform.forward);

		RaycastHit hitTarget = new RaycastHit();

		if (Physics.Raycast(shootRay,out hitTarget, 100,targetLayers))
		{
			print(hitTarget.collider.tag);

			switch (hitTarget.collider.tag)
			{
				case "Player":
					hitTarget.collider.GetComponentInParent<PlayerHealthHandler>().DecreaseHealth(1);
					break;
			}
		}
	}
	#endregion

}
