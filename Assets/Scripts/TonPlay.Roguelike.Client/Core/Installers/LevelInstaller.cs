using TonPlay.Roguelike.Client.Core.Collectables.Config;
using TonPlay.Roguelike.Client.Core.Collectables.Config.Interfaces;
using TonPlay.Roguelike.Client.Core.Enemies.Configs;
using TonPlay.Roguelike.Client.Core.Enemies.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Levels.Config;
using TonPlay.Roguelike.Client.Core.Levels.Config.Interfaces;
using TonPlay.Roguelike.Client.Core.Waves;
using TonPlay.Roguelike.Client.Core.Waves.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Roguelike.Client.Core.Installers
{
	[CreateAssetMenu(fileName = nameof(LevelInstaller), menuName = AssetMenuConstants.CORE_INSTALLERS + nameof(LevelInstaller))]
	public class LevelInstaller : ScriptableObjectInstaller<LevelInstaller>
	{
		[SerializeField]
		private EnemyConfigProvider _enemyConfigProvider;
		
		[SerializeField]
		private LevelWaveConfigProvider _levelWaveConfigProvider;

		[SerializeField]
		private PlayerLevelsConfigProvider _playersLevelsConfigProvider;

		[SerializeField]
		private CollectableConfigProvider _collectablesConfigProvider;

		public override void InstallBindings()
		{
			Container.Bind<IEnemyConfigProvider>().FromInstance(_enemyConfigProvider).AsSingle();
			Container.Bind<ILevelWaveConfigProvider>().FromInstance(_levelWaveConfigProvider).AsSingle();
			Container.Bind<ICollectableConfigProvider>().FromInstance(_collectablesConfigProvider).AsSingle();
			Container.Bind<IPlayersLevelsConfigProvider>().FromInstance(_playersLevelsConfigProvider).AsSingle();
		}
	}
}