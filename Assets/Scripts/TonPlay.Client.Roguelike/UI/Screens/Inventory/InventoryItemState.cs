using TonPlay.Client.Roguelike.Models.Interfaces;
using UniRx;

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
	}
}