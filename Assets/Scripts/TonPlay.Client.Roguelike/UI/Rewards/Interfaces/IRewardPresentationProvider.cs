namespace TonPlay.Client.Roguelike.UI.Rewards.Interfaces
{
	public interface IRewardPresentationProvider
	{
		IRewardPresentation Get(string id);
	}
}