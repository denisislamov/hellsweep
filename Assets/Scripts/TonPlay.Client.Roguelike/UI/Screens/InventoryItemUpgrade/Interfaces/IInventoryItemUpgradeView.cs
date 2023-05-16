using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.InventoryItemUpgrade.Interfaces
{
	public interface IInventoryItemUpgradeView : IView
	{
		IButtonView CloseButtonView { get; }
		
		IButtonView EquipButtonView { get; }
		
		IButtonView UpgradeButtonView { get; }
		
		IButtonView MaxLevelButtonView { get; }
		
		IInventoryItemGradeDescriptionView UncommonGradeDescriptionView { get; }
		
		IInventoryItemGradeDescriptionView RareGradeDescriptionView { get; }
		
		IInventoryItemGradeDescriptionView LegendaryGradeDescriptionView { get; }

		void SetAttributeValueText(string text);

		void SetAttributeIcon(Sprite sprite);

		void SetTitleText(string text);
		
		void SetDescriptionText(string text);

		void SetLevelText(string text);

		void SetRarityText(string text);

		void SetRarityColor(Color color);

		void SetItemBackgroundGradientMaterial(Material material);

		void SetItemIcon(Sprite sprite);

		void SetGoldPriceText(string text);

		void SetBlueprintsPriceText(string text);

		void SetBlueprintsIcon(Sprite icon);

		void UpdateGradeLayout();
		
		void SetGradeLayoutActiveState(bool state);
	}
}