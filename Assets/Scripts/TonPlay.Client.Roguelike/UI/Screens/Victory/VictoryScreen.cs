using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.Victory.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using Zenject;
using Screen = TonPlay.Client.Common.UIService.Screen;

namespace TonPlay.Client.Roguelike.UI.Screens.Victory
{
	public class VictoryScreen : Screen<VictoryScreenContext>
	{
		[SerializeField]
		private VictoryView _view;

		[Inject]
		private void Construct(VictoryPresenter.Factory factory)
		{
			var presenter = factory.Create(_view, Context);
			Presenters.Add(presenter);
		}

		public class Factory : ScreenFactory<IVictoryScreenContext, VictoryScreen>
		{
			public Factory(DiContainer container, Screen screenPrefab)
				: base(container, screenPrefab)
			{
			}
		}
	}
}