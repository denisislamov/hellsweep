using System;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.Models
{
	public class ShopPackModel : IShopPackModel
	{
		private readonly ShopPackData _cached = new ShopPackData();
		private readonly ShopPackRewardsModel _rewards = new ShopPackRewardsModel();
		private readonly ReactiveProperty<float> _price = new ReactiveProperty<float>();
		
		private string _id;

		public string Id => _id;
		public IReadOnlyReactiveProperty<float> Price => _price;
		public IShopPackRewardsModel Rewards => _rewards;

		public void Update(ShopPackData data)
		{
			_id = data.Id;

			if (Math.Abs(_price.Value - data.Price) > 0.000001f)
			{
				_price.SetValueAndForceNotify(data.Price);
			}
			
			_rewards.Update(data.Rewards);
		}
		
		public ShopPackData ToData()
		{
			_cached.Id = _id;
			_cached.Price = _price.Value;
			_cached.Rewards = _rewards.ToData();
			return _cached;
		}
	}
}