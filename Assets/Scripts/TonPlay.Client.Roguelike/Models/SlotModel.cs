using System;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.Models
{
	internal class SlotModel : ISlotModel
	{
		private readonly SlotData _data = new SlotData();

		private SlotName _slotName;
		private ReactiveProperty<string> _id = new ReactiveProperty<string>();
		private ReactiveProperty<string> _itemId = new ReactiveProperty<string>();
		
		private Subject<Unit> _updated = new Subject<Unit>();

		public SlotName SlotName => _slotName;
		public IReadOnlyReactiveProperty<string> Id => _id;
		public IReadOnlyReactiveProperty<string> ItemId => _itemId;
		public IObservable<Unit> Updated => _updated;

		public void Update(SlotData data)
		{
			_slotName = data.SlotName;
			
			_id.SetValueAndForceNotify(data.Id);
			
			_itemId.SetValueAndForceNotify(data.ItemId);
			
			_updated.OnNext(Unit.Default);
		}
		
		public SlotData ToData()
		{
			_data.Id = _id.Value;
			_data.SlotName = _slotName;
			_data.ItemId = _itemId.Value;

			return _data;
		}
	}
}