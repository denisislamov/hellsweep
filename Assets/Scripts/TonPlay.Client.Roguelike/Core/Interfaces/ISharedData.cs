using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collectables.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Enemies.Configs.Interfaces;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.UI;
using TonPlay.Client.Roguelike.Core.Waves.Interfaces;
using TonPlay.Client.Roguelike.Interfaces;
using TonPlay.Roguelike.Client.Core;
using TonPlay.Roguelike.Client.Core.Collision.Config;
using TonPlay.Roguelike.Client.Core.Levels.Config.Interfaces;
using TonPlay.Roguelike.Client.Core.Player.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using Zenject;

namespace TonPlay.Client.Roguelike.Core.Interfaces
{
	public interface ISharedData
	{
		IPlayerConfigProvider PlayerConfigProvider { get; }

		IEnemyConfigProvider EnemyConfigProvider { get; }

		IWeaponConfigProvider WeaponConfigProvider { get; }

		ICollisionConfigProvider CollisionConfigProvider { get; }

		IGameModel GameModel { get; }

		IPositionProvider PlayerPositionProvider { get; }

		ILevelEnemyWaveConfigProvider EnemyWavesConfigProvider { get; }

		string PlayerWeaponId { get; }

		ICompositeViewPool CompositeViewPool { get; }

		ICollectableConfigProvider CollectablesConfigProvider { get; }

		IPlayersLevelsConfigProvider PlayersLevelsConfigProvider { get; }

		ISkillConfigProvider SkillsConfigProvider { get; }

		KdTreeStorage CollectablesKdTreeStorage { get; }
		
		KdTreeStorage ArenasKdTreeStorage { get; }

		SignalBus SignalBus { get; }

		DamageTextView DamageTextViewPrefab { get; }

		EcsWorld World { get; }

		DiContainer Container { get; }
	}
}