using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Network.Response;
using TonPlay.Client.Roguelike.Utilities;

namespace TonPlay.Client.Roguelike.Core.Skills.Config.Updaters
{
	public struct GuardianSkillConfigUpdater : IUpdatable
	{
		private readonly GuardianSkillConfig _config;
		private readonly SkillAllResponse.Skill _remoteConfig;
		
		public GuardianSkillConfigUpdater(
			GuardianSkillConfig config, 
			SkillAllResponse.Skill remoteConfig)
		{
			_config = config;
			_remoteConfig = remoteConfig;
		}

		public void Update()
		{
			var data = _remoteConfig;
			for (var i = 0; i < data.details.Count; i++)
			{
				var detail = data.details[i];
				var levelConfig = _config.GetLevelConfigInternal(detail.level);

				switch (detail.feature)
				{
					case RoguelikeConstants.Core.Skills.FeatureIds.DAMAGE:
						levelConfig.SetDamage(detail.value);
						break;
					case RoguelikeConstants.Core.Skills.FeatureIds.SPIN_SPEED:
						levelConfig.SetSpinSpeed(detail.value);
						break;
					case RoguelikeConstants.Core.Skills.FeatureIds.COOLDOWN:
						levelConfig.SetCooldown(detail.value);
						break;
					case RoguelikeConstants.Core.Skills.FeatureIds.DURATION:
						levelConfig.SetDuration(detail.value);
						break;
					case RoguelikeConstants.Core.Skills.FeatureIds.RANGE:
						levelConfig.SetRange(detail.value);
						break;
					case RoguelikeConstants.Core.Skills.FeatureIds.QUANTITY:
						levelConfig.SetQuantity((int) detail.value);
						break;
				}
			}
		}
	}
}