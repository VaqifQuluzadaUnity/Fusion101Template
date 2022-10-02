using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;

public class LocalInputHandler : MonoBehaviour,INetworkRunnerCallbacks
{
	private const string HORIZONTAL_INPUT = "Horizontal";

	private const string VERTICAL_INPUT = "Vertical";



	public void OnInput(NetworkRunner runner, NetworkInput input)
	{
		NetworkInputData inputData = new NetworkInputData();

		float horizontalInput = Input.GetAxis(HORIZONTAL_INPUT);
		float verticalInput = Input.GetAxis(VERTICAL_INPUT);

		inputData.movementInput = new Vector3(horizontalInput, 0, verticalInput);

		float mouseX = Input.GetAxis("Mouse X");

		float mouseY = Input.GetAxis("Mouse Y");

		inputData.mouseInput = new Vector2(mouseX,mouseY);

		input.Set(inputData);

	}


	#region Non-Used Interface methods

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

	public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
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
