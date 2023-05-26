using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.MyBag.Interfaces
{
	public interface IMyBagPopupView : IView
	{
		IButtonView CloseButtonView { get; }
		
		IButtonView ItemsButtonView { get; }
		
		IButtonView NFTButtonView { get; }
		
		IButtonView AllButtonView { get; }
		
		IMyBagNftPanelView NftPanelView { get; }
		
		IMyBagItemsPanelView ItemsPanelView { get; }
		
		void SetTitleText(string text);

		void RefreshLayout();
	}
}