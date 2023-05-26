using System;
using System.Collections.Generic;
using System.Threading;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MyBag.Interfaces;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.MyBag
{
	internal class MyBagPopupPresenter : Presenter<IMyBagPopupView, IScreenContext>
	{
		private readonly IUIService _uiService;
		private readonly MyBagNftPanelPresenter.Factory _myBagNftPanelPresenterFactory;
		private readonly MyBagItemsPanelPresenter.Factory _myBagItemsPanelPresenterFactory;
		private readonly IButtonPresenterFactory _buttonPresenterFactory;

		private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();
		
		private ReactiveProperty<MyBagTabName> _currentNavTab = new ReactiveProperty<MyBagTabName>();
		
		private ReactiveProperty<bool> _itemsLockedReactiveProperty = new ReactiveProperty<bool>();
		private ReactiveProperty<bool> _nftLockedReactiveProperty = new ReactiveProperty<bool>();
		private ReactiveProperty<bool> _allLockedReactiveProperty = new ReactiveProperty<bool>();

		public MyBagPopupPresenter(
			IMyBagPopupView view,
			IScreenContext context,
			IUIService uiService,
			IButtonPresenterFactory buttonPresenterFactory,
			MyBagNftPanelPresenter.Factory myBagNftPanelPresenterFactory,
			MyBagItemsPanelPresenter.Factory myBagItemsPanelPresenterFactory)
			: base(view, context)
		{
			_uiService = uiService;
			_buttonPresenterFactory = buttonPresenterFactory;
			_myBagNftPanelPresenterFactory = myBagNftPanelPresenterFactory;
			_myBagItemsPanelPresenterFactory = myBagItemsPanelPresenterFactory;

			AddSubscriptionToCurrentNavTab();
			InitView();
			AddItemsPanelPresenter();
			AddNftPanelPresenter();
			AddItemsButtonPresenter();
			AddNftButtonPresenter();
			AddAllButtonPresenter();
			AddCloseButtonPresenter();
		}

		public override void Show()
		{
			base.Show();
			
			_currentNavTab.SetValueAndForceNotify(MyBagTabName.Items);
		}

		public override void Dispose()
		{
			_compositeDisposables.Dispose();
			base.Dispose();
		}
		
		private void AddSubscriptionToCurrentNavTab()
		{
			_currentNavTab.Subscribe(TabChangedHandler).AddTo(_compositeDisposables);
		}

		private void TabChangedHandler(MyBagTabName tabName)
		{
			_itemsLockedReactiveProperty.SetValueAndForceNotify(tabName == MyBagTabName.Items);	
			_nftLockedReactiveProperty.SetValueAndForceNotify(tabName == MyBagTabName.NFT);	
			_allLockedReactiveProperty.SetValueAndForceNotify(tabName == MyBagTabName.All);	
			
			if (tabName == MyBagTabName.Items || tabName == MyBagTabName.All)
			{
				View.ItemsPanelView.Show();
			}
			else
			{
				View.ItemsPanelView.Hide();
			}

			if (tabName == MyBagTabName.NFT) //todo: return this when nft is ready || tabName == MyBagTabName.All)
			{
				View.NftPanelView.Show();
			}
			else
			{
				View.NftPanelView.Hide();
			}
			
			View.RefreshLayout();
		}
		
		private void InitView()
		{
			View.SetTitleText("My Bag");
		}
		
		private void AddItemsPanelPresenter()
		{
			var presenter = _myBagItemsPanelPresenterFactory.Create(View.ItemsPanelView, ScreenContext.Empty);
			
			Presenters.Add(presenter);
		}
		
		private void AddNftPanelPresenter()
		{
			var presenter = _myBagNftPanelPresenterFactory.Create(
				View.NftPanelView,
				new MyBagNftPanelContext(() => _currentNavTab.SetValueAndForceNotify(MyBagTabName.Items)));
			
			Presenters.Add(presenter);
		}
		
		private void AddItemsButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.ItemsButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(() => _currentNavTab.SetValueAndForceNotify(MyBagTabName.Items)))
				   .Add(new ReactiveLockButtonContext(_itemsLockedReactiveProperty)));
			
			Presenters.Add(presenter);
		}
		
		private void AddNftButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.NFTButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(() => _currentNavTab.SetValueAndForceNotify(MyBagTabName.NFT)))
				   .Add(new ReactiveLockButtonContext(_nftLockedReactiveProperty)));
			
			Presenters.Add(presenter);
		}
		
		private void AddAllButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.AllButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(() => _currentNavTab.SetValueAndForceNotify(MyBagTabName.All)))
				   .Add(new ReactiveLockButtonContext(_allLockedReactiveProperty)));
			
			Presenters.Add(presenter);
		}
		
		private void AddCloseButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.CloseButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(OnCloseButtonClickHandler)));
			
			Presenters.Add(presenter);
		}
		
		private void OnCloseButtonClickHandler()
		{
			_uiService.Close(Context.Screen);
		}

		internal class Factory : PlaceholderFactory<IMyBagPopupView, IScreenContext, MyBagPopupPresenter>
		{
		}
	}
}