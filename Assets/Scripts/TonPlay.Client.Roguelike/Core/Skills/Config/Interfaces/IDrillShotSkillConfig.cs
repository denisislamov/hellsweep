using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface IDrillShotSkillConfig : ISkillConfig<IDrillShotSkillLevelConfig>
	{
		IProjectileConfig ProjectileConfig { get; }

		Rect FlyingZone { get; }
		
		float DelayBetweenSpawn { get; }
	}
	
	public interface IDrillShotSkillLevelConfig : ISkillLevelConfig
	{
		int Quantity { get; }
		
		float Speed { get; }

		float Cooldown { get; }

		IDamageProvider DamageProvider { get; }

		ICollisionAreaConfig CollisionAreaConfig { get; }
	}
}