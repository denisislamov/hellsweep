using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Core.Match;
using TonPlay.Client.Roguelike.Core.Match.Interfaces;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Pause.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Pause
{
	public class PauseScreenPresenter : Presenter<IPauseScreenView, IPauseScreenContext>
	{
		private readonly IUIService _uiService;
		private readonly IMatchProvider _matchProvider;
		private readonly IGameModelProvider _gameModelProvider;
		private readonly ISkillConfigProvider _skillConfigProvider;
		private readonly IButtonPresenterFactory _buttonPresenterFactory;

		private readonly ReactiveProperty<bool> _buttonsLockedProperty = new ReactiveProperty<bool>();

		public PauseScreenPresenter(
			IPauseScreenView view,
			IPauseScreenContext context,
			IUIService uiService,
			IMatchProvider matchProvider,
			IGameModelProvider gameModelProvider,
			ISkillConfigProvider skillConfigProvider,
			IButtonPresenterFactory buttonPresenterFactory)
			: base(view, context)
		{
			_uiService = uiService;
			_matchProvider = matchProvider;
			_gameModelProvider = gameModelProvider;
			_skillConfigProvider = skillConfigProvider;
			_buttonPresenterFactory = buttonPresenterFactory;

			AddQuitButtonPresenter();
			AddContinueButtonPresenter();
			InitSkillsViews();
		}

		private void InitSkillsViews()
		{
			var attackSkills = 0;
			var defenceSkills = 0;

			for (var index = 0; index < View.AttackSkillItemViews.Length; index++)
			{
				var skillItemView = View.AttackSkillItemViews[index];
				skillItemView.SetColor(new Color(1f, 1f, 1f, 0f));
			}
			
			for (var index = 0; index < View.DefenceSkillItemViews.Length; index++)
			{
				var skillItemView = View.DefenceSkillItemViews[index];
				skillItemView.SetColor(new Color(1f, 1f, 1f, 0f));
			}

			foreach (var kvp in _gameModelProvider.Get().PlayerModel.SkillsModel.SkillLevels)
			{
				var skillName = kvp.Key;
				var currentLevel = kvp.Value;
				var config = _skillConfigProvider.Get(skillName);

				IPauseSkillItemView itemView = null;
				switch (config.SkillType)
				{
					case SkillType.UltimateDefence:
					case SkillType.Defence:
					{
						itemView = View.AttackSkillItemViews[defenceSkills];
						defenceSkills++;
						break;
					}
					case SkillType.Utility:
					{
						itemView = View.DefenceSkillItemViews[attackSkills];
						attackSkills++;
						break;
					}
					default:
						throw new ArgumentOutOfRangeException();
				}

				itemView?.SetIcon(config.Icon);
				itemView?.SetCurrentLevel(currentLevel);
				itemView?.SetMaxLevel(config.MaxLevel);
				itemView?.SetColor(Color.white);
			}
		}

		private void AddContinueButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.ContinueButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(ContinueButtonClickHandler))
				   .Add(new ReactiveLockButtonContext(_buttonsLockedProperty)));

			Presenters.Add(presenter);
		}

		private void AddQuitButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(
				View.QuitButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(QuitButtonClickHandler))
				   .Add(new ReactiveLockButtonContext(_buttonsLockedProperty)));

			Presenters.Add(presenter);
		}

		private void ContinueButtonClickHandler()
		{
			_uiService.Close(Context.Screen);

			Context?.ScreenClosedCallback?.Invoke();
		}

		private async void QuitButtonClickHandler()
		{
			_buttonsLockedProperty.SetValueAndForceNotify(true);
			
			var gameModel = _gameModelProvider.Get();
			var gainModel = gameModel.PlayerModel.MatchProfileGainModel;

			await _matchProvider.Current.FinishSession(
				new MatchResult(MatchResultType.Lose,
					gainModel.Gold.Value,
					gainModel.ProfileExperience.Value));

			await _matchProvider.Current.Finish();
		}

		public class Factory : PlaceholderFactory<IPauseScreenView, IPauseScreenContext, PauseScreenPresenter>
		{
		}
	}
}