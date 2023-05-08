using TonPlay.Client.Roguelike.Models.Data;
using UniRx;

namespace TonPlay.Client.Roguelike.Models.Interfaces
{
	public interface IShopPackModel : IModel<ShopPackData>
	{
		string Id { get; }

		IReadOnlyReactiveProperty<float> Price { get;}

		IShopPackRewardsModel Rewards { get; }
	}
}