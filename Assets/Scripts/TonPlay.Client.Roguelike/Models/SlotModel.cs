using System;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.Models
{
	internal class SlotModel : ISlotModel
	{
		private readonly SlotData _data = new SlotData();
		private readonly IInventoryItemModel _item = new InventoryItemModel();

		private SlotName _slotName;
		private ReactiveProperty<string> _id = new ReactiveProperty<string>();
		
		private Subject<Unit> _updated = new Subject<Unit>();

		public SlotName SlotName => _slotName;
		public IReadOnlyReactiveProperty<string> Id => _id;
		public IInventoryItemModel Item => _item;
		public IObservable<Unit> Updated => _updated;

		public void Update(SlotData data)
		{
			_slotName = data.SlotName;
			
			_id.SetValueAndForceNotify(data.Id);
			
			_item.Update(data.Item);
			
			_updated.OnNext(Unit.Default);
		}
		
		public SlotData ToData()
		{
			_data.Id = _id.Value;
			_data.SlotName = _slotName;
			_data.Item = _item.ToData();

			return _data;
		}
	}
}