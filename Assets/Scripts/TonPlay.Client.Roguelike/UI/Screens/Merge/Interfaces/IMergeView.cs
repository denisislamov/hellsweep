using System.Collections.Generic;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Merge.Interfaces
{
    public interface IMergeView : IView
    {
        IProfileBarView ProfileBarView { get; }
        
        INavigationMenuView NavigationMenuView { get; }
        
        IButtonView GameSettingsButtonView { get; }
        
        IButtonView InventoryButtonView { get; }
        
        IInventorySortPanelView SortPanelView { get; }
        
        IButtonView SortButtonView { get; }
        
        IInventoryItemCollectionView ItemCollectionView { get; }
        
        IInventorySlotView[] Slots { get; }
        
        IButtonView MergeButtonView { get; }

        void SetDescriptionHeaderText(string text);
        
        void SetDescriptionInfoText(string text);
		
        void SetDescriptionValuesText(string text);
    }
}
