using Fusion;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthHandler : NetworkBehaviour
{
	[Networked(OnChanged = nameof(OnHealthChanged))]
	[SerializeField] private float playerHealth { get; set; }

	[Header("Health Properties")]
	[SerializeField] private float maxHealth = 10;

	[Header("References")]
	[SerializeField] private Image healthbarImage;

	[SerializeField] private MeshRenderer playerMesh;

	[SerializeField] private Color originalColor =Color.green;

	[SerializeField] private Color hitColor=Color.gray;

	[SerializeField] private Color invincibleColor = Color.white;

	[Header("Respawn references")]

	[Range(1000,10000)]
	[SerializeField] private int cooldownTimer = 5000;

	[SerializeField] private GameObject playerModel;

	[SerializeField] private PlayerMovementHandler movementHandler;

	[SerializeField] private NetworkRigidbody networkPlayerRb;

	[SerializeField] private GameObject[] spawnPoints;

	[SerializeField] private HitboxRoot playerHitBoxRoot;

	[Header("Respawn UI references")]

	[SerializeField] private Text respawnCooldownText;
	#region Unity Methods

	private void Start()
	{
		spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

		//Only host can handle the life of player
		if (Object.HasStateAuthority)
		{
			playerHealth = maxHealth;
		}
	}

	#endregion

	#region Fusion Callbacks

	public override void Spawned()
	{
		originalColor = playerMesh.material.color;
	}

	#endregion


	#region Networked Property Callbacks

	private static void OnHealthChanged(Changed<PlayerHealthHandler> changed)
	{
		changed.Behaviour.OnHealthChangedRemote();
	}

	#endregion

	private async void OnHealthChangedRemote()
	{
		//At the beginning of the game we need to set max health
		if (playerHealth == maxHealth)
		{
			playerMesh.material.color = originalColor;
			healthbarImage.fillAmount = 1;
			return;
		}

		playerMesh.material.color = hitColor;

		await Task.Delay(250);

		playerMesh.material.color = originalColor;

		healthbarImage.fillAmount = playerHealth / maxHealth;

		if (playerHealth <= 0)
		{
			Respawn();
		}
	}

	private async void Respawn()
	{
		playerModel.SetActive(false);

		playerHitBoxRoot.HitboxRootActive = false;

		movementHandler.enabled = false;

		//Visual show of respawn timer

		respawnCooldownText.gameObject.SetActive(true);

		int timer = cooldownTimer / 1000;
		for (int i = timer; i > 0; i--)
		{
			respawnCooldownText.text = i.ToString();
			await Task.Delay(1000);
		}
		respawnCooldownText.gameObject.SetActive(false);

		///////////
		
		if (Object.HasStateAuthority)
		{
			playerHealth = maxHealth;
			print("respawning object");
			RespawnInDifferentPoint();
		}

		playerMesh.material.color = invincibleColor;

		playerModel.SetActive(true);

		await Task.Delay(2000);

		playerMesh.material.color = originalColor;

		playerHitBoxRoot.HitboxRootActive = true;

		movementHandler.enabled = true;

		if (Object.HasStateAuthority)
		{
			playerHealth = maxHealth;
		}
	}

	public void DecreaseHealth(float damage)
	{
		playerHealth -= damage;
	}

	public float ReturnUserHealth()
	{
		return playerHealth;
	}

	private void RespawnInDifferentPoint()
	{
		int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Length);

		Transform randomSpawnPoint = spawnPoints[randomIndex].transform;

		networkPlayerRb.TeleportToPositionRotation(randomSpawnPoint.position, randomSpawnPoint.rotation);
	}



}
