using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Components.Skills
{
	public struct DrillShotProjectileComponent
	{
		public int CreatorEntityId;
		public IDrillShotSkillConfig Config;
		public IDrillShotSkillLevelConfig LevelConfig;
		public bool LastInsideRectState;
	}
}