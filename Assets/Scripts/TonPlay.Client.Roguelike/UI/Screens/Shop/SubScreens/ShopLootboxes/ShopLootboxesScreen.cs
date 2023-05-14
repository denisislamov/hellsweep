using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using Zenject;
using Screen = TonPlay.Client.Common.UIService.Screen;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes
{
	public class ShopLootboxesScreen : Screen<ShopLootboxesScreenContext>
	{
		[SerializeField]
		private ShopLootboxesView _view;

		[Inject]
		private void Construct(ShopLootboxesPresenter.Factory factory)
		{
			Context.Screen = this;

			var presenter = factory.Create(_view, Context);
			Presenters.Add(presenter);
		}

		public class Factory : ScreenFactory<IShopLootboxesScreenContext, ShopLootboxesScreen>
		{
			public Factory(DiContainer container, Screen screenPrefab)
				: base(container, screenPrefab)
			{
			}
		}
	}
}