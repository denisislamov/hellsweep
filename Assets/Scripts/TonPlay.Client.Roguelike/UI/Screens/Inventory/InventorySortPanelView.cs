using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory
{
	public class InventorySortPanelView : View, IInventorySortPanelView
	{
		[SerializeField] 
		private ButtonView _rarityButtonView;
		
		[SerializeField]
		private ButtonView _slotButtonView;
		
		[SerializeField] 
		private ButtonView _levelButtonView;
		
		public IButtonView RarityButtonView => _rarityButtonView;
		public IButtonView SlotButtonView => _slotButtonView;
		public IButtonView LevelButtonView => _levelButtonView;
		
		public void Toggle()
		{
			gameObject.SetActive(!gameObject.activeSelf);
		}
	}
}