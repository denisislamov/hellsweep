namespace TonPlay.Client.Roguelike.Shop
{
	public interface IShopRewardPresentationProvider
	{
		IShopPackPresentationConfig Get(string packId);
		IShopPackRewardPresentationConfig GetRewardPresentation(string rewardId);
	}
}