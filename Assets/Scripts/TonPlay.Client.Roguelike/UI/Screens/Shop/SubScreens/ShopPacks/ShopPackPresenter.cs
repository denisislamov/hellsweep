using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks
{
	internal class ShopPackPresenter : Presenter<IShopPackView, IShopPackContext>
	{
		private readonly IUIService _uiService;
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		private readonly IMetaGameModelProvider _metaGameModelProvider;
		private readonly IRestApiClient _restApiClient;
		private readonly ShopPackItemCollectionPresenter.Factory _collectionPresenterFactory;
		
		private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();

		public ShopPackPresenter(
			IShopPackView view,
			IShopPackContext context,
			IUIService uiService,
			IButtonPresenterFactory buttonPresenterFactory,
			IMetaGameModelProvider metaGameModelProvider,
			IRestApiClient restApiClient,
			ShopPackItemCollectionPresenter.Factory collectionPresenterFactory)
			: base(view, context)
		{
			_uiService = uiService;
			_buttonPresenterFactory = buttonPresenterFactory;
			_metaGameModelProvider = metaGameModelProvider;
			_restApiClient = restApiClient;
			_collectionPresenterFactory = collectionPresenterFactory;

			InitView();
			AddCollectionPresenter();
			AddButtonPresenter();
		}

		public override void Show()
		{
			base.Show();
		}

		public override void Dispose()
		{
			_compositeDisposables.Dispose();
			base.Dispose();
		}
		
		private void InitView()
		{
			View.SetTitleText(Context.Title);
			View.SetPanelsColor(Context.MainColor);
			View.SetBackgroundGradientMaterial(Context.Gradient);
		}
		
		private void AddCollectionPresenter()
		{
			var presenter = _collectionPresenterFactory.Create(View.ItemCollectionView, new ShopPackItemCollectionContext(Context.ShopPackModel.Rewards));
			
			Presenters.Add(presenter);
		}
		
		private void AddButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.ButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(OnButtonClickHandler)));
			
			Presenters.Add(presenter);
		}
		
		private void OnButtonClickHandler()
		{
			Debug.Log($"Clicked shop pack with id {Context.ShopPackModel.Id}");
		}

		internal class Factory : PlaceholderFactory<IShopPackView, IShopPackContext, ShopPackPresenter>
		{
		}
	}
}