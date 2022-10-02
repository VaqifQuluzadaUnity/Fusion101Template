using DynamicBox.EventManagement;
using Fusion;

namespace DynamicBox.UIEvents
{
  public class OnCreateRoomButtonPressedEvent : GameEvent
  {
    public RoomData RoomData;

		public OnCreateRoomButtonPressedEvent(RoomData roomData)
		{
			RoomData = roomData;
		}
	}
}

[System.Serializable]
public class RoomData
{
  public string roomName;

	public string lobbyName;

  public int roomPlayerCount;

  public string roomPass;

	public int mapSceneIndex;

  public bool isVisible;

	public GameMode mode;

	

	public RoomData(string roomName, int roomPlayerCount, string roomPass,int mapSceneIndex, bool isHidden,GameMode mode,string lobbyName)
	{
		this.roomName = roomName;
		this.roomPlayerCount = roomPlayerCount;
		this.roomPass = roomPass;
		this.isVisible = isHidden;
		this.mapSceneIndex = mapSceneIndex;
		this.mode = mode;
		this.lobbyName = lobbyName;
	}
}