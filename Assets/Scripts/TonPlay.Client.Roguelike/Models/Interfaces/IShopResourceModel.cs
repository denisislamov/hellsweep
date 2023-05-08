using TonPlay.Client.Roguelike.Models.Data;

namespace TonPlay.Client.Roguelike.Models.Interfaces
{
	public interface IShopResourceModel : IModel<ShopResourceData>
	{
		string Id { get; }
		RarityName Rarity { get; }
		ShopResourceType Type { get; }
		ulong Amount { get; }
		float Price { get; }
	}
}