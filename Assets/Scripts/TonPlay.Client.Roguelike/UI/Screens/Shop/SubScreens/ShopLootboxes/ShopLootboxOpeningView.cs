using System;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Playables;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes
{
	public class ShopLootboxOpeningView : View, IShopLootboxOpeningView
	{
		[SerializeField]
		private PlayableDirector _playableDirector;

		public IObservable<Unit> OpeningAnimationFinishedAsObservable => _playableDirector.OnFinishedPlayingAsObservable();
		
		public void PlayOpeningAnimation()
		{
			_playableDirector.Play();
		}
	}
}