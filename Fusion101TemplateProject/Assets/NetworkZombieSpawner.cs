using DynamicBox.EventManagement;
using DynamicBox.NetworkEvents;
using Fusion;
using UnityEngine;

public class NetworkZombieSpawner : NetworkBehaviour
{
	[SerializeField] private Transform[] zombieSpawnPoints=new Transform[0];

	[SerializeField] private NetworkPrefabRef[] zombiePrefabs;

	[Range(1, 10)]
	[SerializeField] private float minSpawnTime=5;

	[Range(11, 20)]
	[SerializeField] private float maxSpawnTime=11;

	private TickTimer zombieSpawnTimer { get; set; }

	private TickTimer zombieWaveTimer { get; set; }

	[Networked(OnChanged =nameof(OnWaveSpawnTimeChanged))]
	[SerializeField] private int spawnTime { get; set; }
	public override void Spawned()
	{
		if (Object.HasStateAuthority)
		{
			zombieWaveTimer = TickTimer.CreateFromSeconds(Runner, 12);
		}
	}

	public override void FixedUpdateNetwork()
	{

		if (!Object.HasStateAuthority)
		{
			return;
		}

		if (!zombieWaveTimer.ExpiredOrNotRunning(Runner))
		{
			print("Zombie Wave timer not ended");

			spawnTime =(int)zombieWaveTimer.RemainingTime(Runner);
			return;
		}

		SpawnZombie();
	}

	private void SpawnZombie()
	{
		if (!zombieSpawnTimer.ExpiredOrNotRunning(Runner))
		{
			return;
		}

		// Define spawn pos
		int randomSpawnIndex = Random.Range(0,zombieSpawnPoints.Length);

		Vector3 spawnPos = zombieSpawnPoints[randomSpawnIndex].position;

		Quaternion spawnRot = zombieSpawnPoints[randomSpawnIndex].rotation;


		//Define zombie prefab
		int randomZombieIndex = Random.Range(0, zombiePrefabs.Length);

		NetworkPrefabRef randomZombiePrefab = zombiePrefabs[randomZombieIndex];

		NetworkObject spawnedZombie = Runner.Spawn(randomZombiePrefab,spawnPos,spawnRot,PlayerRef.None);

		float spawnTimer = Random.Range(minSpawnTime,maxSpawnTime);

		zombieSpawnTimer = TickTimer.CreateFromSeconds(Runner,spawnTimer);
	}


	private static void OnWaveSpawnTimeChanged(Changed<NetworkZombieSpawner> changed)
	{
		changed.Behaviour.OnWaveSpawnTimeChangedRemotely();
	}


	private void OnWaveSpawnTimeChangedRemotely()
	{
		EventManager.Instance.Raise(new OnZombieWaveSpawnTimerTick(spawnTime));
	}


}
