namespace TonPlay.Client.Roguelike.Shop
{
	public interface IShopPackPresentationProvider
	{
		IShopPackPresentationConfig Get(string packId);
		IShopPackRewardPresentationConfig GetRewardPresentation(string rewardId);
	}
}