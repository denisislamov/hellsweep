using System;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.Models
{
	internal class InventoryItemModel : IInventoryItemModel
	{
		private readonly InventoryItemData _data = new InventoryItemData();
		
		private ReactiveProperty<string> _id = new ReactiveProperty<string>();
		private ReactiveProperty<string> _detailId = new ReactiveProperty<string>();
		// private ReactiveProperty<string> _name = new ReactiveProperty<string>();
		// private ReactiveProperty<RarityName> _rarity = new ReactiveProperty<RarityName>();
		private ReactiveProperty<ushort> _level = new ReactiveProperty<ushort>();

		public IReadOnlyReactiveProperty<string> Id => _id;
		public IReadOnlyReactiveProperty<string> DetailId => _detailId;
		// public IReadOnlyReactiveProperty<string> Name => _name;
		public IReadOnlyReactiveProperty<ushort> Level => _level;
		// public IReadOnlyReactiveProperty<RarityName> Rarity => _rarity;
		
		public void Update(InventoryItemData data)
		{
			_id.SetValueAndForceNotify(data.Id);
			_detailId.SetValueAndForceNotify(data.DetailId);
			_level.SetValueAndForceNotify(data.Level);
			// _name.SetValueAndForceNotify(data.Name);
			// _rarity.SetValueAndForceNotify(data.Rarity);
		}
		
		public InventoryItemData ToData()
		{
			_data.Id = _id.Value;
			_data.DetailId = _detailId.Value;
			_data.Level = _level.Value;
			// _data.Name = _name.Value;
			// _data.Rarity = _rarity.Value;

			return _data;
		}
	}
}