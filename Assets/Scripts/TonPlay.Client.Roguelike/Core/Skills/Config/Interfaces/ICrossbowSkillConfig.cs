using TonPlay.Client.Roguelike.Core.Effects.Revolver;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface ICrossbowSkillConfig : ISkillConfig<ICrossbowLevelSkillConfig>
	{
		CrossbowSightEffect SightEffectView { get; }

		IProjectileConfig ProjectileConfig { get; }
	}

	public interface ICrossbowLevelSkillConfig : ISkillLevelConfig
	{
		IDamageProvider DamageProvider { get; }

		int ProjectileQuantity { get; }

		float ShootDelay { get; }

		float FieldOfView { get; }
	}
}