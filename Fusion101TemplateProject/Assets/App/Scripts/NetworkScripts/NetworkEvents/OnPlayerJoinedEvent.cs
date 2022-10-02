using DynamicBox.EventManagement;
using Fusion;

namespace DynamicBox.NetworkEvents
{
	public class OnPlayerJoinedEvent : GameEvent
	{
		public PlayerRef player;

		public OnPlayerJoinedEvent(PlayerRef player)
		{
			this.player = player;
		}
	}
}

