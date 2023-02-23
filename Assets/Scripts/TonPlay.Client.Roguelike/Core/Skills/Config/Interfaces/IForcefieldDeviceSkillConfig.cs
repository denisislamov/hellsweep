using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Effects;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface IForcefieldDeviceSkillConfig : ISkillConfig<IForcefieldDeviceSkillLevelConfig>
	{
		EffectView EffectView { get; }
	}

	public interface IForcefieldDeviceSkillLevelConfig : ISkillLevelConfig
	{
		IDamageProvider DamageProvider { get; }

		float Size { get; }

		int CollisionLayerMask { get; }

		ICollisionAreaConfig CollisionAreaConfig { get; }
	}
}