namespace TonPlay.Client.Roguelike.Profile.Interfaces
{
	public interface IProfileConfigProviderUpdater
	{
		void UpdateConfigExperienceToLevelUp(int level, long experienceToLevel);
	}
}