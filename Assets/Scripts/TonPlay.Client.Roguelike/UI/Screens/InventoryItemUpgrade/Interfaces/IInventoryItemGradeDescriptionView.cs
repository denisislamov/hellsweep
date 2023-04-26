using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.InventoryItemUpgrade.Interfaces
{
	public interface IInventoryItemGradeDescriptionView : IView
	{
		void SetText(string text);
		void SetUnlockedState(bool state);
		void SetIconBackgroundColor(Color color);
		void UpdateMainLayout();
		void UpdateTextLayout();
		void SetGradeLayoutActiveState(bool state);
	}
}