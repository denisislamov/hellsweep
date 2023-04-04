using System;
using System.Collections.Generic;
using TonPlay.Client.Roguelike.Models.Data;
using UniRx;

namespace TonPlay.Client.Roguelike.Models.Interfaces
{
	public interface IInventoryModel : IModel<InventoryData>
	{
		IReadOnlyList<IInventoryItemModel> Items { get; }
		
		IReadOnlyDictionary<SlotName, ISlotModel> Slots { get; }
		
		IObservable<Unit> Updated { get; }
	}
}