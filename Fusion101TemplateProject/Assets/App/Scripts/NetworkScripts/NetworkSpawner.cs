using Fusion;
using Fusion.Sockets;
using UnityEngine;
using DynamicBox.EventManagement;
using DynamicBox.NetworkEvents;
using System.Collections.Generic;
using System.Linq;

public class NetworkSpawner : NetworkBehaviour
{
	[SerializeField] private NetworkPrefabRef networkPlayerPrefab;

	[SerializeField] private List<GameObject> spawnPoints = new List<GameObject>();

	private Dictionary<PlayerRef, NetworkObject> spawnedCharacters=new Dictionary<PlayerRef, NetworkObject>();

	private NetworkRunner netRunner;


	private void Start()
	{
		spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint").ToList();

	}


	#region Unity Methods

	private void OnEnable()
	{
		EventManager.Instance.AddListener<OnPlayerJoinedEvent>(OnPlayerJoinedEventHandler);

		EventManager.Instance.AddListener<OnPlayerLeftEvent>(OnPlayerLeftEventHandler);
	}

	private void OnDisable()
	{
		EventManager.Instance.RemoveListener<OnPlayerJoinedEvent>(OnPlayerJoinedEventHandler);

		EventManager.Instance.AddListener<OnPlayerLeftEvent>(OnPlayerLeftEventHandler);
	}

	#endregion


	#region Event Handlers

	private void OnPlayerJoinedEventHandler(OnPlayerJoinedEvent e)
	{
		int spawnIndex = spawnedCharacters.Count;

		Transform spawnTransform = spawnPoints[spawnIndex].transform;

		NetworkObject spawnedCharacter = Runner.Spawn(networkPlayerPrefab, spawnTransform.position, spawnTransform.rotation, e.player);

		spawnedCharacters.Add(e.player,spawnedCharacter);
	}

	private void OnPlayerLeftEventHandler(OnPlayerLeftEvent e)
	{
		if(spawnedCharacters.TryGetValue(e.playerRef,out NetworkObject despawnedChar))
		{
			Runner.Despawn(despawnedChar);

			spawnedCharacters.Remove(e.playerRef);
		}
	}
	#endregion
}
