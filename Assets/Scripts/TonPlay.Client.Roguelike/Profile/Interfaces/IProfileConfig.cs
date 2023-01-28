namespace TonPlay.Client.Roguelike.Profile.Interfaces
{
	public interface IProfileConfig
	{
		int Level { get; }
		
		float ExperienceToLevelUp { get; }
		
		int MaxEnergy { get; }
	}
}