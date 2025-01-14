using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Interfaces;
using UniRx;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory
{
	internal class InventoryItemState : IInventoryItemState
	{
		private readonly ReactiveProperty<bool> _state = new ReactiveProperty<bool>();
		public IInventoryItemModel Model { get; }
		public IReadOnlyReactiveProperty<bool> EquippedState => _state;
		
		
		public InventoryItemState(IInventoryItemModel model)
		{
			Model = model;
		}
		
		public void SetEquippedState(bool state)
		{
			_state.SetValueAndForceNotify(state);
		}

		private ReactiveProperty<MergeStates> _mergingState = new ReactiveProperty<MergeStates>(MergeStates.IN_INVENTORY);
		public IReadOnlyReactiveProperty<MergeStates> MergingState => _mergingState;
		
		public void SetMergeState(MergeStates state)
		{
			Common.Utilities.Logger.Log($"InventoryItemState SetMergeState {state}");
			_mergingState.SetValueAndForceNotify(state);
		}
	}
}