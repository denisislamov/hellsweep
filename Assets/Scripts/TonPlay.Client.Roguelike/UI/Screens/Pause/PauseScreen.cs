using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Pause.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using Zenject;
using Screen = TonPlay.Client.Common.UIService.Screen;

namespace TonPlay.Client.Roguelike.UI.Screens.Pause
{
	public class PauseScreen : Screen<IPauseScreenContext>
	{
		[SerializeField]
		private PauseScreenView _view;

		[Inject]
		private void Construct(PauseScreenPresenter.Factory factory)
		{
			Context.Screen = this;
			
			var presenter = factory.Create(_view, Context);
			Presenters.Add(presenter);
		}

		public class Factory : ScreenFactory<IPauseScreenContext, PauseScreen>
		{
			public Factory(DiContainer container, Screen screenPrefab)
				: base(container, screenPrefab)
			{
			}
		}
	}
}