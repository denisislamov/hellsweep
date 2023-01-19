using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using TonPlay.Client.Roguelike.Interfaces;
using TonPlay.Roguelike.Client.Core.Collectables.Config.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.Config;
using TonPlay.Roguelike.Client.Core.Enemies.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Levels.Config.Interfaces;
using TonPlay.Roguelike.Client.Core.Models.Interfaces;
using TonPlay.Roguelike.Client.Core.Player.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Pooling;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Core.Skills.Config.Interfaces;
using TonPlay.Roguelike.Client.Core.Waves.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using Zenject;

namespace TonPlay.Client.Roguelike.Core
{
	public class SharedData : ISharedData
	{
		public IPlayerConfigProvider PlayerConfigProvider { get; }

		public IEnemyConfigProvider EnemyConfigProvider { get; }
		
		public IWeaponConfigProvider WeaponConfigProvider { get; }
		
		public ICollisionConfigProvider CollisionConfigProvider { get; }

		public IGameModel GameModel { get; }

		public IPositionProvider PlayerPositionProvider { get; private set; }
		
		public string PlayerWeaponId { get; private set; }
		
		public ICompositeViewPool CompositeViewPool { get; }
		
		public ICollectableConfigProvider CollectablesConfigProvider { get; }

		public ILevelWaveConfigProvider WavesConfigProvider { get; }
		
		public ISkillConfigProvider SkillsConfigProvider { get; }
		
		public IPlayersLevelsConfigProvider PlayersLevelsConfigProvider { get; }

		public SharedData(
			IPlayerConfigProvider playerConfigProvider,
			IEnemyConfigProvider enemyConfigProvider,
			IWeaponConfigProvider weaponConfigProvider,
			IGameModelProvider gameModelProvider, 
			ICollisionConfigProvider collisionConfigProvider, 
			ILevelWaveConfigProvider wavesConfigProvider, 
			ICollectableConfigProvider collectablesConfigProvider, 
			ISkillConfigProvider skillsConfigProvider, 
			IPlayersLevelsConfigProvider playersLevelsConfigProvider)
		{
			PlayerConfigProvider = playerConfigProvider;
			EnemyConfigProvider = enemyConfigProvider;
			WeaponConfigProvider = weaponConfigProvider;
			CollisionConfigProvider = collisionConfigProvider;
			WavesConfigProvider = wavesConfigProvider;
			CollectablesConfigProvider = collectablesConfigProvider;
			SkillsConfigProvider = skillsConfigProvider;
			PlayersLevelsConfigProvider = playersLevelsConfigProvider;
			GameModel = gameModelProvider.Get();
			CompositeViewPool = new CompositeViewPool();
		}

		public void SetPlayerPositionProvider(IPositionProvider positionProvider)
		{
			PlayerPositionProvider = positionProvider;
		}

		public void SetPlayerWeapon(string weaponId)
		{
			PlayerWeaponId = weaponId;
		}

		public class Factory : PlaceholderFactory<SharedData>
		{
		}
	}
}