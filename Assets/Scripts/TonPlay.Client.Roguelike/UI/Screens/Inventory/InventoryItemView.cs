using Nox7atra.UIFigmaGradients;
using TMPro;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory
{
	public class InventoryItemView : CollectionItemView, IInventoryItemView
	{
		[SerializeField]
		private TMP_Text _panelText;

		[SerializeField]
		private Image _iconImage;

		[SerializeField]
		private Image _iconGroupImage;
		
		[SerializeField]
		private Image[] _backgroundPanels;
		
		[SerializeField]
		private GameObject[] _equippedObjects;
		
		[SerializeField]
		private RawImage _backgroundGradient;
		
		[SerializeField]
		private ButtonView _buttonView;

		public IButtonView ButtonView => _buttonView;
		
		public void SetBackgroundGradientMaterial(Material material)
		{
			_backgroundGradient.material = material;
		}
		
		public void SetPanelsColor(Color color)
		{
			for (var i = 0; i < _backgroundPanels.Length; i++)
			{
				var panel = _backgroundPanels[i];
				panel.color = color;
			}
		}

		public void SetItemSlotIcon(Sprite sprite)
		{
			_iconGroupImage.sprite = sprite;
		}
		
		public void SetItemIcon(Sprite sprite)
		{
			_iconImage.sprite = sprite;
		}
		
		public void SetPanelText(string text)
		{
			_panelText.SetText(text);
		}
		
		public void SetEquippedState(bool state)
		{
			for (var i = 0; i < _equippedObjects.Length; i++)
			{
				_equippedObjects[i].SetActive(state);
			}
		}
	}
}