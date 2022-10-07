using DynamicBox.EventManagement;
using DynamicBox.NetworkEvents;
using DynamicBox.UIEvents;
using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DynamicBox.Controllers
{
	public class FusionNetworkHandler : MonoBehaviour, INetworkRunnerCallbacks
	{
		public static FusionNetworkHandler instance;

		[SerializeField] private NetworkRunner networkRunnerPrefab;

		[SerializeField] private LocalInputHandler localInputHandler;

		[SerializeField] private NetworkSpawner networkSpawner;

		[SerializeField] private NetworkZombieSpawner zombieSpawner;
		
		private NetworkSceneManagerDefault sceneManager;

		public NetworkRunner currentNetworkRunner;

		private NetworkSpawner networkSpawnerInstance;

		private NetworkZombieSpawner zombieSpawnerInstance;

		private byte[] connectionToken;

		private int sessionCount;

		private int currentSceneIndex;

		#region Player Input Properties

		#endregion
		#region Unity Methods


		private void Awake()
		{
			if(instance!=null && instance != this)
			{
				Destroy(instance.gameObject);
			}
			instance = this;
			DontDestroyOnLoad(this);

			connectionToken = Guid.NewGuid().ToByteArray();
		}

		private void Start()
		{
			currentNetworkRunner = Instantiate(networkRunnerPrefab);

			sceneManager = currentNetworkRunner.GetComponent<NetworkSceneManagerDefault>();

			SetupRunnerCallbackListeners(currentNetworkRunner);

			Application.targetFrameRate = 60;

			JoinLobbyAtStartup();
		}



		private void OnEnable()
		{
			EventManager.Instance.AddListener<OnCreateRoomButtonPressedEvent>(OnCreateRoomButtonPressedEventHandler);
			EventManager.Instance.AddListener<OnQuickGameButtonPressedEvent>(OnQuickGameButtonPressedEventHandler);
			EventManager.Instance.AddListener<OnJoinRoomButtonPressedEvent>(OnJoinRoomButtonPressedEventHandler);
		}

		private void OnDisable()
		{
			EventManager.Instance.RemoveListener<OnCreateRoomButtonPressedEvent>(OnCreateRoomButtonPressedEventHandler);

			EventManager.Instance.RemoveListener<OnQuickGameButtonPressedEvent>(OnQuickGameButtonPressedEventHandler);

			EventManager.Instance.RemoveListener<OnJoinRoomButtonPressedEvent>(OnJoinRoomButtonPressedEventHandler);

		}

		#endregion


		#region Event Handlers

		private void OnCreateRoomButtonPressedEventHandler(OnCreateRoomButtonPressedEvent eventDetails)
		{
			JoinLobbyAndStartGame(eventDetails.RoomData);
		}

		private void OnQuickGameButtonPressedEventHandler(OnQuickGameButtonPressedEvent eventDetails)
		{
			JoinRandomRoom(currentNetworkRunner);
		}

		private void OnJoinRoomButtonPressedEventHandler(OnJoinRoomButtonPressedEvent eventDetails)
		{
			JoinRoom(eventDetails.roomName);
		}
		#endregion

		#region Network Runner Callbacks

		public void OnConnectedToServer(NetworkRunner runner)
		{
			print(runner.SessionInfo.Name);
		}

		public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
		{
			runner.Shutdown();
		}

		public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
		{
		}

		public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
		{
		}

		public void OnDisconnectedFromServer(NetworkRunner runner)
		{
			currentNetworkRunner.Shutdown();
		}

		public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
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
			if (!runner.IsServer)
			{
				return;
			}
			if(networkSpawnerInstance==null || zombieSpawnerInstance == null)
			{
				networkSpawnerInstance = runner.Spawn(networkSpawner);

				zombieSpawnerInstance = runner.Spawn(zombieSpawner);
			}

			print("Spawning player");

			StartCoroutine(WaitUntilSceneDone(player));
		}

		public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
		{
			if (runner.IsServer)
			{
				Debug.Log(player);
				EventManager.Instance.Raise(new OnPlayerLeftEvent(player));
			}
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
			print("Session list updated");

			sessionCount = sessionList.Count;

			print(sessionCount);

			EventManager.Instance.Raise(new OnSessionListUpdatedEvent(sessionList));
		}

		public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
		{
			if (shutdownReason == ShutdownReason.HostMigration)
			{
				print("Shut down for host migration");
			}
			else
			{
				SceneManager.LoadScene(0);
			}
		}

		public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
		{
		}

		#endregion

		#region Private Methods

		private async void JoinLobbyAtStartup()
		{
			Task<StartGameResult> task = currentNetworkRunner.JoinSessionLobby(SessionLobby.Custom, "Default");

			await task;

			EventManager.Instance.Raise(new OnConnectedToServerEvent());

			print("Joined lobby " + currentNetworkRunner.LobbyInfo.Name);
		}

		private async Task JoinRoom(string roomName)
		{
			StartGameResult result = await currentNetworkRunner.StartGame
				(new StartGameArgs()
				{
					GameMode = GameMode.Client,
					SessionName = roomName,
					ConnectionToken = FusionNetworkHandler.instance.GetConnectionToken(),
				}
				);

			if (result.Ok)
			{
				print("Join room successful");
			}
			else
			{
				print(result.ShutdownReason);
			}
		}
		private async void CreateRoom(RoomData roomData)
		{
			currentNetworkRunner.ProvideInput = true;

			Dictionary<string, SessionProperty> sessionProperties = new Dictionary<string, SessionProperty>
			{
				{ "pass", roomData.roomPass },
				{"map",roomData.mapSceneIndex}
			};

			currentSceneIndex = roomData.mapSceneIndex;



			StartGameArgs roomCreateParams = new StartGameArgs
			{
				GameMode = roomData.mode,
				SessionName = roomData.roomName,
				CustomLobbyName = roomData.lobbyName,
				Scene = roomData.mapSceneIndex,
				PlayerCount = roomData.roomPlayerCount,
				SceneManager = sceneManager,
				SessionProperties = sessionProperties,
				ConnectionToken = FusionNetworkHandler.instance.GetConnectionToken(),
			};

			await currentNetworkRunner.StartGame(roomCreateParams);

			currentNetworkRunner.SessionInfo.IsVisible = roomData.isVisible;
		}

		private async Task JoinRandomRoom(NetworkRunner runner)
		{
			Dictionary<string, SessionProperty> sessionProperties = new Dictionary<string, SessionProperty>
			{
				{ "pass", "" }
			};


			StartGameResult result = await runner.StartGame(new StartGameArgs()
			{
				GameMode = GameMode.AutoHostOrClient,
				SceneManager = sceneManager,
				//SessionName = "Session" + sessionCount + 1,
				//Scene = 1,
				//PlayerCount = 4,
				SessionProperties = sessionProperties
			}
			);

			if (result.Ok)
			{
				print("Successfully connected to random room");
			}
			else
			{
				print("Connecting to random room failed");
			}

		}

		

		IEnumerator WaitUntilSceneDone(PlayerRef player)
		{
			yield return new WaitUntil(()=> SceneManager.GetSceneByBuildIndex(currentSceneIndex).isLoaded);

			EventManager.Instance.Raise(new OnPlayerJoinedEvent(player));

		}
		#endregion

		#region Public Methods

		public void SetupRunnerCallbackListeners(NetworkRunner runner)
		{
			runner.AddCallbacks(this);
			runner.AddCallbacks(localInputHandler);
		}

		public byte[] GetConnectionToken()
		{
			if (connectionToken == null)
			{
				connectionToken = Guid.NewGuid().ToByteArray();
			}

			return connectionToken;
		}

		public async void JoinLobbyAndStartGame(RoomData roomData)
		{
			Task<StartGameResult> task = currentNetworkRunner.JoinSessionLobby(SessionLobby.Custom, roomData.lobbyName);

			await task;

			print("Joined lobby " + currentNetworkRunner.LobbyInfo.Name);

			CreateRoom(roomData);
		}

		#endregion
	}
}

