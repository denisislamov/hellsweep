using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.Models
{
	internal class InventoryModel : IInventoryModel
	{
		private readonly InventoryData _data = new InventoryData()
		{
			Items = new List<InventoryItemData>(),
			Slots = new Dictionary<SlotName, SlotData>(),
			MergeSlots = new List<SlotData>(3)
		};
		
		private readonly Subject<Unit> _updated = new Subject<Unit>();

		private Dictionary<string, IInventoryItemModel> _itemsMap = new Dictionary<string, IInventoryItemModel>();

		private List<IInventoryItemModel> _items = new List<IInventoryItemModel>();
		private Dictionary<SlotName, ISlotModel> _slots = new Dictionary<SlotName, ISlotModel>();
		private List<ISlotModel> _mergeSlots = new List<ISlotModel>(3);

		public IReadOnlyList<IInventoryItemModel> Items => _items;
		public IReadOnlyDictionary<SlotName, ISlotModel> Slots => _slots;
		public IReadOnlyList<ISlotModel> MergeSlots => _mergeSlots;
		
		private readonly ReactiveProperty<long> _blueprintsArms = new ReactiveProperty<long>();
		private readonly ReactiveProperty<long> _blueprintsBody = new ReactiveProperty<long>();
		private readonly ReactiveProperty<long> _blueprintsBelt = new ReactiveProperty<long>();
		private readonly ReactiveProperty<long> _blueprintsFeet = new ReactiveProperty<long>();
		private readonly ReactiveProperty<long> _blueprintsNeck = new ReactiveProperty<long>();
		private readonly ReactiveProperty<long> _blueprintsWeapon = new ReactiveProperty<long>();
		
		private readonly ReactiveProperty<int> _commonKeys = new ReactiveProperty<int>();
		private readonly ReactiveProperty<int> _uncommonKeys = new ReactiveProperty<int>();
		private readonly ReactiveProperty<int> _rareKeys = new ReactiveProperty<int>();
		private readonly ReactiveProperty<int> _legendaryKeys = new ReactiveProperty<int>();
		
		private int _lastUpdateIndex;

		public IReadOnlyReactiveProperty<long> BlueprintsArms => _blueprintsArms;
		public IReadOnlyReactiveProperty<long> BlueprintsBody => _blueprintsBody;
		public IReadOnlyReactiveProperty<long> BlueprintsBelt => _blueprintsBelt;
		public IReadOnlyReactiveProperty<long> BlueprintsFeet => _blueprintsFeet;
		public IReadOnlyReactiveProperty<long> BlueprintsNeck => _blueprintsNeck;
		public IReadOnlyReactiveProperty<long> BlueprintsWeapon => _blueprintsWeapon;
		
		public IReadOnlyReactiveProperty<int> CommonKeys => _commonKeys;
		public IReadOnlyReactiveProperty<int> UncommonKeys => _uncommonKeys;
		public IReadOnlyReactiveProperty<int> RareKeys => _rareKeys;
		public IReadOnlyReactiveProperty<int> LegendaryKeys => _legendaryKeys;
		
		public IObservable<Unit> Updated => _updated;

		public int LastUpdateIndex => _lastUpdateIndex;
		
		public IInventoryItemModel GetItemModel(string userItemId)
		{
			if (string.IsNullOrWhiteSpace(userItemId) || !_itemsMap.ContainsKey(userItemId))
			{
				return null;
			}
			
			return _itemsMap[userItemId];
		}

		public void Update(InventoryData data)
		{
			_lastUpdateIndex++;

			UpdateItems(data);
			UpdateSlots(data);
			UpdateMergeSlots(data);
			
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
			
			if (data.CommonKeys != _commonKeys.Value)
			{
				_commonKeys.SetValueAndForceNotify(data.CommonKeys);
			}
			
			if (data.UncommonKeys != _uncommonKeys.Value)
			{
				_uncommonKeys.SetValueAndForceNotify(data.UncommonKeys);
			}
			
			if (data.RareKeys != _rareKeys.Value)
			{
				_rareKeys.SetValueAndForceNotify(data.RareKeys);
			}
			
			if (data.LegendaryKeys != _legendaryKeys.Value)
			{
				_legendaryKeys.SetValueAndForceNotify(data.LegendaryKeys);
			}

			_updated.OnNext(Unit.Default);
		}
		
		private void UpdateSlots(InventoryData data)
		{
			foreach (var kvp in data.Slots)
			{
				if (!_slots.ContainsKey(kvp.Key))
				{
					_slots.Add(kvp.Key, new SlotModel());
				}
				
				_slots[kvp.Key].Update(kvp.Value);
			}

			var slotsToDelete = new List<SlotName>();
			foreach (var kvp in _slots)
			{
				if (!data.Slots.ContainsKey(kvp.Key))
				{
					slotsToDelete.Add(kvp.Key);
				}
			}

			for (var i = 0; i < slotsToDelete.Count; i++)
			{
				data.Slots.Remove(slotsToDelete[i]);
			}
		}
		
		private void UpdateItems(InventoryData data)
		{
			for (var i = 0; i < data.Items.Count; i++)
			{
				IInventoryItemModel model;
				
				if (!_itemsMap.ContainsKey(data.Items[i].Id))
				{
					model = new InventoryItemModel();
					_items.Add(model);
				}
				else
				{
					model = _itemsMap[data.Items[i].Id];
				}

				data.Items[i].LastUpdateIndex = _lastUpdateIndex;

				model.Update(data.Items[i]);

				_itemsMap[data.Items[i].Id] = model;
			}

			for (var i = 0; i < _items.Count; i++)
			{
				if (_items[i].LastUpdateIndex == _lastUpdateIndex)
				{
					continue;
				}

				if (!string.IsNullOrWhiteSpace(_items[i].Id.Value))
				{
					_itemsMap.Remove(_items[i].Id.Value);
				}
				
				_items.RemoveAt(i);
				i--;
			}
		}

		private void UpdateMergeSlots(InventoryData data)
		{
			for (var i = 0; i < data.MergeSlots.Count; i++)
			{
				if (i >= _mergeSlots.Count)
				{
					_mergeSlots.Add(new SlotModel());
				}

				_mergeSlots[i].Update(data.MergeSlots[i]);
			}

			while (_mergeSlots.Count > data.MergeSlots.Count)
			{
				_mergeSlots.RemoveAt(_mergeSlots.Count - 1);
			}
		}
		
		public InventoryData ToData()
		{
			_data.Items.Clear();
			_data.Slots.Clear();
			_data.MergeSlots.Clear();
			
			for (var i = 0; i < _items.Count; i++)
			{
				_data.Items.Add(_items[i].ToData());
			}

			foreach (var kvp in _slots)
			{
				if (!_data.Slots.ContainsKey(kvp.Key))
				{
					_data.Slots.Add(kvp.Key, null);
				}

				_data.Slots[kvp.Key] = _slots[kvp.Key].ToData();
			}

			for (var i = 0; i < _mergeSlots.Count; i++)
			{
				_data.MergeSlots.Add(_mergeSlots[i].ToData());
			}

			_data.BlueprintsArms = BlueprintsArms.Value;
			_data.BlueprintsBody = BlueprintsBody.Value;
			_data.BlueprintsBelt = BlueprintsBelt.Value;
			_data.BlueprintsFeet = BlueprintsFeet.Value;
			_data.BlueprintsNeck = BlueprintsNeck.Value;
			_data.BlueprintsWeapon = BlueprintsWeapon.Value;
			
			_data.CommonKeys = CommonKeys.Value;
			_data.UncommonKeys = UncommonKeys.Value;
			_data.RareKeys = RareKeys.Value;
			_data.LegendaryKeys = LegendaryKeys.Value;
			
			return _data;
		}
	}
}