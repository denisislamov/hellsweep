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
		private readonly ReactiveProperty<long> _blueprints = new ReactiveProperty<long>();

		private List<IInventoryItemModel> _items = new List<IInventoryItemModel>();
		private Dictionary<SlotName, ISlotModel> _slots = new Dictionary<SlotName, ISlotModel>();
		private List<ISlotModel> _mergeSlots = new List<ISlotModel>(3);

		public IReadOnlyList<IInventoryItemModel> Items => _items;
		public IReadOnlyDictionary<SlotName, ISlotModel> Slots => _slots;
		public IReadOnlyList<ISlotModel> MergeSlots => _mergeSlots;

		public IReadOnlyReactiveProperty<long> Blueprints => _blueprints;
		public IObservable<Unit> Updated => _updated;
		
		public IInventoryItemModel GetItemModel(string userItemId)
		{
			return Items.FirstOrDefault(_ => _.Id.Value == userItemId);
		}

		public void Update(InventoryData data)
		{
			UpdateItems(data);
			UpdateSlots(data);
			UpdateMergeSlots(data);
			
			if (data.Blueprints != Blueprints.Value)
			{
				_blueprints.SetValueAndForceNotify(data.Blueprints);
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
				if (_items.Count >= i)
				{
					_items.Add(new InventoryItemModel());
				}

				_items[i].Update(data.Items[i]);
			}

			while (_items.Count > data.Items.Count)
			{
				_items.RemoveAt(_items.Count - 1);
			}
		}

		private void UpdateMergeSlots(InventoryData data)
		{
			for (var i = 0; i < data.MergeSlots.Count; i++)
			{
				if (_mergeSlots.Count >= i)
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
			
			return _data;
		}
	}
}