using System.Collections.Generic;
using TMPro;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Merge.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Screens.Merge
{
    public class MergeView : View, IMergeView
    {
        [SerializeField]
        private ProfileBarView _profileBarView;
		
        [SerializeField]
        private NavigationMenuView _navigationMenuView;
		
        [SerializeField]
        private ButtonView _inventoryButtonView;

        [SerializeField]
        private InventoryItemCollectionView _itemCollectionView;
        
        [SerializeField] 
        private InventorySlotView[] _slots;
    
        [SerializeField]
        private ButtonView _mergeButtonView;

        [SerializeField]
        private TMP_Text _descriptionHeaderText;
        
        [SerializeField]
        private TMP_Text _descriptionInfoText;
		
        [SerializeField]
        private TMP_Text _descriptionValuesText;
    
        [SerializeField]
        private InventorySortPanelView _sortPanelView;
        
        [SerializeField] 
        private ButtonView _sortButtonView;
		
        [SerializeField] 
        private ButtonView _gameSettingsButtonView;

        [SerializeField] 
        private Image _mergedItemView;

        [SerializeField] 
        private TMP_Text _selectItemText;
        
        [SerializeField] 
        private Image _glowImage;
        
        [SerializeField] 
        private GameObject _descriptionPanel;
        
        [SerializeField]
        private ParticleSystem _mergeParticles;
        
        public IProfileBarView ProfileBarView => _profileBarView;
        public INavigationMenuView NavigationMenuView => _navigationMenuView;
        public IInventoryItemCollectionView ItemCollectionView => _itemCollectionView;
        public IInventorySortPanelView SortPanelView => _sortPanelView;
        public IButtonView SortButtonView => _sortButtonView;
        public IButtonView GameSettingsButtonView => _gameSettingsButtonView;
        public IButtonView InventoryButtonView => _inventoryButtonView;
        public IInventorySlotView[] Slots => _slots;
        public IButtonView MergeButtonView => _mergeButtonView;
        public TMP_Text SelectItemText => _selectItemText;
        
        public Image GlowImage => _glowImage;
        
        public GameObject DescriptionPanel => _descriptionPanel;
        public ParticleSystem MergeParticles => _mergeParticles;
        
        public void SetMergedItemView(Sprite sprite)
        {
            _mergedItemView.sprite = sprite;
        }

        public Image MergedItemView => _mergedItemView;
       
        public void SetDescriptionHeaderText(string text)
        {
            _descriptionHeaderText.text = text;
        }

        public void SetDescriptionInfoText(string text)
        {
            _descriptionInfoText.text = text;
        }

        public void SetDescriptionValuesText(string text)
        {
            _descriptionValuesText.text = text;
        }
    }
}