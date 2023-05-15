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
		private readonly IMetaGameModelProvider _metaGameModelProvider;
		private readonly ShopLootboxItemCollectionPresenter.Factory _shopLootboxItemCollectionPresenterFactory;
		private readonly IShopEmbeddedScreenStorage _embeddedScreenStorage;

		private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();

		private IRestApiClient _restApiClient;

		public ShopLootboxOpeningPresenter(
			IShopLootboxOpeningView view,
			IShopLootboxOpeningScreenContext context,
			IUIService uiService,
			IButtonPresenterFactory buttonPresenterFactory,
			IMetaGameModelProvider metaGameModelProvider,
			IRestApiClient restApiClient,
			ShopLootboxItemCollectionPresenter.Factory shopLootboxItemCollectionPresenterFactory,
			IShopEmbeddedScreenStorage embeddedScreenStorage)
			: base(view, context)
		{
			_uiService = uiService;
			_buttonPresenterFactory = buttonPresenterFactory;
			_metaGameModelProvider = metaGameModelProvider;
			_restApiClient = restApiClient;
			_shopLootboxItemCollectionPresenterFactory = shopLootboxItemCollectionPresenterFactory;
			_embeddedScreenStorage = embeddedScreenStorage;

			AddAnimationSubscription();
			AddItemCollectionPresenter();
			AddCloseButtonPresenter();
		}

		public override void Show()
		{
			base.Show();
			
			View.PlayOpeningAnimation();
		}

		public override void Dispose()
		{
			_compositeDisposables.Dispose();
			base.Dispose();
		}
		
		private void AddAnimationSubscription()
		{
			View.OpeningAnimationFinishedAsObservable.Subscribe(_ => AnimationFinishedHandler()).AddTo(_compositeDisposables);
		}
		
		private void AddItemCollectionPresenter()
		{
			var presenter = _shopLootboxItemCollectionPresenterFactory.Create(
				View.ShopLootboxItemCollectionView, 
				new ShopLootboxItemCollectionContext(Context.Items));
			
			Presenters.Add(presenter);
		}

		private void AnimationFinishedHandler()
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