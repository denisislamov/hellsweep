namespace TonPlay.Client.Roguelike.Profile.Interfaces
{
	public interface IProfileConfigProvider
	{
		IProfileConfig Get(int level);
	}
}