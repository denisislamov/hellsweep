using System;
using System.Collections.Generic;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Shop;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes
{
	internal class ShopLootboxOpeningPresenter : Presenter<IShopLootboxOpeningView, IShopLootboxOpeningScreenContext>
	{
		private readonly IUIService _uiService;
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		private readonly ShopLootboxItemCollectionPresenter.Factory _shopLootboxItemCollectionPresenterFactory;
		private readonly IShopEmbeddedScreenStorage _embeddedScreenStorage;

		private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();

		private IReadOnlyList<IInventoryItemModel> _items = null;
		private bool _chestOpened;

		public ShopLootboxOpeningPresenter(
			IShopLootboxOpeningView view,
			IShopLootboxOpeningScreenContext context,
			IUIService uiService,
			IButtonPresenterFactory buttonPresenterFactory,
			ShopLootboxItemCollectionPresenter.Factory shopLootboxItemCollectionPresenterFactory,
			IShopEmbeddedScreenStorage embeddedScreenStorage)
			: base(view, context)
		{
			_uiService = uiService;
			_buttonPresenterFactory = buttonPresenterFactory;
			_shopLootboxItemCollectionPresenterFactory = shopLootboxItemCollectionPresenterFactory;
			_embeddedScreenStorage = embeddedScreenStorage;

			AddScalingAnimationSubscription();
			AddOpeningAnimationSubscription();
			AddItemsSubscription();
			AddCloseButtonPresenter();
		}
		
		private void AddItemsSubscription()
		{
			IDisposable itemsSubscription = null;
			itemsSubscription = Context.ItemsAsObservable.Subscribe(items =>
			{
				itemsSubscription?.Dispose();
				
				_items = items;

				AddItemCollectionPresenter(items);
				
				TryPlayScalingAnimation();
			});

			if (itemsSubscription != null)
			{
				_compositeDisposables.Add(itemsSubscription);
			}
		}

		public override void Show()
		{
			base.Show();
			
			View.PlayAnimation();
		}

		public override void Dispose()
		{
			_compositeDisposables.Dispose();
			base.Dispose();
		}
		
		private void AddOpeningAnimationSubscription()
		{
			View.OpeningAnimationFinishedAsObservable.Subscribe(_ => OpeningAnimationFinishedHandler()).AddTo(_compositeDisposables);
		}

		private void AddScalingAnimationSubscription()
		{
			View.ScalingAnimationFinishedAsObservable.Subscribe(_ => ScaleAnimationFinishedHandler()).AddTo(_compositeDisposables);
		}
		
		private void OpeningAnimationFinishedHandler()
		{
			_chestOpened = true;
			
			View.PauseAnimation();

			TryPlayScalingAnimation();
		}
		
		private void TryPlayScalingAnimation()
		{
			if (_chestOpened && _items != null)
			{
				View.PlayAnimation();
			}
		}

		private void AddItemCollectionPresenter(IReadOnlyList<IInventoryItemModel> items)
		{
			var presenter = _shopLootboxItemCollectionPresenterFactory.Create(
				View.ShopLootboxItemCollectionView, 
				new ShopLootboxItemCollectionContext(items));
			
			Presenters.Add(presenter);
		}

		private void ScaleAnimationFinishedHandler()
		{
			View.SetPlayableDirectorActiveState(false);
			View.SetItemsLayoutContentSizeFitterActiveState(false);
			View.SetItemsLayoutActiveState(false);
		}
		
		private void AddCloseButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.CloseButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(CloseButtonClickHandler)));
			
			Presenters.Add(presenter);
		}
		
		private void CloseButtonClickHandler()
		{
			if (_embeddedScreenStorage.Current != null)
			{
				_uiService.Close(_embeddedScreenStorage.Current, true);
			}
			
			_embeddedScreenStorage.Set(
				_uiService.Open<ShopLootboxesScreen, IShopLootboxesScreenContext>(new ShopLootboxesScreenContext(), true));
		}

		internal class Factory : PlaceholderFactory<IShopLootboxOpeningView, IShopLootboxOpeningScreenContext, ShopLootboxOpeningPresenter>
		{
		}
	}
}