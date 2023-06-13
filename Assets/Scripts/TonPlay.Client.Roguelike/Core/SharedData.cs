using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collectables.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Enemies.Configs.Interfaces;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using TonPlay.Client.Roguelike.Core.Player.Configs.Interfaces;
using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.UI;
using TonPlay.Client.Roguelike.Core.Waves.Interfaces;
using TonPlay.Client.Roguelike.Interfaces;
using TonPlay.Roguelike.Client.Core;
using TonPlay.Roguelike.Client.Core.Collision.Config;
using TonPlay.Roguelike.Client.Core.Levels.Config.Interfaces;
using TonPlay.Roguelike.Client.Core.Pooling;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using UnityEngine;
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

		public ILevelEnemyWaveConfigProvider EnemyWavesConfigProvider { get; }

		public ISkillConfigProvider SkillsConfigProvider { get; }

		public KdTreeStorage CollectablesKdTreeStorage { get; private set; }
		
		public KdTreeStorage ArenasKdTreeStorage { get; private set; }

		public SignalBus SignalBus { get; }

		public DamageTextView DamageTextViewPrefab { get; }

		public IPlayersLevelsConfigProvider PlayersLevelsConfigProvider { get; }

		public EcsWorld MainWorld { get; private set; }
		
		public EcsWorld EffectsWorld { get; private set; }

		public DiContainer Container { get; private set; }
		
		public ILevelPoolObjectCreatorList LevelPoolObjectCreatorList { get; }

		public Vector2 LocationSize { get; private set; }

		public SharedData(
			IPlayerConfigProvider playerConfigProvider,
			IEnemyConfigProvider enemyConfigProvider,
			IWeaponConfigProvider weaponConfigProvider,
			IGameModelProvider gameModelProvider,
			ICollisionConfigProvider collisionConfigProvider,
			ILevelEnemyWaveConfigProvider enemyWavesConfigProvider,
			ICollectableConfigProvider collectablesConfigProvider,
			ISkillConfigProvider skillsConfigProvider,
			IPlayersLevelsConfigProvider playersLevelsConfigProvider,
			ICompositeViewPool compositeViewPool,
			SignalBus signalBus,
			DamageTextView damageTextViewPrefab,
			DiContainer container,
			ILevelPoolObjectCreatorList levelPoolObjectCreatorList)
		{
			PlayerConfigProvider = playerConfigProvider;
			EnemyConfigProvider = enemyConfigProvider;
			WeaponConfigProvider = weaponConfigProvider;
			CollisionConfigProvider = collisionConfigProvider;
			EnemyWavesConfigProvider = enemyWavesConfigProvider;
			CollectablesConfigProvider = collectablesConfigProvider;
			SkillsConfigProvider = skillsConfigProvider;
			PlayersLevelsConfigProvider = playersLevelsConfigProvider;
			GameModel = gameModelProvider.Get();
			CompositeViewPool = compositeViewPool;
			SignalBus = signalBus;
			DamageTextViewPrefab = damageTextViewPrefab;
			Container = container;
			LevelPoolObjectCreatorList = levelPoolObjectCreatorList;
		}

		public void SetPlayerPositionProvider(IPositionProvider positionProvider)
		{
			PlayerPositionProvider = positionProvider;
		}

		public void SetPlayerWeapon(string weaponId)
		{
			PlayerWeaponId = weaponId;
		}

		public void SetCollectablesKdTreeStorage(KdTreeStorage kdTreeStorage)
		{
			CollectablesKdTreeStorage = kdTreeStorage;
		}
		
		public void SetArenasKdTreeStorage(KdTreeStorage kdTreeStorage)
		{
			ArenasKdTreeStorage = kdTreeStorage;
		}

		public void SetMainWorld(EcsWorld world)
		{
			MainWorld = world;
		}
		
		public void SetEffectsWorld(EcsWorld world)
		{
			EffectsWorld = world;
		}

		public void SetLocationSize(Vector2 locationSize)
		{
			LocationSize = locationSize;
		}

		public class Factory : PlaceholderFactory<SharedData>
		{
		}
	}
}