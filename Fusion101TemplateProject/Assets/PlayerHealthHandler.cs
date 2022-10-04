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
	#region Unity Methods

	private void Start()
	{
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

	private void Respawn()
	{
		
	}

	public void DecreaseHealth(float damage)
	{
		playerHealth -= damage;
	}
}
