using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Effects;
using TonPlay.Roguelike.Client.Core.Skills.Config.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface IForcefieldDeviceSkillConfig : ISkillConfig
	{
		EffectView EffectView { get; }
		
		IForcefieldDeviceSkillLevelConfig GetLevelConfig(int level);
	}

	public interface IForcefieldDeviceSkillLevelConfig 
	{
		float Damage { get; }
		
		float Size { get; }

		int CollisionLayerMask { get; }

		ICollisionAreaConfig CollisionAreaConfig { get; }
	}
}