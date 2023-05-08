using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;

namespace TonPlay.Client.Roguelike.Models
{
	internal class ShopResourceModel : IShopResourceModel
	{
		private readonly ShopResourceData _cached = new ShopResourceData();
		
		private string _id;
		private RarityName _rarity;
		private ulong _amount;
		private float _price;
		private ShopResourceType _type;

		public string Id => _id;
		public RarityName Rarity => _rarity;
		public ShopResourceType Type => _type;
		public ulong Amount => _amount;
		public float Price => _price;
		
		public void Update(ShopResourceData data)
		{
			_id = data.Id;
			_price = data.Price;
			_amount = data.Amount;
			_rarity = data.Rarity;
			_type = data.Type;
		}
		
		public ShopResourceData ToData()
		{
			_cached.Id = _id;
			_cached.Price = _price;
			_cached.Amount = _amount;
			_cached.Rarity = _rarity;
			_cached.Type = _type;
			return _cached;
		}
	}
}