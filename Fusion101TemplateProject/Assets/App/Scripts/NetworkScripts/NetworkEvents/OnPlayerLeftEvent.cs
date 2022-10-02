using DynamicBox.EventManagement;
using Fusion;

public class OnPlayerLeftEvent:GameEvent
{
	public PlayerRef playerRef;

	public OnPlayerLeftEvent(PlayerRef playerRef)
	{
		this.playerRef = playerRef;
	}
}