using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models
{
	public class ShopModel : IShopModel
	{
		private readonly ShopData _cached = new ShopData();
		
		private readonly List<IShopPackModel> _packs = new List<IShopPackModel>();

		public IReadOnlyList<IShopPackModel> Packs => _packs;

		public void Update(ShopData data)
		{
			_packs.Clear();
			
			_packs.AddRange(data.Packs.Select(shopPackData =>
			{
				var model = new ShopPackModel();
				model.Update(shopPackData);
				return model;
			}));
		}
		
		public ShopData ToData()
		{
			_cached.Packs = _packs.Select(_ => _.ToData()).ToList();
			return _cached;
		}
	}
}