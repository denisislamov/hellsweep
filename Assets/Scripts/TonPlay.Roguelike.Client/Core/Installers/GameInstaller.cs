using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Collision;
using TonPlay.Roguelike.Client.Core.Collision.Config;
using TonPlay.Roguelike.Client.Core.Interfaces;
using TonPlay.Roguelike.Client.Core.Player.Configs;
using TonPlay.Roguelike.Client.Core.Player.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Roguelike.Client.Core.Installers
{
	
	[CreateAssetMenu(fileName = nameof(GameInstaller), menuName = AssetMenuConstants.CORE_INSTALLERS + nameof(GameInstaller))]
	public class GameInstaller : ScriptableObjectInstaller<GameInstaller>
	{
		[SerializeField]
		private PlayerConfigProvider _playerConfigProvider;
		
		[SerializeField]
		private WeaponConfigProvider _weaponConfigProvider;
		
		[SerializeField]
		private CollisionConfigProvider _collisionConfigProvider;

		public override void InstallBindings()
		{
			Container.BindFactory<SharedData, SharedData.Factory>().AsSingle();
			Container.BindFactory<EcsWorld, KdTreeStorage, OverlapExecutor, OverlapExecutor.Factory>().AsSingle();
			Container.Bind<IPlayerConfigProvider>().FromInstance(_playerConfigProvider).AsSingle();
			Container.Bind<IWeaponConfigProvider>().FromInstance(_weaponConfigProvider).AsSingle();
			Container.Bind<ICollisionConfigProvider>().FromInstance(_collisionConfigProvider).AsSingle();
		}
	}
}