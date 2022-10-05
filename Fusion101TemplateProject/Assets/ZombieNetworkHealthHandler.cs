using Fusion;
using UnityEngine.UI;
using UnityEngine;
public class ZombieNetworkHealthHandler : NetworkBehaviour
{
	[Networked(OnChanged =nameof(OnZombieHealthChanged))]
	[SerializeField] private float currentHealth { get; set; }

	[SerializeField] private float maxHealth = 10;

	[SerializeField] private Image zombieHealthbar;

	private void Start()
	{
		currentHealth = maxHealth;
	}

	#region Networked Property Callbacks

	private static void OnZombieHealthChanged(Changed<ZombieNetworkHealthHandler> changed)
	{
		changed.Behaviour.OnZombieHealthChanged();
	}

	private void OnZombieHealthChanged()
	{
		zombieHealthbar.fillAmount = currentHealth / maxHealth;
	}

	public void DecreaseZombieHealth(int damage)
	{
		currentHealth -= damage;

		if (currentHealth <= 0)
		{
			Runner.Despawn(Object);
		}
	}

	public float ReturnZombieHealth()
	{
		return currentHealth;
	}
	#endregion


}
