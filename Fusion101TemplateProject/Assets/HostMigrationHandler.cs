using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HostMigrationHandler : MonoBehaviour, INetworkRunnerCallbacks
{
	[SerializeField] private NetworkRunner networkRunnerPrefab;
	private NetworkRunner networkRunner;

	public async void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
	{
		Debug.Log("Host migration started");

		await runner.Shutdown(shutdownReason: ShutdownReason.HostMigration);

		NetworkRunner newRunner = Instantiate(networkRunnerPrefab);

		NetworkSceneManagerDefault sceneManager = newRunner.GetComponent<NetworkSceneManagerDefault>();

		StartGameResult result = await newRunner.StartGame(new StartGameArgs()
		{
			GameMode=hostMigrationToken.GameMode,
			SceneManager = sceneManager,
			HostMigrationToken = hostMigrationToken,
			HostMigrationResume = HostMigrationResume,
		});

		// Check StartGameResult as usual
		if (result.Ok == false)
		{
			Debug.LogWarning(result.ShutdownReason);
		}
		else
		{
			Debug.Log("Done");
		}
	}

	private void HostMigrationResume(NetworkRunner runner)
	{
		foreach (var resumeNO in runner.GetResumeSnapshotNetworkObjects())

			if (
					resumeNO.TryGetBehaviour<NetworkPositionRotation>(out var posRot))
			{

				runner.Spawn(resumeNO, position: posRot.ReadPosition(), rotation: posRot.ReadRotation(), onBeforeSpawned: (runner, newNO) =>
				{
					newNO.CopyStateFrom(resumeNO);

					if (resumeNO.TryGetBehaviour<NetworkBehaviour>(out var myCustomNetworkBehaviour))
					{
						newNO.GetComponent<NetworkBehaviour>().CopyStateFrom(myCustomNetworkBehaviour);
					}
				});
			}
	}





	#region Non-Used Methods

	public void OnConnectedToServer(NetworkRunner runner)
	{
	}

	public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
	{
	}

	public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
	{
	}

	public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
	{
	}

	public void OnDisconnectedFromServer(NetworkRunner runner)
	{
	}

	public void OnInput(NetworkRunner runner, NetworkInput input)
	{
	}

	public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
	{
	}

	public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
	{
	}

	public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
	{
	}

	public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
	{
	}

	public void OnSceneLoadDone(NetworkRunner runner)
	{
	}

	public void OnSceneLoadStart(NetworkRunner runner)
	{
	}

	public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
	{
	}

	public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
	{
	}

	public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
	{
	}

	#endregion

}
