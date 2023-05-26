using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using Zenject;
using Screen = TonPlay.Client.Common.UIService.Screen;

namespace TonPlay.Client.Roguelike.UI.Screens.MyBag
{
	public class MyBagPopupScreen : Screen<IScreenContext>
	{
		[SerializeField]
		private MyBagPopupView _view;

		[Inject]
		private void Construct(MyBagPopupPresenter.Factory factory)
		{
			Context.Screen = this;

			var presenter = factory.Create(_view, Context);
			Presenters.Add(presenter);
		}

		public class Factory : ScreenFactory<IScreenContext, MyBagPopupScreen>
		{
			public Factory(DiContainer container, Screen screenPrefab)
				: base(container, screenPrefab)
			{
			}
		}
	}
}