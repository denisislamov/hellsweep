using TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Interfaces;
using TonPlay.Roguelike.Client.UI.Screens.SkillChoice.Views;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using Zenject;
using Screen = TonPlay.Roguelike.Client.UI.UIService.Screen;

namespace TonPlay.Client.Roguelike.UI.Screens.SkillChoice
{
	public class SkillChoiceScreen : Screen<ISkillChoiceScreenContext>
	{
		[SerializeField]
		private SkillChoiceView _view;
		
		[Inject]
		private void Construct(SkillChoicePresenter.Factory factory)
		{
			var presenter = factory.Create(_view, Context);
			Presenters.Add(presenter);
		}

		public class Factory : ScreenFactory<ISkillChoiceScreenContext, SkillChoiceScreen>
		{
			public Factory(DiContainer container, Screen screenPrefab)
				: base(container, screenPrefab)
			{
			}
		}
	}
}