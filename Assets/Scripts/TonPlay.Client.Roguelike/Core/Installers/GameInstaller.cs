using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collectables;
using TonPlay.Client.Roguelike.Core.Collision;
using TonPlay.Client.Roguelike.Core.Collision.Config;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Pooling;
using TonPlay.Client.Roguelike.Core.Skills.Config;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.UI;
using TonPlay.Client.Roguelike.Core.Weapons.Configs;
using TonPlay.Roguelike.Client.Core;
using TonPlay.Roguelike.Client.Core.Collision;
using TonPlay.Roguelike.Client.Core.Collision.Config;
using TonPlay.Roguelike.Client.Core.Player.Configs;
using TonPlay.Roguelike.Client.Core.Player.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace TonPlay.Client.Roguelike.Core.Installers
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

		[SerializeField]
		private SkillConfigProvider _skillConfigProvider;

		[FormerlySerializedAs("_damageText3DView")] [SerializeField]
		private DamageTextView damageTextView;

		public override void InstallBindings()
		{
			Container.BindFactory<SharedData, SharedData.Factory>().AsSingle();
			Container.BindFactory<EcsWorld, KdTreeStorage[], OverlapExecutor, OverlapExecutor.Factory>().AsSingle();
			Container.Bind<IPlayerConfigProvider>().FromInstance(_playerConfigProvider).AsSingle();
			Container.Bind<IWeaponConfigProvider>().FromInstance(_weaponConfigProvider).AsSingle();
			Container.Bind<ICollisionConfigProvider>().FromInstance(_collisionConfigProvider).AsSingle();
			Container.Bind<ISkillConfigProvider>().FromInstance(_skillConfigProvider).AsSingle();

			Container.Bind<ICompositeViewPool>().To<CompositeViewPool>().AsSingle();
			
			Container.BindFactory<ISharedData, CollectablesEntityFactory, CollectablesEntityFactory.Factory>().AsSingle();
			Container.Bind<GoldCollectablesEntityFactory>().To<GoldCollectablesEntityFactory>().AsSingle();
			Container.Bind<ProfileExperienceCollectablesEntityFactory>().To<ProfileExperienceCollectablesEntityFactory>().AsSingle();
			Container.Bind<ExperienceCollectablesEntityFactory>().To<ExperienceCollectablesEntityFactory>().AsSingle();
			Container.Bind<HealthCollectablesEntityFactory>().To<HealthCollectablesEntityFactory>().AsSingle();
			Container.Bind<MagnetCollectablesEntityFactory>().To<MagnetCollectablesEntityFactory>().AsSingle();
			Container.Bind<BombCollectablesEntityFactory>().To<BombCollectablesEntityFactory>().AsSingle();
			Container.BindInterfacesAndSelfTo<SharedDataProvider>().AsSingle();

			Container.Bind<DamageTextView>().FromInstance(damageTextView).AsSingle();
		}
	}
}