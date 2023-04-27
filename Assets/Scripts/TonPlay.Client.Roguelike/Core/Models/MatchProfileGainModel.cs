using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Models.Data;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.Core.Models
{
	public class MatchProfileGainModel : IMatchProfileGainModel
	{
		private readonly MatchProfileGainData _cached = new MatchProfileGainData();

		private readonly ReactiveProperty<int> _gold = new ReactiveProperty<int>();
		private readonly ReactiveProperty<float> _profileExperience = new ReactiveProperty<float>();
		private readonly List<IInventoryItemModel> _items = new List<IInventoryItemModel>();

		public IReadOnlyReactiveProperty<int> Gold => _gold;
		public IReadOnlyReactiveProperty<float> ProfileExperience => _profileExperience;
		public IReadOnlyList<IInventoryItemModel> Items => _items;

		public void Update(MatchProfileGainData data)
		{
			if (Math.Abs(_profileExperience.Value - data.ProfileExperience) > 0.001f)
			{
				_profileExperience.SetValueAndForceNotify(data.ProfileExperience);
			}

			if (_gold.Value != data.Gold)
			{
				_gold.SetValueAndForceNotify(data.Gold);
			}
			
			_items.Clear();
			
			foreach (var inventoryItemData in data.Items)
			{
				var itemModel = new InventoryItemModel();
				itemModel.Update(inventoryItemData);
				_items.Add(itemModel);
			}
		}

		public MatchProfileGainData ToData()
		{
			_cached.Gold = _gold.Value;
			_cached.ProfileExperience = _profileExperience.Value;
			_cached.Items = _items.Select(_ => _.ToData()).ToList();
			return _cached;
		}
	}
}