namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface ISkillConfigUpdaterVisitor
	{
		void Update(BrickSkillConfig brickSkillConfig);
		
		void Update(EnergyDrinkSkillConfig energyDrinkSkillConfig);
		
		void Update(DrillShotSkillConfig drillShotSkillConfig);
		
		void Update(CrossbowSkillConfig crossbowSkillConfig);
		
		void Update(ExoBracerSkillConfig exoBracerSkillConfig);
		
		void Update(FitnessGuideSkillConfig fitnessGuideSkillConfig);
		
		void Update(ForcefieldDeviceSkillConfig forcefieldDeviceSkillConfig);
		
		void Update(GuardianSkillConfig guardianSkillConfig);
		
		void Update(HiPowerBulletSkillConfig hiPowerBulletSkillConfig);
		
		void Update(HiPowerMagnetSkillConfig hiPowerMagnetSkillConfig);
		
		void Update(HolyWaterSkillConfig holyWaterSkillConfig);
		
		void Update(KatanaSkillConfig katanaSkillConfig);
		
		void Update(RevolverSkillConfig revolverSkillConfig);
		
		void Update(RoninOyoroiSkillConfig roninOyoroiSkillConfig);
		
		void Update(RPGSkillConfig rpgSkillConfig);
		
		void Update(SportShoesSkillConfig sportShoesSkillConfig);
	}
}