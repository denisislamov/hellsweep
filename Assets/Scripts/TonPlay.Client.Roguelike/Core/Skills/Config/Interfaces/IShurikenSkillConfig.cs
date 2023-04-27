using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface IShurikenSkillConfig : ISkillConfig<IShurikenLevelSkillConfig>
	{
		IProjectileConfig ProjectileConfig { get; }
	}
	
	public interface IShurikenLevelSkillConfig : ISkillLevelConfig
	{
		IDamageProvider DamageProvider { get; }

		float ShootDelay { get; }

		int CollisionLayerMask { get; }

		ICollisionAreaConfig CollisionAreaConfig { get; }
	}
}