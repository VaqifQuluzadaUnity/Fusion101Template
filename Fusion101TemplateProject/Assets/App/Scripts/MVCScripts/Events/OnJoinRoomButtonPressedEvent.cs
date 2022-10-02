using DynamicBox.EventManagement;

namespace DynamicBox.UIEvents
{
	internal class OnJoinRoomButtonPressedEvent : GameEvent
	{
		public string roomName;

		public OnJoinRoomButtonPressedEvent(string roomName)
		{
			this.roomName = roomName;
		}
	}
}