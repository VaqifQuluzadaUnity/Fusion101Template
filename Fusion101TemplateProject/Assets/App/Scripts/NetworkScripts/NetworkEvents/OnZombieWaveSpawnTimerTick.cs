using DynamicBox.EventManagement;

namespace DynamicBox.NetworkEvents
{
	public class OnZombieWaveSpawnTimerTick : GameEvent
	{
		public int SpawnTime;

		public OnZombieWaveSpawnTimerTick(int spawnTime)
		{
			SpawnTime = spawnTime;
		}
	}
}