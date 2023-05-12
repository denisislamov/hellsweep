using System;
using Cysharp.Threading.Tasks;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes.Interfaces
{
	public interface IShopLootboxOpeningView : IView
	{
		IObservable<Unit> OpeningAnimationFinishedAsObservable { get; }
		
		void PlayOpeningAnimation();
	}
}