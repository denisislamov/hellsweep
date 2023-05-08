namespace TonPlay.Client.Roguelike.Shop
{
	public interface IShopResourcePresentationProvider
	{
		IShopResourcePresentationConfig Get(string resourceId);
	}
}