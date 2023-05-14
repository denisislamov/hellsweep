using System;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes.Interfaces
{
	public interface IShopLootboxOpeningView : IView
	{
		IObservable<Unit> OpeningAnimationFinishedAsObservable { get; }
		
		IShopLootboxItemCollectionView ShopLootboxItemCollectionView { get; }
		
		IButtonView CloseButtonView { get; }

		void PlayOpeningAnimation();
		
		void SetPlayableDirectorActiveState(bool state);
		
		void SetItemsLayoutActiveState(bool state);
		
		void SetItemsLayoutContentSizeFitterActiveState(bool state);
	}
}