using System;
using TonPlay.Client.Roguelike.Models.Data;
using UniRx;

namespace TonPlay.Client.Roguelike.Models.Interfaces
{
	public interface ISlotModel : IModel<SlotData>
	{
		public SlotName SlotName { get; }
		
		public IReadOnlyReactiveProperty<string> Id { get; }
		
		public IReadOnlyReactiveProperty<string> ItemId { get; }
		
		IObservable<Unit> Updated { get; }
	}
}