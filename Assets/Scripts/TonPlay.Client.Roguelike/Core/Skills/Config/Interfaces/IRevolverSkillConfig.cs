using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Effects;
using TonPlay.Client.Roguelike.Core.Effects.Revolver;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface IRevolverSkillConfig : ISkillConfig
	{
		RevolverSightEffect SightEffectView { get; }
		
		IProjectileConfig ProjectileConfig { get; }

		IRevolverLevelSkillConfig GetLevelConfig(int level);
	}

	public interface IRevolverLevelSkillConfig
	{
		IDamageProvider DamageProvider { get; }
		
		float ShootDelay { get; }

		float FieldOfView { get; }

		int CollisionLayerMask { get; }

		ICollisionAreaConfig CollisionAreaConfig { get; }
	}
}