using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Effects.Revolver;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface IKatanaSkillConfig : ISkillConfig
	{
		IProjectileConfig ProjectileConfig { get; }

		IKatanaLevelSkillConfig GetLevelConfig(int level);
	}

	public interface IKatanaLevelSkillConfig
	{
		int ProjectileQuantity { get; }

		IDamageProvider DamageProvider { get; }
		
		float Cooldown { get; }

		float ShootDelay { get; }
		
		Vector2 SpawnOffset { get; }
	}
}