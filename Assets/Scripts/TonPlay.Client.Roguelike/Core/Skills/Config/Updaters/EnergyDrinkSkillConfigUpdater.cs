using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Network.Response;

namespace TonPlay.Client.Roguelike.Core.Skills.Config.Updaters
{
	public struct EnergyDrinkSkillConfigUpdater : IUpdatable
	{
		private readonly EnergyDrinkSkillConfig _config;
		private readonly BoostAllResponse.Boost _remoteConfig;
		
		public EnergyDrinkSkillConfigUpdater(
			EnergyDrinkSkillConfig config, 
			BoostAllResponse.Boost remoteConfig)
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
				levelConfig.SetValue(detail.value);
			}
		}
	}
}