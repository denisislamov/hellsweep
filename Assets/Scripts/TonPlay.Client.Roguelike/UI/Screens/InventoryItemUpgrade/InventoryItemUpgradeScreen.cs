using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.InventoryItemUpgrade.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using Zenject;
using Screen = TonPlay.Client.Common.UIService.Screen;

namespace TonPlay.Client.Roguelike.UI.Screens.InventoryItemUpgrade
{
	public class InventoryItemUpgradeScreen : Screen<IInventoryItemUpgradeScreenContext>
	{
		[SerializeField]
		private InventoryItemUpgradeView _view;

		[Inject]
		private void Construct(InventoryItemUpgradePresenter.Factory factory)
		{
			Context.Screen = this;

			var presenter = factory.Create(_view, Context);
			Presenters.Add(presenter);
		}

		public class Factory : ScreenFactory<IInventoryItemUpgradeScreenContext, InventoryItemUpgradeScreen>
		{
			public Factory(DiContainer container, Screen screenPrefab)
				: base(container, screenPrefab)
			{
			}
		}
	}
}