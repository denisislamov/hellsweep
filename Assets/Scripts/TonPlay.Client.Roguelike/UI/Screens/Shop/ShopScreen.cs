using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using Zenject;
using Screen = TonPlay.Client.Common.UIService.Screen;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop
{
	public class ShopScreen : Screen<ShopScreenContext>
	{
		[SerializeField]
		private ShopView _view;
		
		private ShopPresenter.Factory _factory;

		[Inject]
		private void Construct(ShopPresenter.Factory factory)
		{
			_factory = factory;
		}

		public override void Open()
		{
			var presenter = _factory.Create(_view, Context);
			Presenters.Add(presenter);
			
			base.Open();
		}

		public class Factory : ScreenFactory<IShopScreenContext, ShopScreen>
		{
			public Factory(DiContainer container, Screen screenPrefab)
				: base(container, screenPrefab)
			{
			}
		}
	}
}