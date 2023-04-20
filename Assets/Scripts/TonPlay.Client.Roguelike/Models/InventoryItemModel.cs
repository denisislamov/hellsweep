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
		private ReactiveProperty<string> _itemId = new ReactiveProperty<string>();
		private ReactiveProperty<string> _detailId = new ReactiveProperty<string>();

		public IReadOnlyReactiveProperty<string> Id => _id;
		public IReadOnlyReactiveProperty<string> DetailId => _detailId;
		public IReadOnlyReactiveProperty<string> ItemId => _itemId;
		
		public void Update(InventoryItemData data)
		{
			_id.SetValueAndForceNotify(data.Id);
			_itemId.SetValueAndForceNotify(data.ItemId);
			_detailId.SetValueAndForceNotify(data.DetailId);
		}
		
		public InventoryItemData ToData()
		{
			_data.Id = _id.Value;
			_data.DetailId = _detailId.Value;
			_data.ItemId = _itemId.Value;

			return _data;
		}
	}
}