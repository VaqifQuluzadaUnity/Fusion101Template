using Fusion;
using UnityEngine;

public class PlayerShootHandler : NetworkBehaviour
{
	[Networked(OnChanged = nameof(OnShoot))]
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
		if (Runner.TryGetInputForPlayer<NetworkInputData>(Object.InputAuthority, out NetworkInputData data))
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
		LagCompensatedHit hit = new LagCompensatedHit();

		Runner.LagCompensation.Raycast(shootCrosshairTransform.position, shootCrosshairTransform.forward, 10, Object.InputAuthority, out hit, targetLayers);

		if (hit.Hitbox)
		{
			GameObject hitRoot = hit.Hitbox.Root.gameObject;

			switch (hitRoot.tag)
			{
				case "Player":
					PlayerHealthHandler enemyHealthHandler = hitRoot.GetComponent<PlayerHealthHandler>();
					if (enemyHealthHandler.ReturnUserHealth() > 0)
					{
						enemyHealthHandler.DecreaseHealth(1);
					}
					break;

				case "Zombie":
					print("Shooting zombie");
					ZombieNetworkHealthHandler zombieHealthHandler = hitRoot.GetComponent<ZombieNetworkHealthHandler>();

					if (zombieHealthHandler.ReturnZombieHealth() > 0)
					{
						zombieHealthHandler.DecreaseZombieHealth(1);
					}
					break;
			}
		}





		#region Commented Code - Old Shooting logic without Hitbox

		//Ray shootRay = new Ray(shootCrosshairTransform.position,shootCrosshairTransform.forward);

		//RaycastHit hitTarget = new RaycastHit();

		////if (Physics.Raycast(shootRay,out hitTarget, 100,targetLayers))
		////{
		////	print(hitTarget.collider.tag);

		////	switch (hitTarget.collider.tag)
		////	{
		////		case "Player":
		////			hitTarget.collider.GetComponentInParent<PlayerHealthHandler>().DecreaseHealth(1);
		////			break;
		////	}
		////}
		///
		#endregion
	}
	#endregion

}
