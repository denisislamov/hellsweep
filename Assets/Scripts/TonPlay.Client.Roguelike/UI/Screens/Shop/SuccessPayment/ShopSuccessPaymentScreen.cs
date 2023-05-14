using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SuccessPayment.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using Zenject;
using Screen = TonPlay.Client.Common.UIService.Screen;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SuccessPayment
{
	public class ShopSuccessPaymentScreen : Screen<ShopSuccessPaymentScreenContext>
	{
		[SerializeField]
		private ShopSuccessPaymentPopupView _view;

		[Inject]
		private void Construct(ShopSuccessPaymentPresenter.Factory factory)
		{
			var presenter = factory.Create(_view, Context);
			Presenters.Add(presenter);
		}

		public class Factory : ScreenFactory<IShopSuccessPaymentScreenContext, ShopSuccessPaymentScreen>
		{
			public Factory(DiContainer container, Screen screenPrefab)
				: base(container, screenPrefab)
			{
			}
		}
	}
}