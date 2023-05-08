using System;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UniRx;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces
{
	public interface IInventoryItemView : ICollectionItemView
	{
		IButtonView ButtonView { get; }

		void SetBackgroundGradientMaterial(Material material);

		void SetPanelsColor(Color color);

		void SetItemSlotIcon(Sprite sprite);

		void SetItemIcon(Sprite sprite);

		void SetPanelText(string text);

		void SetEquippedState(bool state);
	}
}