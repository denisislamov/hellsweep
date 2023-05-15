using System;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes
{
	public class ShopLootboxOpeningView : View, IShopLootboxOpeningView
	{
		[SerializeField]
		private PlayableDirector _playableDirector;

		[SerializeField] 
		private ShopLootboxItemCollectionView _shopLootboxItemCollectionView;
		
		[SerializeField] 
		private ButtonView _closeButtonView;
		
		[SerializeField] 
		private LayoutGroup _itemsLayout;
		
		[SerializeField] 
		private ContentSizeFitter _itemsLayoutContentSizeFitter;

		private readonly Subject<Unit> _scalingAnimationSubject = new Subject<Unit>();
		private readonly Subject<Unit> _openingAnimationSubject = new Subject<Unit>();

		public IShopLootboxItemCollectionView ShopLootboxItemCollectionView => _shopLootboxItemCollectionView;
		public IButtonView CloseButtonView => _closeButtonView;
		public IObservable<Unit> ScalingAnimationFinishedAsObservable => _scalingAnimationSubject;
		public IObservable<Unit> OpeningAnimationFinishedAsObservable => _openingAnimationSubject;
		
		public void PlayAnimation()
		{
			_playableDirector.Play();
		}
		
		public void PauseAnimation()
		{
			_playableDirector.Pause();
		}
		
		public void SetPlayableDirectorActiveState(bool state)
		{
			_playableDirector.enabled = state;
		}
		
		public void SetItemsLayoutActiveState(bool state)
		{
			_itemsLayout.enabled = state;
		}
		
		public void SetItemsLayoutContentSizeFitterActiveState(bool state)
		{
			_itemsLayoutContentSizeFitter.enabled = state;
		}

		public void ScalingAnimationFinishedSignalHandler()
		{
			_scalingAnimationSubject?.OnNext(Unit.Default);
		}
		
		public void OpeningAnimationFinishedSignalHandler()
		{
			_openingAnimationSubject?.OnNext(Unit.Default);
		}
	}
}