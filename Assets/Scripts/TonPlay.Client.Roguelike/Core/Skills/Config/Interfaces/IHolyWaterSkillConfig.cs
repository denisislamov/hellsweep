using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface IHolyWaterSkillConfig : ISkillConfig<IHolyWaterSkillLevelConfig>
	{
		IProjectileConfig BottleProjectileConfig { get; }
		
		IProjectileConfig DamagingAreaProjectileConfig { get; }
		
		float DelayBetweenThrowingProjectiles { get; }
		
		int CollisionLayerMask { get; }
	}
	
	public interface IHolyWaterSkillLevelConfig : ISkillLevelConfig
	{
		int Quantity { get; }
		
		float Cooldown { get; }
		
		IDamageProvider DamageProvider { get; }
	}
}