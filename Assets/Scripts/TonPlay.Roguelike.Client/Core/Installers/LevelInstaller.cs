using TonPlay.Roguelike.Client.Core.Enemies.Configs;
using TonPlay.Roguelike.Client.Core.Enemies.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Player.Configs;
using TonPlay.Roguelike.Client.Core.Player.Configs.Interfaces;
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

		public override void InstallBindings()
		{
			Container.Bind<IEnemyConfigProvider>().FromInstance(_enemyConfigProvider).AsSingle();
		}
	}
}