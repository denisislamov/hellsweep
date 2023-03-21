using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Network.Response;
using TonPlay.Client.Roguelike.Utilities;

namespace TonPlay.Client.Roguelike.Core.Skills.Config.Updaters
{
	public struct RPGSkillConfigUpdater : IUpdatable
	{
		private readonly RPGSkillConfig _config;
		private readonly SkillAllResponse.Skill _remoteConfig;
		
		public RPGSkillConfigUpdater(
			RPGSkillConfig config, 
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
					case RoguelikeConstants.Core.Skills.FeatureIds.RANGE:
						levelConfig.SetCooldown(detail.value);
						break;
					case RoguelikeConstants.Core.Skills.FeatureIds.QUANTITY:
						levelConfig.SetQuantity((int) detail.value);
						break;
				}
			}
		}
	}
}