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
		
		IReadOnlyList<ISlotModel> MergeSlots { get; }
		
		IReadOnlyReactiveProperty<long> BlueprintsArms { get; }
		
		IReadOnlyReactiveProperty<long> BlueprintsBody { get; }
		
		IReadOnlyReactiveProperty<long> BlueprintsBelt { get; }
		
		IReadOnlyReactiveProperty<long> BlueprintsFeet { get; }
		
		IReadOnlyReactiveProperty<long> BlueprintsNeck { get; }
		
		IReadOnlyReactiveProperty<long> BlueprintsWeapon { get; }
		
		IReadOnlyReactiveProperty<int> CommonKeys { get; }
		
		IReadOnlyReactiveProperty<int> UncommonKeys { get; }
		
		IReadOnlyReactiveProperty<int> RareKeys { get; }
		
		IReadOnlyReactiveProperty<int> LegendaryKeys { get; }
		
		IObservable<Unit> Updated { get; }
		
		IInventoryItemModel GetItemModel(string userItemId);
	}
}