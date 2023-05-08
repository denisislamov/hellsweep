using TonPlay.Client.Roguelike.Models.Data;

namespace TonPlay.Client.Roguelike.Models.Interfaces
{
	public interface IShopPackRewardsModel : IModel<ShopPackRewardsData>
	{
		uint Energy { get; }
		
		ulong Coins { get; }
		
		uint Blueprints { get; }
		
		uint KeysCommon { get; }
		
		uint KeysUncommon { get; }
		
		uint KeysRare { get; }
		
		uint KeysLegendary { get; }
		
		uint HeroSkins { get; }
	}
}