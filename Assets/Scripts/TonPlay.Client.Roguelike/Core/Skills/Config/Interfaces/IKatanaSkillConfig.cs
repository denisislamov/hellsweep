using TonPlay.Client.Roguelike.Core.Effects.Revolver;
using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface IKatanaSkillConfig : ISkillConfig
	{
		CrossbowSightEffect SightEffectView { get; }
		
		IProjectileConfig ProjectileConfig { get; }

		IKatanaLevelSkillConfig GetLevelConfig(int level);
	}
	
	public interface IKatanaLevelSkillConfig
	{
		int ProjectileQuantity { get; }
		
		float Damage { get; }
		
		float ShootDelay { get; }

		float FieldOfView { get; }

		int CollisionLayerMask { get; }

		ICollisionAreaConfig CollisionAreaConfig { get; }
	}
}