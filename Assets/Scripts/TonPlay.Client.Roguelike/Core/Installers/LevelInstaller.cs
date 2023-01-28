using TonPlay.Client.Roguelike.Core.Collectables.Config;
using TonPlay.Client.Roguelike.Core.Enemies.Configs;
using TonPlay.Client.Roguelike.Core.Locations;
using TonPlay.Client.Roguelike.Core.Locations.Interfaces;
using TonPlay.Client.Roguelike.Core.Waves;
using TonPlay.Client.Roguelike.Core.Waves.Interfaces;
using TonPlay.Roguelike.Client.Core.Collectables.Config;
using TonPlay.Roguelike.Client.Core.Collectables.Config.Interfaces;
using TonPlay.Roguelike.Client.Core.Enemies.Configs;
using TonPlay.Roguelike.Client.Core.Enemies.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Levels.Config;
using TonPlay.Roguelike.Client.Core.Levels.Config.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace TonPlay.Roguelike.Client.Core.Installers
{
	[CreateAssetMenu(fileName = nameof(LevelInstaller), menuName = AssetMenuConstants.CORE_INSTALLERS + nameof(LevelInstaller))]
	public class LevelInstaller : ScriptableObjectInstaller<LevelInstaller>
	{
		[SerializeField]
		private EnemyConfigProvider _enemyConfigProvider;
		
		[FormerlySerializedAs("_levelWaveConfigProvider")] [SerializeField]
		private LevelEnemyWaveConfigProvider levelEnemyWaveConfigProvider;

		[SerializeField]
		private PlayerLevelsConfigProvider _playersLevelsConfigProvider;

		[SerializeField]
		private CollectableConfigProvider _collectablesConfigProvider;

		[SerializeField]
		private LocationConfigProvider _locationConfigProvider;

		public override void InstallBindings()
		{
			Container.Bind<IEnemyConfigProvider>().FromInstance(_enemyConfigProvider).AsSingle();
			Container.Bind<ILevelEnemyWaveConfigProvider>().FromInstance(levelEnemyWaveConfigProvider).AsSingle();
			Container.Bind<ICollectableConfigProvider>().FromInstance(_collectablesConfigProvider).AsSingle();
			Container.Bind<IPlayersLevelsConfigProvider>().FromInstance(_playersLevelsConfigProvider).AsSingle();
			Container.Bind<ILocationConfigProvider>().FromInstance(_locationConfigProvider).AsSingle();
		}
	}
}