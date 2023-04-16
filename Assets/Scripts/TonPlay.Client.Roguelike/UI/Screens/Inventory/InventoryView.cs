using TMPro;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory
{
	public class InventoryView : View, IInventoryView
	{
		[SerializeField]
		private ProfileBarView _profileBarView;
		
		[SerializeField]
		private NavigationMenuView _navigationMenuView;
		
		[SerializeField]
		private InventoryItemCollectionView _itemCollectionView;
		
		[SerializeField]
		private InventorySlotView _weaponSlotView;
		
		[SerializeField]
		private InventorySlotView _neckSlotView;
		
		[SerializeField]
		private InventorySlotView _armsSlotView;
		
		[SerializeField]
		private InventorySlotView _bodySlotView;
		
		[SerializeField]
		private InventorySlotView _beltSlotView;
		
		[SerializeField]
		private InventorySlotView _bootsSlotView;

		[SerializeField]
		private TMP_Text _attackText;
		
		[SerializeField]
		private TMP_Text _armorText;
		
		[SerializeField]
		private TMP_Text _healthText;
		
		[SerializeField]
		private InventorySortPanelView _sortPanelView;
		
		[SerializeField] 
		private ButtonView _sortButtonView;

		public IProfileBarView ProfileBarView => _profileBarView;
		public INavigationMenuView NavigationMenuView => _navigationMenuView;

		public IInventoryItemCollectionView ItemCollectionView => _itemCollectionView;
		
		public IInventorySlotView WeaponSlotView => _weaponSlotView;
		
		public IInventorySlotView NeckSlotView => _neckSlotView;
		
		public IInventorySlotView ArmsSlotView => _armsSlotView;
		
		public IInventorySlotView BodySlotView => _bodySlotView;
		
		public IInventorySlotView BeltSlotView => _beltSlotView;
		
		public IInventorySlotView BootsSlotView => _bootsSlotView;
		
		public IInventorySortPanelView SortPanelView => _sortPanelView;
		
		public IButtonView SortButtonView => _sortButtonView;

		public void SetAttackValueText(string text)
		{
			_attackText.SetText(text);
		}
		
		public void SetArmorValueText(string text)
		{
			_armorText.SetText(text);
		}
		
		public void SetHealthValueText(string text)
		{
			_healthText.SetText(text);
		}
	}
}