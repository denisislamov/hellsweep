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

		private readonly ReactiveProperty<long> _blueprintsArms = new ReactiveProperty<long>();
		private readonly ReactiveProperty<long> _blueprintsBody = new ReactiveProperty<long>();
		private readonly ReactiveProperty<long> _blueprintsBelt = new ReactiveProperty<long>();
		private readonly ReactiveProperty<long> _blueprintsFeet = new ReactiveProperty<long>();
		private readonly ReactiveProperty<long> _blueprintsNeck = new ReactiveProperty<long>();
		private readonly ReactiveProperty<long> _blueprintsWeapon = new ReactiveProperty<long>();
		
		public IReadOnlyReactiveProperty<int> Gold => _gold;
		public IReadOnlyReactiveProperty<float> ProfileExperience => _profileExperience;
		public IReadOnlyList<IInventoryItemModel> Items => _items;
		
		public IReadOnlyReactiveProperty<long> BlueprintsArms => _blueprintsArms;
		public IReadOnlyReactiveProperty<long> BlueprintsBody => _blueprintsBody;
		public IReadOnlyReactiveProperty<long> BlueprintsBelt => _blueprintsBelt;
		public IReadOnlyReactiveProperty<long> BlueprintsFeet => _blueprintsFeet;
		public IReadOnlyReactiveProperty<long> BlueprintsNeck => _blueprintsNeck;
		public IReadOnlyReactiveProperty<long> BlueprintsWeapon => _blueprintsWeapon;

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
			
			if (data.BlueprintsArms != BlueprintsArms.Value)
			{
				_blueprintsArms.SetValueAndForceNotify(data.BlueprintsArms);
			}
			
			if (data.BlueprintsBody != BlueprintsBody.Value)
			{
				_blueprintsBody.SetValueAndForceNotify(data.BlueprintsBody);
			}
			
			if (data.BlueprintsBelt != BlueprintsBelt.Value)
			{
				_blueprintsBelt.SetValueAndForceNotify(data.BlueprintsBelt);
			}
			
			if (data.BlueprintsFeet != BlueprintsFeet.Value)
			{
				_blueprintsFeet.SetValueAndForceNotify(data.BlueprintsFeet);
			}
			
			if (data.BlueprintsNeck != BlueprintsNeck.Value)
			{
				_blueprintsNeck.SetValueAndForceNotify(data.BlueprintsNeck);
			}
			
			if (data.BlueprintsWeapon != BlueprintsWeapon.Value)
			{
				_blueprintsWeapon.SetValueAndForceNotify(data.BlueprintsWeapon);
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
			_cached.BlueprintsArms = _blueprintsArms.Value;
			_cached.BlueprintsBelt = _blueprintsBelt.Value;
			_cached.BlueprintsBody = _blueprintsBody.Value;
			_cached.BlueprintsFeet = _blueprintsFeet.Value;
			_cached.BlueprintsNeck = _blueprintsNeck.Value;
			_cached.BlueprintsWeapon = _blueprintsWeapon.Value;
			return _cached;
		}
	}
}