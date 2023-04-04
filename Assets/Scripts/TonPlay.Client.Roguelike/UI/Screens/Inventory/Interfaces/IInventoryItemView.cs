using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces
{
	public interface IInventoryItemView : ICollectionItemView
	{
		void SetBackgroundGradient(Gradient gradient);

		void SetPanelsColor(Color color);

		void SetItemGroupIcon(Sprite sprite);
		
		void SetItemIcon(Sprite sprite);

		void SetPanelText(string text);
	}
}