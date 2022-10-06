using DynamicBox.EventManagement;
using DynamicBox.NetworkEvents;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHandler : NetworkBehaviour
{
	[SerializeField] private Text spawnCountDownText;


	#region Unity Methods

	public override void Despawned(NetworkRunner runner, bool hasState)
	{
		if (Object.HasInputAuthority)
		{
			EventManager.Instance.RemoveListener<OnZombieWaveSpawnTimerTick>(OnZombieWaveSpawnTimerTickHandler);
		}
	}

	#endregion

	public override void Spawned()
	{
		if (Object.HasInputAuthority)
		{
			EventManager.Instance.AddListener<OnZombieWaveSpawnTimerTick>(OnZombieWaveSpawnTimerTickHandler);
		}
	}

	private void OnZombieWaveSpawnTimerTickHandler(OnZombieWaveSpawnTimerTick e)
	{
		

		if (e.SpawnTime == 0)
		{
			spawnCountDownText.gameObject.SetActive(false);
			return;
		}

		spawnCountDownText.gameObject.SetActive(true);

		spawnCountDownText.text = e.SpawnTime.ToString();


	}
}
