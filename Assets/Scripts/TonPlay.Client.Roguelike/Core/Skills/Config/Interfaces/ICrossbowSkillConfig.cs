using TonPlay.Client.Roguelike.Core.Effects.Revolver;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface ICrossbowSkillConfig : ISkillConfig
	{
		CrossbowSightEffect SightEffectView { get; }
		
		IProjectileConfig ProjectileConfig { get; }

		ICrossbowLevelSkillConfig GetLevelConfig(int level);
	}
	
	public interface ICrossbowLevelSkillConfig
	{
		IDamageProvider DamageProvider { get; }

		int ProjectileQuantity { get; }
		
		float ShootDelay { get; }

		float FieldOfView { get; }
	}
}