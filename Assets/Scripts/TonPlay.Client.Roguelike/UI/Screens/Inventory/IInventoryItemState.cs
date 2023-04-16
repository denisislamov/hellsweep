using TonPlay.Client.Roguelike.Models.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory
{
	public interface IInventoryItemState
	{
		IInventoryItemModel Model { get; }
		IReadOnlyReactiveProperty<bool> EquippedState { get; }
		void SetEquippedState(bool state);
	}
}