using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Skills.Config.Updaters;
using TonPlay.Client.Roguelike.Network.Response;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Skills.Config
{
	public class SkillConfigUpdater : ISkillConfigUpdater, ISkillConfigUpdaterVisitor
	{
		private readonly ISkillConfigProvider _skillConfigProvider;

		private SkillAllResponse.Skill _currentSkill;
		private BoostAllResponse.Boost _currentBoost;

		public SkillConfigUpdater(ISkillConfigProvider skillConfigProvider)
		{
			_skillConfigProvider = skillConfigProvider;
		}

		public void UpdateConfig(SkillName skillName, SkillAllResponse.Skill data)
		{
			var config = _skillConfigProvider.Get(skillName);
			if (config is null)
			{
				Debug.LogWarning($"{skillName} doesn't exists. Nothing to update.");
				return;
			}

			_currentSkill = data;

			config.AcceptUpdaterVisitor(this);
		}

		public void UpdateConfig(SkillName skillName, BoostAllResponse.Boost data)
		{
			var config = _skillConfigProvider.Get(skillName);
			if (config is null)
			{
				Debug.LogWarning($"{skillName} doesn't exists. Nothing to update.");
				return;
			}

			_currentBoost = data;

			config.AcceptUpdaterVisitor(this);
		}

		public void Update(BrickSkillConfig config)
		{
			new BrickSkillConfigUpdater(config, _currentSkill).Update();
		}

		public void Update(DrillShotSkillConfig config)
		{
			new DrillShotSkillConfigUpdater(config, _currentSkill).Update();
		}

		public void Update(CrossbowSkillConfig config)
		{
			new CrossbowSkillConfigUpdater(config, _currentSkill).Update();
		}

		public void Update(ForcefieldDeviceSkillConfig config)
		{
			new ForcefieldDeviceSkillConfigUpdater(config, _currentSkill).Update();
		}

		public void Update(GuardianSkillConfig config)
		{
			new GuardianSkillConfigUpdater(config, _currentSkill).Update();
		}

		public void Update(HolyWaterSkillConfig config)
		{
			new HolyWaterSkillConfigUpdater(config, _currentSkill).Update();
		}

		public void Update(KatanaSkillConfig config)
		{
			new KatanaSkillConfigUpdater(config, _currentSkill).Update();
		}

		public void Update(RevolverSkillConfig config)
		{
			new RevolverSkillConfigUpdater(config, _currentSkill).Update();
		}
		
		public void Update(RPGSkillConfig config)
		{
			new RPGSkillConfigUpdater(config, _currentSkill).Update();
		}

		public void Update(RoninOyoroiSkillConfig config)
		{
			new RoninOyoroiSkillConfigUpdater(config, _currentBoost).Update();
		}

		public void Update(SportShoesSkillConfig config)
		{
			new SportShoesSkillConfigUpdater(config, _currentBoost).Update();
		}
		
		public void Update(HiPowerBulletSkillConfig config)
		{
			new HiPowerBulletSkillConfigUpdater(config, _currentBoost).Update();
		}

		public void Update(HiPowerMagnetSkillConfig config)
		{
			new HiPowerMagnetSkillConfigUpdater(config, _currentBoost).Update();
		}
		
		public void Update(ExoBracerSkillConfig config)
		{
			new ExoBracerSkillConfigUpdater(config, _currentBoost).Update();
		}

		public void Update(FitnessGuideSkillConfig config)
		{
			new FitnessGuideSkillConfigUpdater(config, _currentBoost).Update();
		}
		
		public void Update(EnergyDrinkSkillConfig config)
		{
			new EnergyDrinkSkillConfigUpdater(config, _currentBoost).Update();
		}
	}
}