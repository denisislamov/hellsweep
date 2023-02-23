using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface IRPGSkillConfig : ISkillConfig<IRPGSkillLevelConfig>
	{
		IProjectileConfig ProjectileConfig { get; }
	}

	public interface IRPGSkillLevelConfig : ISkillLevelConfig
	{
		float Delay { get; }

		int ProjectileQuantity { get; }

		IDamageProvider DamageProvider { get; }
	}
}