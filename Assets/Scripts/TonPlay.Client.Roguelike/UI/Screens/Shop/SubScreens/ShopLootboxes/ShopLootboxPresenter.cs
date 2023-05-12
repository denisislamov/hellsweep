using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes
{
	internal class ShopLootboxPresenter : Presenter<IShopLootboxView, IShopLootboxContext>
	{
		private readonly IUIService _uiService;
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		private readonly IMetaGameModelProvider _metaGameModelProvider;

		private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();

		private IRestApiClient _restApiClient;
		private ReactiveProperty<bool> _buttonLockReactiveProperty = new ReactiveProperty<bool>();
		private ReactiveProperty<string> _buttonTextReactiveProperty = new ReactiveProperty<string>();

		public ShopLootboxPresenter(
			IShopLootboxView view,
			IShopLootboxContext context,
			IUIService uiService,
			IButtonPresenterFactory buttonPresenterFactory,
			IMetaGameModelProvider metaGameModelProvider,
			IRestApiClient restApiClient)
			: base(view, context)
		{
			_uiService = uiService;
			_buttonPresenterFactory = buttonPresenterFactory;
			_metaGameModelProvider = metaGameModelProvider;
			_restApiClient = restApiClient;

			AddButtonPresenter();
			InitView();
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
			var metaGameModel = _metaGameModelProvider.Get();
			var keysCount = metaGameModel.ProfileModel.InventoryModel.ToData().GetKeysValue(Context.Rarity);
			_buttonLockReactiveProperty.SetValueAndForceNotify(keysCount < 1);
			_buttonTextReactiveProperty.SetValueAndForceNotify($"{keysCount}/1 {Context.Rarity.ToString().ToLowerInvariant().FirstCharToUpper()} Keys");
		}

		private void AddButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.ButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(ButtonClickHandler))
				   .Add(new ReactiveLockButtonContext(_buttonLockReactiveProperty))
				   .Add(new ReactiveTextButtonContext(_buttonTextReactiveProperty)));
			
			Presenters.Add(presenter);
		}
		
		private void ButtonClickHandler()
		{
			Debug.Log($"{Context.Rarity} Lootbox Clicked");
		}

		internal class Factory : PlaceholderFactory<IShopLootboxView, IShopLootboxContext, ShopLootboxPresenter>
		{
		}
	}
}