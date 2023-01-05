using TonPlay.Roguelike.Client.Core.Enemies.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Player.Configs.Interfaces;
using TonPlay.Roguelike.Client.Interfaces;

namespace TonPlay.Roguelike.Client.Core
{
	public class SharedData
	{
		public IPlayerSpawnConfigProvider PlayerSpawnConfigProvider { get; }

		public IEnemySpawnConfigProvider EnemySpawnConfigProvider { get; }
		
		public IPositionProvider PlayerPositionProvider { get; private set; }
		
		public SharedData(
			IPlayerSpawnConfigProvider playerSpawnConfigProvider,
			IEnemySpawnConfigProvider enemySpawnConfigProvider)
		{
			PlayerSpawnConfigProvider = playerSpawnConfigProvider;
			EnemySpawnConfigProvider = enemySpawnConfigProvider;
		}

		public void SetPlayerPositionProvider(IPositionProvider positionProvider)
		{
			PlayerPositionProvider = positionProvider;
		}
	}
}