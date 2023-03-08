using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface IKatanaSkillConfig : ISkillConfig<IKatanaLevelSkillConfig>
	{
		IProjectileConfig ProjectileConfig { get; }
	}

	public interface IKatanaLevelSkillConfig : ISkillLevelConfig
	{
		int ProjectileQuantity { get; }

		IDamageProvider DamageProvider { get; }

		float Cooldown { get; }

		float PrepareAttackTiming { get; }

		float ShootDelay { get; }

		Vector2 SpawnOffset { get; }
	}
}