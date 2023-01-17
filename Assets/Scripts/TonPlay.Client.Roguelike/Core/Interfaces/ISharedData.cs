using TonPlay.Client.Roguelike.Interfaces;
using TonPlay.Roguelike.Client.Core.Collectables.Config.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.Config;
using TonPlay.Roguelike.Client.Core.Enemies.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Levels.Config.Interfaces;
using TonPlay.Roguelike.Client.Core.Models.Interfaces;
using TonPlay.Roguelike.Client.Core.Player.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Core.Skills.Config.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;

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

		string PlayerWeaponId { get; }
		
		ICompositeViewPool CompositeViewPool { get; }
		
		ICollectableConfigProvider CollectablesConfigProvider { get; }
		
		IPlayersLevelsConfigProvider PlayersLevelsConfigProvider { get; }
		
		ISkillConfigProvider SkillsConfigProvider { get; }
	}
}