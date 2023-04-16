using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces
{
	public interface IInventorySlotView : IView
	{
		IButtonView ButtonView { get; }

		void SetEmptyState(bool state);

		void SetIcon(Sprite sprite);

		void SetBackgroundMaterial(Material material);
	}
}