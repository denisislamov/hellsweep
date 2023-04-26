using TMPro;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.InventoryItemUpgrade.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Screens.InventoryItemUpgrade
{
	public class InventoryItemUpgradeView : View, IInventoryItemUpgradeView
	{
		[SerializeField] 
		private ButtonView _closeButtonView;
		
		[SerializeField] 
		private ButtonView _equipButtonView;
		
		[SerializeField] 
		private ButtonView _nextLevelUpgradeButtonView;
		
		[SerializeField] 
		private ButtonView _maxLevelUpgradeButtonView;
		
		[SerializeField] 
		private TMP_Text _attributeValueText;
		
		[SerializeField] 
		private TMP_Text _titleText;
		
		[SerializeField] 
		private TMP_Text _descriptionText;
		
		[SerializeField] 
		private TMP_Text _levelText;
		
		[SerializeField] 
		private TMP_Text _rarityText;
		
		[SerializeField] 
		private TMP_Text _goldPriceText;
		
		[SerializeField] 
		private TMP_Text _drawingPriceText;

		[SerializeField]
		private Image _attributeIconImage;
		
		[SerializeField]
		private Image _itemIconImage;
		
		[SerializeField]
		private RawImage _itemBackgroundImage;
		
		[SerializeField]
		private Image[] _rarityPanels;
		
		[SerializeField] 
		private InventoryItemGradeDescriptionView _uncommonGradeDescriptionView;
		
		[SerializeField] 
		private InventoryItemGradeDescriptionView _rareGradeDescriptionView;
		
		[SerializeField] 
		private InventoryItemGradeDescriptionView _legendaryGradeDescriptionView;
		
		[SerializeField]
		private LayoutGroup _gradesLayout;

		public IButtonView CloseButtonView => _closeButtonView;
		public IButtonView EquipButtonView => _equipButtonView;
		public IButtonView UpgradeButtonView => _nextLevelUpgradeButtonView;
		public IButtonView MaxLevelButtonView => _maxLevelUpgradeButtonView;
		public IInventoryItemGradeDescriptionView UncommonGradeDescriptionView => _uncommonGradeDescriptionView;
		public IInventoryItemGradeDescriptionView RareGradeDescriptionView => _rareGradeDescriptionView;
		public IInventoryItemGradeDescriptionView LegendaryGradeDescriptionView => _legendaryGradeDescriptionView;

		public void SetAttributeValueText(string text)
		{
			_attributeValueText.SetText(text);
		}
		
		public void SetAttributeIcon(Sprite sprite)
		{
			_attributeIconImage.sprite = sprite;
		}
		
		public void SetTitleText(string text)
		{
			_titleText.SetText(text);
		}
		
		public void SetDescriptionText(string text)
		{
			_descriptionText.SetText(text);
		}
		
		public void SetLevelText(string text)
		{
			_levelText.SetText(text);
		}
		
		public void SetRarityText(string text)
		{
			_rarityText.SetText(text);
		}
		
		public void SetRarityColor(Color color)
		{
			for (var i = 0; i < _rarityPanels.Length; i++)
			{
				_rarityPanels[i].color = color;
			}
		}
		
		public void SetItemBackgroundGradientMaterial(Material material)
		{
			_itemBackgroundImage.material = material;
		}
		
		public void SetItemIcon(Sprite sprite)
		{
			_itemIconImage.sprite = sprite;
		}

		public void SetGoldPriceText(string text)
		{
			_goldPriceText.SetText(text);
		}
		
		public void SetBlueprintsPriceText(string text)
		{
			_drawingPriceText.SetText(text);
		}
		
		public void UpdateGradeLayout()
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) _gradesLayout.transform);
		}
		
		public void SetGradeLayoutActiveState(bool state)
		{
			_gradesLayout.enabled = state;
		}
	}
}