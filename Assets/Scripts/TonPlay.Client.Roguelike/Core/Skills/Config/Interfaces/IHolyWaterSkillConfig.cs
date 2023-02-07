using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface IHolyWaterSkillConfig : ISkillConfig
	{
		IProjectileConfig BottleProjectileConfig { get; }
		
		IProjectileConfig DamagingAreaProjectileConfig { get; }
		
		float DelayBetweenThrowingProjectiles { get; }
		
		int CollisionLayerMask { get; }

		IHolyWaterSkillLevelConfig GetLevelConfig(int level);
	}
	
	public interface IHolyWaterSkillLevelConfig 
	{
		int Quantity { get; }
		
		float Cooldown { get; }
		
		IDamageProvider DamageProvider { get; }
	}
}