using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.Shop.TransactionProcessing.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using Zenject;
using Screen = TonPlay.Client.Common.UIService.Screen;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.TransactionProcessing
{
	public class ShopTransactionProcessingScreen : Screen<IShopTransactionProcessingScreenContext>
	{
		[SerializeField]
		private ShopTransactionProcessingView _view;
		
		private ShopTransactionProcessingPresenter.Factory _factory;

		[Inject]
		private void Construct(ShopTransactionProcessingPresenter.Factory factory)
		{
			_factory = factory;
		}

		public override void Open()
		{
			var presenter = _factory.Create(_view, Context);
			Presenters.Add(presenter);
			
			base.Open();
		}
		
		public class Factory : ScreenFactory<IShopTransactionProcessingScreenContext, ShopTransactionProcessingScreen>
		{
			public Factory(DiContainer container, Screen screenPrefab)
				: base(container, screenPrefab)
			{
			}
		}
	}
}