using System.Collections.Generic;
using TonPlay.Client.Roguelike.Models.Data;

namespace TonPlay.Client.Roguelike.Models.Interfaces
{
	public interface IShopModel : IModel<ShopData>
	{
		IReadOnlyList<IShopPackModel> Packs { get; }
		
		IReadOnlyList<IShopResourceModel> Resources { get; }
	}
}